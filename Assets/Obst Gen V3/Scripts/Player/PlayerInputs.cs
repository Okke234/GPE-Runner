using UnityEngine;
using UnityEngine.InputSystem;

namespace Obst_Gen_V3.Scripts.Player
{
    public class PlayerInputs : MonoBehaviour
    {
        public Vector2 move;
        public bool moving;
        public bool jump;

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
            moving = newMoveDirection != Vector2.zero;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }
    }
}