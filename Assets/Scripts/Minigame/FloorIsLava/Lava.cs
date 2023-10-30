using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour, IColourChange
{
    float LowestPoint = 3.5f;
    float LavaHeight;
    bool RaiseLowerToggle = true;
    [SerializeField] float LavaSpeed = 0.2f;
    [SerializeField] GameObject LavaHeighObject;
    private bool isColoured = false;

    // Start is called before the first frame update
    void Start()
    {
        //Finds Y difference between Lava and lavaHeightObject to find nummber to scale lava to
        LavaHeight = (LavaHeighObject.transform.position.y - transform.position.y)*2;
        Debug.Log(LavaHeight);
        
    }

    public void ColourChange()
    {
        isColoured = true;
        transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
    }
    // Update is called once per frame
    void Update()
    {
        
        // If lava is at top point lower
        if(transform.localScale.y >= LavaHeight)
        {
            RaiseLowerToggle = false;
            
        }
        //If lava is at bot raise
        if (transform.localScale.y <= LowestPoint)
        {
            RaiseLowerToggle = true;
        }

        if (isColoured)
        {
            raiseLower();
        }
        

    }
    // Die on hit with lava
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isColoured)
        {
            Debug.Log("Dead");
        }

    }

    void raiseLower()
    {
        //Raises the lava
        if (transform.localScale.y < LavaHeight && RaiseLowerToggle)
        {
            transform.localScale += new Vector3(0, LavaSpeed * Time.deltaTime, 0);
        }
        //Lowers the lava
        if (!RaiseLowerToggle)
        {
            transform.localScale -= new Vector3(0, LavaSpeed * Time.deltaTime, 0);
        }

    }




}
