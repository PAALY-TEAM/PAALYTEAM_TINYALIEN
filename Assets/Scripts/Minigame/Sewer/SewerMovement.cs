using Interfaces.ColourChange;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class SewerMovement : MonoBehaviour
{
    float _lowestPoint;
    float _sewerHeight;
    bool _raiseLowerToggle = true;
    [SerializeField] float sewerSpeed = 0.2f;
    [SerializeField] GameObject sewerHeightObject;
    private bool isColoured = false;


    void Start()
    {
        //Finds Y difference between sewer and sewerHeightObject to find nummber to scale sewer to
        _sewerHeight = (sewerHeightObject.transform.position.y - transform.position.y);
        _lowestPoint = transform.localScale.y;
        Debug.Log(_sewerHeight);

    }

    // Update is called once per frame
    void Update()
    {
        // If sewer is at top point lower
        if (transform.localScale.y >= _sewerHeight)
        {
            _raiseLowerToggle = false;
        }
        //If sewer is at bot raise
        if (transform.localScale.y <= _lowestPoint)
        {
            _raiseLowerToggle = true;
        }


        RaiseLower();

    }

    [SerializeField] private Transform[] crayonSpawns;

    [SerializeField] private Transform playerSpawn;
    
    // Lose on hit with sewer
    private void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<LoseGame>().Lose(crayonSpawns, playerSpawn.position);
        }

    }

    void RaiseLower()
    {
        //Raises the sewer
        if (transform.localScale.y < _sewerHeight && _raiseLowerToggle)
        {
            transform.localScale += new Vector3(0, sewerSpeed * Time.deltaTime, 0);
        }
        //Lowers the sewer
        if (!_raiseLowerToggle)
        {
            transform.localScale -= new Vector3(0, sewerSpeed * Time.deltaTime, 0);
        }
    }

}
