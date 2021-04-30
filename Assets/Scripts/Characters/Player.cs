using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        private void OnJump()
        {
            character.Jump();
        }

        private void OnMove(InputValue value)
        {
            var input = value.Get<float>();

            if (input > 0)
                character.MoveRight();

            else if (input < 0)
                character.MoveLeft();

            else
                character.StopMoving();
        }
    }
}
