using System.Collections;
using UnityEngine;

namespace Minigame.HideAndSeek
{
	public class Seeker : MonoBehaviour
	{
		public float speed = 5;
		public float waitTime = .3f;
		[Header("Quarter Turn per second")]
		public float turnSpeed;
		public float timeToSpotPlayer;

		public Light spotlight;
		public float viewDistance;
		[Header("Set to obstacle")]
		public LayerMask viewMask;

		private float _viewAngle;
		private float _playerVisibleTimer;
	

		public Transform pathHolder;
		private GameObject _player;
		private Transform _playerTransform;
		private Color _originalSpotlightColour;

		private Vector3[] _waypoints;
		private int _targetWaypointIndex;
		[SerializeField] private bool isFlying;


		public Vector3 idlePos;
	
	

		public static bool PlayerSpotted;

		private void Start()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_playerTransform = _player.transform;
			_viewAngle = spotlight.spotAngle;
			idlePos = transform.position;
			_originalSpotlightColour = spotlight.color;
			turnSpeed *= 90;

			_waypoints = new Vector3[pathHolder.childCount];
			for (var i = 0; i < _waypoints.Length; i++)
			{
				_waypoints[i] = pathHolder.GetChild(i).position;
				_waypoints[i] = new Vector3(_waypoints[i].x, transform.position.y, _waypoints[i].z);
			}
		}

		private void Update() {
			if (PlayerSpotted)
			{
				var chaseVector = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
				if (isFlying)
					chaseVector =  _playerTransform.position;
				transform.position = Vector3.MoveTowards(transform.position, chaseVector, speed * Time.deltaTime);
				transform.LookAt(_playerTransform);
			}
			else
				GuardMovement();
		}

	
		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				GameObject.Find("Timer").GetComponent<Timer>().gameOver = true;
				PlayerSpotted = false;
			}
		}
	
		private void GuardMovement()
		{
		
			transform.position =
				Vector3.MoveTowards(transform.position, _waypoints[_targetWaypointIndex],
					Time.deltaTime * speed);
			if (Vector3.Distance(_waypoints[_targetWaypointIndex], transform.position) < .1f)
			{
				_targetWaypointIndex = (_targetWaypointIndex + 1) % _waypoints.Length;
				if (!isFlying)
					StartCoroutine(TurnToFace(_waypoints[_targetWaypointIndex]));
			}

			if (CanSeePlayer())
			{
				PlayerSpotted = true;
			}
		
			_playerVisibleTimer = Mathf.Clamp (_playerVisibleTimer, 0, timeToSpotPlayer);
			spotlight.color = Color.Lerp (_originalSpotlightColour, Color.red, _playerVisibleTimer / timeToSpotPlayer);
		}

		private bool CanSeePlayer() {
			if (Vector3.Distance(transform.position,_playerTransform.position) < viewDistance) {
				var dirToPlayer = (_playerTransform.position - transform.position).normalized;
				var angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
				if (angleBetweenGuardAndPlayer < _viewAngle / 2f) {
					if (!Physics.Linecast (transform.position, _playerTransform.position, viewMask)) {
						return true;
					}
				}
			}
			return false;
		}


		private IEnumerator TurnToFace(Vector3 lookTarget) {
			var dirToLookTarget = (lookTarget - transform.position).normalized;
			var targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

			while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
				var angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
				transform.eulerAngles = Vector3.up * angle;
				yield return null;
			}
		}

		private void OnDrawGizmos() {
			var startPosition = pathHolder.GetChild (0).position;
			var previousPosition = startPosition;

			foreach (Transform waypoint in pathHolder) {
				Gizmos.DrawSphere (waypoint.position, .3f);
				Gizmos.DrawLine (previousPosition, waypoint.position);
				previousPosition = waypoint.position;
			}
			Gizmos.DrawLine (previousPosition, startPosition);

			Gizmos.color = Color.red;
			Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
		}
	}
}
