using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject laser;
    bool cooldown = false;
    public GameObject player;
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
            Debug.Log(player.transform.position);
            cooldown = true;
            agent.SetDestination(player.transform.position);
            Invoke("cooldownTimer", 0.5f);
            
        }
    }


    public void cooldownTimer()
    {
        cooldown = false;
    }

}
