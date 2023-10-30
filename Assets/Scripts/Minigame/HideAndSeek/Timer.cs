using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minigame.HideAndSeek
{
    public class Timer : MonoBehaviour
    {
        [Header("Time the player has to hide and another for how long the game should last")]
        [SerializeField] private float insertHideTime;
        [SerializeField] private float insertPlayTime;
        private float _timeToHide;
        private float _timeToPlay;
        private TextMeshProUGUI _timer;

        [SerializeField] private GameObject seekers;
    
    
        [HideInInspector] public bool gameOver = false;
        private bool _gameStart = false;

        [FormerlySerializedAs("EndPos")] [SerializeField] private Vector3 endPos;

        private GameObject _guard;
        private GameObject _crayon;

        [SerializeField] private GameObject crayonPrefab;
        [SerializeField] private Vector3[] crayonInstPlace;
    
    
        private void Start()
        {
            _guard = GameObject.Find("Guards");
            _crayon = GameObject.Find("Crayons");
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.Z))
            {
                gameOver = true;
            }

            if (_gameStart)
            {
                bool activeSeeker = false;
                if (_timeToHide > 0)
                {
                    //Decreases timer and displays whole number
                    _timeToHide -= Time.deltaTime;
                    var currentTime = _timeToHide.ToString("F0");
                    _timer.text = currentTime;
                }
                else if (_timeToPlay > 0)
                {
                    if (!activeSeeker)
                    {
                        GangComponents(true);
                        activeSeeker = true;
                    }
                    //Decreases timer and displays whole number
                    _timeToPlay -= Time.deltaTime;
                    var currentTime = _timeToPlay.ToString("F0");
                    _timer.text = currentTime;

                    if (gameOver)
                    {
                        Lose();
                    
                        _timeToPlay = 0;
                    }
                }
                else
                {
                    Win();
                }
            }

        }
        public void StartHideAndSeek()
        {
            //Get timer component
            _timer = GetComponent<TextMeshProUGUI>();
            //Initialize timers
            _timeToHide = insertHideTime;
            _timeToPlay = insertPlayTime;
            //TempRemove Guards & Crayons
            _guard.SetActive(false);
            _crayon.SetActive(false);
            //StartsGame when everything is initialized
            _gameStart = true;
        }

        private void Win()
        {
            //Could spawn Victory or game completion screen
            _gameStart = false;
            ReturnToStart(true);
        }
        private void Lose()
        {
            //Could Spawn in lose screen with retry and exit button
            gameOver = false;
            _gameStart = false;
            ReturnToStart(false);
        }

        private void ReturnToStart(bool won)
        {
            GangComponents(false);
        
            GameObject.FindGameObjectWithTag("Player").transform.position = endPos;
            Physics.SyncTransforms();
            //Bring back Guards and Crayons
            _guard.SetActive(true);
            _crayon.SetActive(true);
            if (won)
            {
                foreach (var pos in crayonInstPlace)
                {
                    Instantiate(crayonPrefab, pos, Quaternion.identity);
                }
            }
        }

        private void GangComponents(bool b)
        {
            for (int i = 0; i < 4; i++)
            {
                var currentChild = seekers.transform.GetChild(i);
                currentChild.GetComponent<Light>().enabled = b;
                currentChild.GetComponent<Seeker>().enabled = b;

                if (!b)
                {
                    currentChild.position = currentChild.GetComponent<Seeker>().idlePos;
                    Physics.SyncTransforms();
                }
            }
        }
    }
}
