using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] GameObject _player;
        private CharacterController _controller;
        [SerializeField] Transform _camera;

        [Header("Move")]
        [SerializeField] float _speed;
        [SerializeField] float _turnSpeed;
        private float _turnSmoothVelocity;
        private bool _isMoving = false;
        private Vector3 _direction;

        private void Awake()
        {
            _controller = _player.GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _isMoving = !context.canceled;
            if (_isMoving)
            {
                //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
                Vector2 moveDirection = context.ReadValue<Vector2>().normalized;
                _direction = new Vector3(moveDirection.x, 0f, moveDirection.y);
            }

        }

        private void FixedUpdate()
        {

            if (_isMoving)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSpeed);
                _player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDir * _speed * Time.deltaTime);
            }
        }
    }
}

