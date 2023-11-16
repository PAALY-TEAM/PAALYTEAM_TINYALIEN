using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement.Commands
{
    public class ClimbCommand : Command
    {
        private PlayerMovementV03 _playerMovement;
        private InputAction _climbAction;

        public ClimbCommand(PlayerMovementV03 playerMovement)
        {
            this._playerMovement = playerMovement;
            _climbAction = new InputAction(binding: "<Keyboard>/c");
            _climbAction.performed += ctx => Execute();
            _climbAction.Enable();
        }

        public override void Execute()
        {
            _playerMovement.Climb();
        }
    }
}