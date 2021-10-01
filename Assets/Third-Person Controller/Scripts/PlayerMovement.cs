using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerMovement : MonoBehaviour
    {

        private CharacterController _controller;
        [SerializeField] Transform _mainCamera;

        [Header("Jump")]
        [SerializeField] float _jumpHeight;

        [Header("Move")]
        public bool _isMoving = false;
        public Vector3 _direction;
        public float _MoveResistance;
        [SerializeField] float _speed;
        [SerializeField] float _turnTime;
        private float _turnSmoothVelocity;
        private Vector3 _moveDir;

        [Header("Gravity")]
        private float _gravity = -9.81f;
        private Vector3 _velocity = new Vector3(0f, 0f, 0f);
        private float _basicDown = -1f;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            CalculateResistance();
            CalculateGravity();

            _controller.Move(_velocity * Time.deltaTime);

        }

        private void CalculateResistance()
        {
            float brakeXPower = Mathf.Abs(_velocity.x) / _speed;
            float brakeZPower = Mathf.Abs(_velocity.z) / _speed;

            Vector3 brakeForce = new Vector3(0f, 0f, 0f);
            brakeForce.x = _MoveResistance * brakeXPower * (_velocity.x > 0f ? -1 : 1);
            brakeForce.z = _MoveResistance * brakeZPower * (_velocity.z > 0f ? -1 : 1);

            Debug.Log(brakeForce);
            AddForce(brakeForce, false);
        }

        private void CalculateMovement()
        {
            if (_isMoving && _controller.isGrounded)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                _moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                AddForce(_moveDir * _speed * Time.deltaTime, false);

                _velocity.x = Mathf.Clamp(_velocity.x, -_speed, _speed);
                _velocity.z = Mathf.Clamp(_velocity.z, -_speed, _speed);
            }
        }
        private void CalculateGravity()
        {
            if (!_controller.isGrounded)
            {
                _velocity.y += _gravity * Time.deltaTime;
            }
            else
            {
                if (_velocity.y < _basicDown)
                {
                    _velocity.y = _basicDown;
                }
            }
        }


        public void Jump()
        {
            if (_controller.isGrounded)
            {
                float forceUp = Mathf.Sqrt(_jumpHeight * -2f * _gravity) - _basicDown;
                AddForce(new Vector3(0f, _jumpHeight, 0f), false);
            }
        }

        public void AddForce(Vector3 force, bool reset) => _velocity = reset ? force : force + _velocity;

        private bool SameSign(float num1, float num2) => Mathf.Sign(num1) == Mathf.Sign(num2);
    }
}
