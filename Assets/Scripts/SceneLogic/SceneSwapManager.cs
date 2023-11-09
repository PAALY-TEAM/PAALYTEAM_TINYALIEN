 using System.Collections;
 using Pickup.Player;
 using UI;
 using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;
    
    private static bool _loadFromDoor;
    private static DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;

    private GameObject _player;
    private Collider _playerColl;
    private Collider _doorColl;
    private Vector3 _playerSpawnPosition;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }
    //Takes in scene field and a door to spawn at
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt = DoorTriggerInteraction.DoorToSpawnAt.None)
    {
        //Activate TempDisableMovement script
        _player.GetComponent<TempDisableMovement>().enabled = true;
        
        //start fading to black 
        SceneFadeManager.Instance.StartFadeOut();
        //keep fading out
        while (SceneFadeManager.Instance.IsFadingOut)
        {
            yield return null;
        }
        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }
    private IEnumerator ActivatePlayerControlsAfterFadeIn()
    {
        //wait for fade in to finish
        while (SceneFadeManager.Instance.IsFadingIn)
        {
            yield return null;
        }
        //Activate TempDisableMovement script
        _player.GetComponent<TempDisableMovement>().enabled = false;
    }
    
    //CALLED WHENEVER A NEW SCENE IS LOADED (INCLUDING THE START OF THE GAME)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.Instance.StartFadeIn();
        if (_loadFromDoor)
        {
            StartCoroutine(ActivatePlayerControlsAfterFadeIn());
            
            FindDoor(_doorToSpawnTo);
            //warp player to correct door location
            _player.GetComponent<ItemManager>().MovePlayer(_playerSpawnPosition);
            //sync transforms
            _loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTriggerInteraction.DoorToSpawnAt doorSpawnNumber)
    {
        DoorTriggerInteraction[] doors = (DoorTriggerInteraction[]) FindObjectsOfType(typeof(DoorTriggerInteraction)) ;
            
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].currentDoorPosition == doorSpawnNumber)
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
        _playerSpawnPosition = _doorColl.transform.position + new Vector3(0f, 1f, 0f);
    }
}