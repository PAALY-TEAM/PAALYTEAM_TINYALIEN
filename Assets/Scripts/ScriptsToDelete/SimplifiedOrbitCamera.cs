using UnityEngine;

namespace ScriptsToDelete
{
    public class SimplifiedOrbitCamera : MonoBehaviour
    {
        public Vector2 CurrentOrbitAngles => _orbitAngles;

        [SerializeField, Range(1f, 360f)]
        float rotationSpeed = 90f;

        [SerializeField, Range(-89f, 89f)]
        float minVerticalAngle = -45f, maxVerticalAngle = 45f;

        Vector2 _orbitAngles = new Vector2(45f, 0f);

        void Update()
        {
            ManualRotation();
            ConstrainAngles();
        }

        bool ManualRotation()
        {
            Vector2 input = new Vector2(
                Input.GetAxis("Vertical Camera"),
                Input.GetAxis("Horizontal Camera")
            );
            const float e = 0.001f;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e)
            {
                _orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                return true;
            }
            return false;
        }

        void ConstrainAngles()
        {
            _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (_orbitAngles.y < 0f)
            {
                _orbitAngles.y += 360f;
            }
            else if (_orbitAngles.y >= 360f)
            {
                _orbitAngles.y -= 360f;
            }
        }
    }
}