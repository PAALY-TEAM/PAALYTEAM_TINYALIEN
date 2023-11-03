using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] int holeSize = 2;
    public GameObject floor;
    public GameObject wall;
    public GameObject door;
    [SerializeField] float startSpawnSpeed;
    [SerializeField] float spawnSpeedMultiplier;
    [SerializeField] int wavesToWin;
    public float waveMoveSpeed;
    int waves;


    // Start is called before the first frame update
    void Start()
    {

        Invoke("WallsAndDoors", 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void WallsAndDoors()
    {
        int floorStart = -4;
        int doorSpace = Random.Range(-2, 2)*2;
        for(int i = -4; i < 5; i += 2) 
        {
            if (doorSpace != i)
            {
                Instantiate(wall, new Vector3(i,4,0), Quaternion.Euler(90,0,0));
            }

            else
            {
                Instantiate(door, new Vector3(i,4,0), Quaternion.Euler(90,0,0));
            }
        }

        
        startSpawnSpeed = startSpawnSpeed/spawnSpeedMultiplier;
        Invoke("WallsAndDoors", startSpawnSpeed);


    }

    void Win()
    {
        Debug.Log("Completed");
    }


}
