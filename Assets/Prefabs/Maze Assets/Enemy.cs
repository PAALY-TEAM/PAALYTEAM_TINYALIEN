using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public GameObject laser;
    bool cooldown = false;
    private GameObject player;
    int range = 20;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if reached set point and finding new point
        if (agent.remainingDistance < 1)
            agent.SetDestination(RandomNavmeshLocation(range));
    }

    // Finding random point in a sphere for where to go
    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;

    }

    // Checking for player and following player
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && cooldown == false)
        {
            cooldown = true;
            agent.SetDestination(player.transform.position);
            Invoke("CooldownTimer", 0.5f);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var crayPositions = GameObject.Find("SpawnLocation").GetComponent<SpawnLocation>().crayonLocation;
            var playerPosition = GameObject.Find("SpawnLocation").GetComponent<SpawnLocation>().playerLocation;
            player.GetComponent<LoseGame>().Lose(crayPositions, new Vector3(-22.6f, 0f, 32.6f));
        }
    }
    private void CooldownTimer()
    {
        cooldown = false;
    }
}
