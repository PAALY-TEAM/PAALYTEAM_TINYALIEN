using Camera;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SensitivitySettings : MonoBehaviour
    {
        public CameraInput cameraInput;

        public TextMeshProUGUI sensitivityText;

        public GameObject settingPanel;
        public bool isSettingActive;

        public void SettingsButton()
        {
            if(isSettingActive == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        public void Pause ()
        {
            settingPanel.SetActive(true);
            isSettingActive = true;
        }

        public void Resume ()
        {
            settingPanel.SetActive(false);
            isSettingActive = false;
        }

        public void UpdateCameraSensitivity(float newSensitivity)
        {
            cameraInput.lookSpeedX = newSensitivity;
            cameraInput.lookSpeedY = newSensitivity;      
            sensitivityText.text = "Sensitivity: " + newSensitivity.ToString("F2");
        }
    }
}