using System.Collections;
using Interaction;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLogic
{
    public class SceneSwapManager : MonoBehaviour
    {
        public static SceneSwapManager instance;

        private static bool _loadFromDoor;

        private GameObject _player;
        private Collider _playerColl;
        private Collider _doorColl;
        private Vector3 _playerSpawnPosition;

        private static DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            _player = GameObject.FindGameObjectWithTag("Player");
            _playerColl = _player.GetComponent<Collider>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static void SwapSceneFromDoorUse(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt)
        {
            _loadFromDoor = true;
            instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
        }
        //takes in scene field and a door to spawn at
        private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt)
        {
            //start fading to black 
            SceneFadeManager.instance.StartFadeOut();
            //keep fading out
            while (SceneFadeManager.instance.isFadingOut)
            {
                yield return null;
            }

            _doorToSpawnTo = doorToSpawnAt;
            SceneManager.LoadScene(myScene);
            
        
        }
        //CALLED WHENEVER A NEW SCENE IS LOADED (INCLUDING THE START OF THE GAME)
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneFadeManager.instance.StartFadeIn();
            if (_loadFromDoor)
            {
                
                //warp player to correct door location
                FindDoor(_doorToSpawnTo);
                _player.transform.position = _playerSpawnPosition;
                _player.transform.parent.Find("Head").position = _playerSpawnPosition;
                Physics.SyncTransforms();
                _loadFromDoor = false;
            }
        }

        private void FindDoor(DoorTriggerInteraction.DoorToSpawnAt doorSpawmNumber)
        {
            DoorTriggerInteraction[] doors = (DoorTriggerInteraction[]) FindObjectsOfType(typeof(DoorTriggerInteraction)) ;
            
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].CurrentDoorPosition == doorSpawmNumber)
                {
                    _doorColl = doors[i].gameObject.GetComponent<Collider>();
                
                    //calculate spawn position
                    CalculateSpawnPosition();
                    return;  
                }
            
            }
        }

        private void CalculateSpawnPosition()
        {
            float colliderHeight = _playerColl.bounds.extents.y;
            _playerSpawnPosition = _doorColl.transform.position ;
        }
    }
}
