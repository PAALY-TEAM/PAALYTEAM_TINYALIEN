using Interfaces.ColourChange;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Lava : MonoBehaviour, IColourChange
{
    float _lowestPoint = 3.5f;
    float _lavaHeight;
    bool _raiseLowerToggle = true;
    [FormerlySerializedAs("LavaSpeed")] [SerializeField] float lavaSpeed = 0.2f;
    [FormerlySerializedAs("LavaHeighObject")] [SerializeField] GameObject lavaHeighObject;
    private bool isColoured = false;
    
        
    
    public void ColourChange(int colourIndex)
    {
        // aka if the colour is red
        if (colourIndex == 0)
        {
            //Lave has been Coloured and is active
            isColoured = true;
            //Starts dialogue telling player that the lava is rising and dangerous
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
        
    }
    void Start()
    {
        //Finds Y difference between Lava and lavaHeightObject to find nummber to scale lava to
        _lavaHeight = (lavaHeighObject.transform.position.y - transform.position.y)*2;
        Debug.Log(_lavaHeight);
    
    }

    // Update is called once per frame
    void Update()
    {
        // If lava is at top point lower
        if (transform.localScale.y >= _lavaHeight)
        {
            _raiseLowerToggle = false;
        }
        //If lava is at bot raise
        if (transform.localScale.y <= _lowestPoint)
        {
            _raiseLowerToggle = true;
        }
        if (isColoured)
        {
            RaiseLower();
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

    void RaiseLower()
    {
        //Raises the lava
        if (transform.localScale.y < _lavaHeight && _raiseLowerToggle)
        {
            transform.localScale += new Vector3(0, lavaSpeed * Time.deltaTime, 0);
        }
        //Lowers the lava
        if (!_raiseLowerToggle)
        {
            transform.localScale -= new Vector3(0, lavaSpeed * Time.deltaTime, 0);
        }
    }
    
}
