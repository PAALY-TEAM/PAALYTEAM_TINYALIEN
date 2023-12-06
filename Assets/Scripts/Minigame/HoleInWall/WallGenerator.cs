using System.Collections;
using System.Collections.Generic;
using UI;
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
    private int currentWave;
    public float waveMoveSpeed;
    int waves;

    [SerializeField] private GameObject toHide;

    // Start is called before the first frame update
    void Start()
    {
        toHide.SetActive(false);
        Invoke("WallsAndDoors", 2);
    }

    void WallsAndDoors()
    {
        if (currentWave < wavesToWin)
        {
            currentWave++;
            int floorStart = -4;
            int doorSpace = Random.Range(-2, 2)*2;
            GameObject toSpawn;
            GameObject spawned;
            for(int i = -4; i < 5; i += 2)
            {
                
                if (doorSpace != i)
                     toSpawn = wall;
                
                else
                    toSpawn = door;
                
                spawned = Instantiate(toSpawn, new Vector3(i,4,0) + transform.position, Quaternion.Euler(90,0,0));
                Destroy(spawned,10f);
            }

        
            startSpawnSpeed = startSpawnSpeed/spawnSpeedMultiplier;
            Invoke("WallsAndDoors", startSpawnSpeed);
        }
        else
        {
            Invoke("Win", 6);
        }
        


    }

    void Win()
    {
        transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        toHide.SetActive(true);
    }
}
