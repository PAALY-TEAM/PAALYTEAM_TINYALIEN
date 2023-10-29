using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MainMenuCameraDoTweenAnim : MonoBehaviour
    {
        [SerializeField]
        private float duration;

        public CinemachineVirtualCamera virtualCamera;

        public void LookAt(Transform target)
        {
            virtualCamera.transform.DOLookAt(target.position, duration);
        }
    }
}