using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement.Commands
{
    public class JumpCommand : Command
    {
        private PlayerMovementV03 _playerMovement;
        private InputAction _jumpAction;

        public JumpCommand(PlayerMovementV03 playerMovement)
        {
            this._playerMovement = playerMovement;
            _jumpAction = new InputAction(binding: "<Keyboard>/space");
            _jumpAction.performed += ctx => Execute();
            _jumpAction.Enable();
        }

        public override void Execute()
        {
            Vector3 gravity = CustomGravityV01.GetGravity(_playerMovement.body.position, out Vector3 upAxis);
            _playerMovement.Jump(gravity);
        }
    }
}