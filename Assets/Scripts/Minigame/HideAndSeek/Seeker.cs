using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Crayon;
using Pickup.Player;
using TMPro;
using UnityEngine;
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

	float viewAngle;
	float playerVisibleTimer;
	

	public Transform pathHolder;
	private GameObject player;
	private Transform playerTransform;
	Color originalSpotlightColour;

	private Vector3[] waypoints;
	private int targetWaypointIndex = 0;
	[SerializeField] private bool isFlying = false;


	public Vector3 idlePos;
	
	

	public static bool playerSpotted = false;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;
		viewAngle = spotlight.spotAngle;
		idlePos = transform.position;
		originalSpotlightColour = spotlight.color;
		turnSpeed *= 90;

		waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
			waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
		}
	}

	void Update() {
		if (playerSpotted)
		{
			Vector3 chaseVector = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
			if (isFlying)
				chaseVector =  playerTransform.position;
			transform.position = Vector3.MoveTowards(transform.position, chaseVector, speed * Time.deltaTime);
			transform.LookAt(playerTransform);
		}
		else
			GuardMovement();
	}

	
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			GameObject.Find("Timer").GetComponent<Timer>().gameOver = true;
			playerSpotted = false;
		}
	}
	
	private void GuardMovement()
	{
		
		transform.position =
			Vector3.MoveTowards(transform.position, waypoints[targetWaypointIndex],
				Time.deltaTime * speed);
		if (Vector3.Distance(waypoints[targetWaypointIndex], transform.position) < .1f)
		{
			targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
			if (!isFlying)
				StartCoroutine(TurnToFace(waypoints[targetWaypointIndex]));
		}

		if (CanSeePlayer())
		{
			playerSpotted = true;
		}
		
		playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp (originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);
	}
	bool CanSeePlayer() {
		if (Vector3.Distance(transform.position,playerTransform.position) < viewDistance) {
			Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
				if (!Physics.Linecast (transform.position, playerTransform.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}

	

	IEnumerator TurnToFace(Vector3 lookTarget) {
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos() {
		Vector3 startPosition = pathHolder.GetChild (0).position;
		Vector3 previousPosition = startPosition;

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
