using UnityEngine;

namespace Movement
{
    public class RotateHeadToMovement2 : MonoBehaviour
    {
        [SerializeField] private Transform orbitCamera;
        [SerializeField] private float turnSmoothTime = 0.1f;
        
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
            if (!(direction.magnitude >= 0.1f)) return;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + orbitCamera.eulerAngles.y;

            // New rotation represented as Quaternion
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Smooth transition from current rotation to target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothTime * Time.deltaTime);
        }
    }
}