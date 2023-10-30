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
    [Header("DoorToSpawnTo")]
    // Which door in new scene are we going to go into
    [SerializeField] private DoorToSpawnAt doorToSpawnTo; 
    [SerializeField] private SceneField sceneToLoad;
    
    [Space(10f)]
    [Header("THIS DOOR")]
    public DoorToSpawnAt currentDoorPosition;

    public override void Interact()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().hintText.SetActive(true);
        if (sceneToLoad == null || doorToSpawnTo == DoorToSpawnAt.None)
        {
            Debug.Log("Scene to load is not set. Interaction with door is disabled.");
            return;
        }
        // Load new scene
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnTo);
    }
}