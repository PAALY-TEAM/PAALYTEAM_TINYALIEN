using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement.Commands
{
    public class InteractCommand : Command
    {
        private PlayerMovementV03 _playerMovement;
        private InputAction _interactAction;

        public InteractCommand(PlayerMovementV03 playerMovement)
        {
            this._playerMovement = playerMovement;
            _interactAction = new InputAction(binding: "<Keyboard>/e");
            _interactAction.performed += ctx => Execute();
            _interactAction.Enable();
        }

        public override void Execute()
        {
            //_playerMovement.Interact();
        }
    }
}