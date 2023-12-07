using Interfaces.ColourChange;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class SewerMovement : MonoBehaviour
{
    float _lowestPoint = 1f;
    float _lavaHeight;
    bool _raiseLowerToggle = true;
    [SerializeField] float sewerSpeed = 0.2f;
    [SerializeField] GameObject sewerHeightObject;
    private bool isColoured = false;


    void Start()
    {
        //Finds Y difference between Lava and lavaHeightObject to find nummber to scale lava to
        _lavaHeight = (sewerHeightObject.transform.position.y - transform.position.y);
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

    void RaiseLower()
    {
        //Raises the lava
        if (transform.localScale.y < _lavaHeight && _raiseLowerToggle)
        {
            transform.localScale += new Vector3(0, sewerSpeed * Time.deltaTime, 0);
        }
        //Lowers the lava
        if (!_raiseLowerToggle)
        {
            transform.localScale -= new Vector3(0, sewerSpeed * Time.deltaTime, 0);
        }
    }

}
