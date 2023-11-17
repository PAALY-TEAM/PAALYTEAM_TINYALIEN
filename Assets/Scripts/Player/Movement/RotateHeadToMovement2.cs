using UnityEngine;

namespace Movement
{
    public class RotateHeadToMovement2 : MonoBehaviour
    {
        [SerializeField] private Transform orbitCamera;
        [SerializeField] private float turnSmoothTime = 6f;
        [SerializeField] private float tiltBackSmoothTime = 1.5f; // New field for slower tilt back
        [SerializeField] private float tiltBackAfterSeconds = 1f;
        
        private float _noInputTimeCounter = 0f;
        
        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.rotation;
        }
        
        public void ResetRotation()
        {
            transform.rotation = _initialRotation;
            _noInputTimeCounter = 0f; // Reset the no input time counter
        }
        
        private void Update()
        {
            if (RespawnTrigger.IsRespawning)
            {
                return;
            }
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                _noInputTimeCounter = 0f; // Reset the timer when there is input
                Vector3 eulerAngles = orbitCamera.eulerAngles;
                float targetAngleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + eulerAngles.y;
                float targetAngleX = eulerAngles.x;
                var targetRotation = Quaternion.Euler(targetAngleX, targetAngleY, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothTime * Time.deltaTime);
            }
            else
            {
                _noInputTimeCounter += Time.deltaTime; // Start counting when there is no input
                if (!(_noInputTimeCounter >= tiltBackAfterSeconds))      // After 2 seconds of no input
                    return;
                float targetAngleY = transform.eulerAngles.y;
                var targetRotation = Quaternion.Euler(0f, targetAngleY, 0f); // Reset the x-axis rotation to 0
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltBackSmoothTime * Time.deltaTime); // Use tiltBackSmoothTime here
            }
        }
    }
}