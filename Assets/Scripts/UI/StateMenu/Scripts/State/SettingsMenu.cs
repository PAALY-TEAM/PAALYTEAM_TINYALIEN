using Camera;
using State.Menu;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace UI.StateMenu.Scripts.State
{
    public class SettingsMenu : _MenuState
    {
        private CameraInput _cameraInput;
        [SerializeField] private GameObject _mouseSenseSlider;

        private void Awake()
        {
            _cameraInput = GameObject.FindWithTag("Player").GetComponent<CameraInput>();
        }

        //Specific for this state
        public override void InitState(MenuController menuController)
        {
            base.InitState(menuController);

            state = MenuController.MenuState.Settings;
        }

        public void ChagneSense()
        {
            print("ChangedSense");
            var newSense=_mouseSenseSlider.GetComponent<Slider>().value;
            _cameraInput.lookSpeedX = newSense;
            _cameraInput.lookSpeedY = newSense;
        }
    }
}
