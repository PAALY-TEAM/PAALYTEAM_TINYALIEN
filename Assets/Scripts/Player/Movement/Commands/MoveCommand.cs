using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement.Commands
{
    public class MoveCommand : Command
    {
        private PlayerMovementV03 _playerMovement;
        private Vector2 _direction;
        private InputAction _moveAction;

        public MoveCommand(PlayerMovementV03 playerMovement, Vector2 direction)
        {
            this._playerMovement = playerMovement;
            this._direction = direction;
            _moveAction = new InputAction(binding: "<Keyboard>/w");
            _moveAction.performed += callbackContext => Execute();
            _moveAction.Enable();
        }
        public void UpdateDirection(Vector2 newDirection)
        {
            _direction = newDirection;
        }
        public override void Execute()
        {
            _playerMovement.SetInputVector(_direction);
            _playerMovement.CalculateDesiredVelocity();
        }
    }
}
