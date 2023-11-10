using Pickup.Player;
using UnityEngine;

public class DoorTriggerInteraction : TriggerInteractionBase
{
    // Which door is this?
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
        Five,
        // Add more if needed
    }
    [Header("Door Condition")]
    //Should the door look for a specific material on the player?
    [SerializeField] private bool useSharedMaterialCondition = false;
    [Header("DoorToSpawnTo")]
    // Which door in new scene are we going to go into
    [SerializeField] private DoorToSpawnAt doorToSpawnTo; 
    [SerializeField] private SceneField sceneToLoad;
    
    [Space(10f)]
    [Header("THIS DOOR")]
    public DoorToSpawnAt currentDoorPosition;
    
    public override void Interact()
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().hintText.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
       
        // If player is not allowed to interact with this door, return
        if (!CanInteractWithDoor(player))
        {
            Debug.Log("Player is not allowed to interact with this door.");
            return;
        }
        // If scene to load is not set, return
        if (sceneToLoad == null || doorToSpawnTo == DoorToSpawnAt.None)
        {
            Debug.Log("Scene to load is not set. Interaction with door is disabled.");
            return;
        }
        // Load new scene
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnTo);
    }
    // Check if player can interact with door
    private bool CanInteractWithDoor(GameObject player)
    {
        if (!useSharedMaterialCondition)
        {
            return true;
        }

        Material playerMaterial = player.GetComponent<Renderer>().sharedMaterial;
        Material doorMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
    
        return playerMaterial == doorMaterial;
    }
}