using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class MainMenuCameraDOTweenAnim : MonoBehaviour
{
    [SerializeField]
    private float duration;

    public CinemachineVirtualCamera virtualCamera;

    public void LookAt(Transform target)
    {
        virtualCamera.transform.DOLookAt(target.position, duration);
    }
}