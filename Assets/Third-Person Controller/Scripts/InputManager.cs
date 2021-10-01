using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] PlayerMovement _player;

        public void OnMove(InputAction.CallbackContext context)
        {
            _player._isMoving = !context.canceled;
            if (_player._isMoving)
            {
                //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
                Vector2 moveDirection = context.ReadValue<Vector2>().normalized;
                _player._direction = new Vector3(moveDirection.x, 0f, moveDirection.y);
            }

        }
        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log(context);
            if (context.started)
            {
                //TODO implement jump logik
            }
        }
    }
}
