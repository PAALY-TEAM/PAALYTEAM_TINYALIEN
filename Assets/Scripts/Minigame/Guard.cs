using System.Collections;
using Pickup.Crayon;
using Pickup.Player;
using UI;
using UnityEngine;

namespace Minigame
{
	public class Guard : MonoBehaviour {
	
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

		[SerializeField] private GameObject crayon;
		[SerializeField] private CrayonNumber[] crayonColour;
		[SerializeField] private Vector3[] spawnLocation;
		public static int NumbTaken;
	

		[SerializeField] private Vector3 jailLocation;

		private void Start()
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_playerTransform = _player.transform;
			_viewAngle = spotlight.spotAngle;
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
			GuardMovement();
		}

	
		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				Lose();
			}
		}
		// ReSharper disable Unity.PerformanceAnalysis
		private void Lose()
		{
			var head = GameObject.Find("Head");
			//Script to make playerColor lose crayon on lose
			var playerCrayons = ItemManager.NumbCarried;
			for (var i = 0; i < playerCrayons.Length; i++)
			{
				if (playerCrayons[i] > 0)
				{
					playerCrayons[i]--;
					var playerScript = _player.GetComponent<ItemManager>();
				
					playerScript.CrayonProgress--;
					playerScript.UpdateValues();
					var createdCrayon = Instantiate(crayon);
					createdCrayon.transform.position = spawnLocation[NumbTaken];
					createdCrayon.GetComponent<CrayonDisplay>().crayon = crayonColour[i];
					createdCrayon.GetComponent<CrayonDisplay>().isSpinning = true;
					var rend = createdCrayon.GetComponent<Renderer>();
					rend.enabled = true;
					rend.sharedMaterial = crayonColour[i].colour[0];
					NumbTaken++;
					if (NumbTaken >= spawnLocation.Length) NumbTaken = 0;
				
					transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
					break;
				}
			}
			//get head pos
			head.transform.position = jailLocation;
			_playerTransform.position = jailLocation;
			Physics.SyncTransforms();
		}
	
		private void GuardMovement()
		{
			if (CanSeePlayer())
			{
				_playerVisibleTimer += Time.deltaTime;
				var playerTransformPosition = _playerTransform.position;
				var chaseVector = new Vector3(playerTransformPosition.x, transform.position.y, playerTransformPosition.z);
				if (isFlying)
					chaseVector =  _playerTransform.position;
				transform.position = Vector3.MoveTowards(transform.position, chaseVector, speed * Time.deltaTime);
				transform.LookAt(_playerTransform);
			}
			else if (_playerVisibleTimer > 0.1f)
			{
				_playerVisibleTimer -= Time.deltaTime;
				if (isFlying)
					transform.rotation = Quaternion.Euler(90,0,0); 
				transform.Rotate(0, turnSpeed*Time.deltaTime, 0);
			}
			else if(_playerVisibleTimer <= 0.1f)
			{ 
				_playerVisibleTimer -= Time.deltaTime;
				transform.position =
					Vector3.MoveTowards(transform.position, _waypoints[_targetWaypointIndex],
						Time.deltaTime * speed);
				if (Vector3.Distance(_waypoints[_targetWaypointIndex], transform.position) < .1f)
				{
					_targetWaypointIndex = (_targetWaypointIndex + 1) % _waypoints.Length;
					if (!isFlying)
						StartCoroutine(TurnToFace(_waypoints[_targetWaypointIndex]));
				}
			}
			_playerVisibleTimer = Mathf.Clamp (_playerVisibleTimer, 0, timeToSpotPlayer);
			spotlight.color = Color.Lerp (_originalSpotlightColour, Color.red, _playerVisibleTimer / timeToSpotPlayer);
		}

		private bool CanSeePlayer() {
			if (Vector3.Distance(transform.position,_playerTransform.position) < viewDistance) {
				var cachedTransform = transform;
				var dirToPlayer = (_playerTransform.position - cachedTransform.position).normalized;
				var angleBetweenGuardAndPlayer = Vector3.Angle (cachedTransform.forward, dirToPlayer);
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
				var position = waypoint.position;
				Gizmos.DrawSphere (position, .3f);
				Gizmos.DrawLine (previousPosition, position);
				previousPosition = position;
			}
			Gizmos.DrawLine (previousPosition, startPosition);

			Gizmos.color = Color.red;
			var cachedTransform = transform;
			Gizmos.DrawRay (cachedTransform.position, cachedTransform.forward * viewDistance);
		}

	}
}
