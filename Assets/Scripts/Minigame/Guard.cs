using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Crayon;
using Pickup.Player;
using TMPro;
using UnityEngine;

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

	float viewAngle;
	float playerVisibleTimer;
	

	public Transform pathHolder;
	private GameObject player;
	private Transform playerTransform;
	Color originalSpotlightColour;

	private Vector3[] waypoints;
	private int targetWaypointIndex = 0;
	[SerializeField] private bool isFlying = false;

	[SerializeField] private GameObject crayon;
	[SerializeField] private CrayonNumber[] crayonColour;
	[SerializeField] private Vector3[] spawnLocation;
	public static int numbTaken;
	

	[SerializeField] private Vector3 jailLocation;
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;
		viewAngle = spotlight.spotAngle;
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
		GuardMovement();
	}

	
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			Lose();
		}
	}
	// ReSharper disable Unity.PerformanceAnalysis
	private void Lose()
	{
		GameObject head = GameObject.Find("Head");
		 //Script to make playerColor lose crayon on lose
		var playerCrayons = ItemManager.numbCarried;
		for (int i = 0; i < playerCrayons.Length; i++)
		{
			if (playerCrayons[i] > 0)
			{
				playerCrayons[i]--;
				var playerScript = player.GetComponent<ItemManager>();
				
				playerScript.CrayonProgress--;
				playerScript.UpdateValues();
				GameObject createdCrayon = Instantiate(crayon);
				createdCrayon.transform.position = spawnLocation[numbTaken];
				createdCrayon.GetComponent<CrayonDisplay>().crayon = crayonColour[i];
				createdCrayon.GetComponent<CrayonDisplay>().isSpinning = true;
				Renderer rend = createdCrayon.GetComponent<Renderer>();
				rend.enabled = true;
				rend.sharedMaterial = crayonColour[i].colour[0];
				numbTaken++;
				if (numbTaken >= spawnLocation.Length) numbTaken = 0;
				
				transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
				break;
			}
		}
		//get head pos
		head.transform.position = jailLocation;
		playerTransform.position = jailLocation;
		Physics.SyncTransforms();
	}
	
	private void GuardMovement()
	{
		if (CanSeePlayer())
		{
			playerVisibleTimer += Time.deltaTime;
			Vector3 chaseVector = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
			if (isFlying)
				chaseVector =  playerTransform.position;
			transform.position = Vector3.MoveTowards(transform.position, chaseVector, speed * Time.deltaTime);
			transform.LookAt(playerTransform);
		}
		else if (playerVisibleTimer > 0.1f)
		{
			playerVisibleTimer -= Time.deltaTime;
			if (isFlying)
				transform.rotation = Quaternion.Euler(90,0,0); 
			transform.Rotate(0, turnSpeed*Time.deltaTime, 0);
		}
		else if(playerVisibleTimer <= 0.1f)
		{ 
			playerVisibleTimer -= Time.deltaTime;
			transform.position =
				Vector3.MoveTowards(transform.position, waypoints[targetWaypointIndex],
					Time.deltaTime * speed);
			if (Vector3.Distance(waypoints[targetWaypointIndex], transform.position) < .1f)
			{
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				if (!isFlying)
					StartCoroutine(TurnToFace(waypoints[targetWaypointIndex]));
			}
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
