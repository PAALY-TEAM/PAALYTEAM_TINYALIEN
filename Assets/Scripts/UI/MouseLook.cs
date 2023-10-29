using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MouseLook : MonoBehaviour
    {
        public Slider slider;
        public float mouseSensitivity = 100f;
        public Transform playerBody;
        float _xRotation = 0f;

        void Start()
        {
            mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100);
            slider.value = mouseSensitivity/10;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        public void AdjustSpeed(float newSpeed)
        {
            mouseSensitivity = newSpeed * 10;
        }
    }
}