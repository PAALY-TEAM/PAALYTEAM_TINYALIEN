using UnityEngine;

namespace Camera
{
    public class SimplifiedOrbitCamera : MonoBehaviour
    {
        public Vector2 CurrentOrbitAngles => orbitAngles;

        [SerializeField, Range(1f, 360f)]
        float rotationSpeed = 90f;

        [SerializeField, Range(-89f, 89f)]
        float minVerticalAngle = -45f, maxVerticalAngle = 45f;

        Vector2 orbitAngles = new Vector2(45f, 0f);

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
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                return true;
            }
            return false;
        }

        void ConstrainAngles()
        {
            orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (orbitAngles.y < 0f)
            {
                orbitAngles.y += 360f;
            }
            else if (orbitAngles.y >= 360f)
            {
                orbitAngles.y -= 360f;
            }
        }
    }
}