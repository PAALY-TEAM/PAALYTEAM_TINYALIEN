using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [FormerlySerializedAs("CinemachineCameraTarget")]
        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject cinemachineCameraTarget;

        [FormerlySerializedAs("TopClamp")] [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 70.0f;

        [FormerlySerializedAs("BottomClamp")] [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -30.0f;

        [FormerlySerializedAs("CameraAngleOverride")] [Tooltip("Additional degrees to override the camera. Useful for fine-tuning camera position when locked")]
        public float cameraAngleOverride = 0.0f;

        [FormerlySerializedAs("LockCameraPosition")] [Tooltip("For locking the camera position on all axis")]
        public bool lockCameraPosition = false;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private CameraInput _input;
        private GameObject _mainCamera;
        private const float Threshold = 0.01f;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _input = GetComponent<CameraInput>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= Threshold && !lockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * _input.lookSpeedX;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * _input.lookSpeedY;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
            
        }
    }
}         
