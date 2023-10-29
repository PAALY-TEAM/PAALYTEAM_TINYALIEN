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
        // Load new scene
        GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().hintText.SetActive(true);
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnTo);
    }
}

/*
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

    [Header("Spawn To")]
    // Which door in new scene are we going to go into?
    [SerializeField] private DoorToSpawnAt doorToSpawnTo; 
    [SerializeField] private SceneField sceneToLoad;

    [Space(10f)]
    [Header("THIS DOOR")]
    public DoorToSpawnAt currentDoorPosition;

    public override void Interact()
    {
        // Load new scene
        SceneSwapManager.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnTo);
    }
}
*/