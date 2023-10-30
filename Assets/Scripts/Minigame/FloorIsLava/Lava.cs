using UnityEngine;
using UnityEngine.Serialization;

<<<<<<< HEAD
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
=======
namespace Minigame.FloorIsLava
{
    public class Lava : MonoBehaviour
>>>>>>> 6fda68824c1579a99dbb80970c19fa3731b86fca
    {
        float _lowestPoint = 3.5f;
        float _lavaHeight;
        bool _raiseLowerToggle = true;
        [FormerlySerializedAs("LavaSpeed")] [SerializeField] float lavaSpeed = 0.2f;
        [FormerlySerializedAs("LavaHeighObject")] [SerializeField] GameObject lavaHeighObject;

<<<<<<< HEAD
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
=======
        // Start is called before the first frame update
        void Start()
>>>>>>> 6fda68824c1579a99dbb80970c19fa3731b86fca
        {
            //Finds Y difference between Lava and lavaHeightObject to find nummber to scale lava to
            _lavaHeight = (lavaHeighObject.transform.position.y - transform.position.y)*2;
            Debug.Log(_lavaHeight);
        
        }

        // Update is called once per frame
        void Update()
        {
        
            // If lava is at top point lower
            if(transform.localScale.y >= _lavaHeight)
            {
                _raiseLowerToggle = false;
            
<<<<<<< HEAD
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
=======
            }

            //If lava is at bot raise
            if (transform.localScale.y <= _lowestPoint)
            {
                _raiseLowerToggle = true;
            }

   

            RaiseLower();

        }
        // Die on hit with lava
        private void OnTriggerEnter(Collider other)
>>>>>>> 6fda68824c1579a99dbb80970c19fa3731b86fca
        {
            if (other.CompareTag("Player"))
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
}
