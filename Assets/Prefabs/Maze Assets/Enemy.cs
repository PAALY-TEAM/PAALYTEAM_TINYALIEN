using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public GameObject laser;
    bool cooldown = false;
    int range = 20;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if reached set point and finding new point
        Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance < 1)
            agent.SetDestination(RandomNavmeshLocation(range));
    }

    // Finding random point in a sphere for where to go
    public Vector3 RandomNavmeshLocation(float radius)
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
            agent.SetDestination(other.transform.position);
            Invoke("cooldownTimer", 0.5f);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var crayPositions = GameObject.Find("SpawnLocation").GetComponent<SpawnLocation>().crayonLocation;
            var playerPosition = GameObject.Find("SpawnLocation").GetComponent<SpawnLocation>().playerLocation;
            collision.gameObject.GetComponent<LoseGame>().Lose(crayPositions, new Vector3(-22.6f, 0f, 32.6f));
        }
    }




    public void cooldownTimer()
    {
        cooldown = false;
    }

}
