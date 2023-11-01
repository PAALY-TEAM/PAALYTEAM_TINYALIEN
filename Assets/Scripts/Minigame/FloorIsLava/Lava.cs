using UnityEngine;
using UnityEngine.Serialization;

namespace Minigame.FloorIsLava
{
    public class Lava : MonoBehaviour
    {
        private float _lowestPoint = 3.5f;
        private float _lavaHeight;
        private bool _raiseLowerToggle = true;
        [FormerlySerializedAs("LavaSpeed")] [SerializeField]
        private float lavaSpeed = 0.2f;
        [FormerlySerializedAs("LavaHeighObject")] [SerializeField]
        private GameObject lavaHeighObject;

        // Start is called before the first frame update
        private void Start()
        {
            //Finds Y difference between Lava and lavaHeightObject to find nummber to scale lava to
            _lavaHeight = (lavaHeighObject.transform.position.y - transform.position.y)*2;
            Debug.Log(_lavaHeight);
        
        }

        // Update is called once per frame
        private void Update()
        {
        
            // If lava is at top point lower
            if(transform.localScale.y >= _lavaHeight)
            {
                _raiseLowerToggle = false;
            
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
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Dead");
            }

        }

        private void RaiseLower()
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
