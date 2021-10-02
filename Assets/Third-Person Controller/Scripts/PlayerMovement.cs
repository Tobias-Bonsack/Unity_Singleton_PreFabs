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
        [SerializeField] float _jumpResistanceMultiplikator;

        [Header("Move")]
        public bool _isMoving = false;
        public Vector3 _direction = Vector3.zero;
        [SerializeField] float _moveResistance;
        [SerializeField] float _basicSpeedResistance;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _acceleration;
        [SerializeField] float _turnTime;
        private float _turnSmoothVelocity;
        private Vector3 _moveDir;

        [Header("Gravity")]
        private float _gravity = -9.81f;
        private Vector3 _velocity = Vector3.zero;
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

            //TODO here place for extern forces

            _controller.Move(_velocity * Time.deltaTime);

        }
        private void CalculateMovement()
        {
            if (_isMoving && _controller.isGrounded)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                _moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                AddForce(_moveDir * _acceleration * Time.deltaTime, false);

                _velocity.x = Mathf.Clamp(_velocity.x, -_maxSpeed, _maxSpeed);
                _velocity.z = Mathf.Clamp(_velocity.z, -_maxSpeed, _maxSpeed);
            }
        }
        private void CalculateResistance()
        {
            float absVelocityX = Mathf.Abs(_velocity.x);
            float absVelocityZ = Mathf.Abs(_velocity.z);

            float brakeXPercent = absVelocityX / _maxSpeed;
            float brakeZPercent = absVelocityZ / _maxSpeed;

            Vector3 brakeForce = Vector3.zero;

            // resistance * overall percent * opposite direction
            brakeForce.x = _moveResistance * brakeXPercent * OppositeSign(_velocity.x) * (_isMoving ? _jumpResistanceMultiplikator : 1f);
            brakeForce.z = _moveResistance * brakeZPercent * OppositeSign(_velocity.z) * (_isMoving ? _jumpResistanceMultiplikator : 1f);

            if (!_isMoving)
            {
                if (absVelocityX < _basicSpeedResistance && absVelocityX != 0f)
                {
                    brakeForce.x = -_velocity.x;
                }
                if (absVelocityZ < _basicSpeedResistance && absVelocityZ != 0f)
                {
                    brakeForce.z = -_velocity.z;
                }
            }

            AddForce(brakeForce, false);
        }
        private void CalculateGravity()
        {
            if (!_controller.isGrounded) { _velocity.y += _gravity * Time.deltaTime; }
            else if (_velocity.y < _basicDown) { _velocity.y = _basicDown; }
        }


        public void Jump()
        {
            if (_controller.isGrounded)
            {
                float forceUp = Mathf.Sqrt(_jumpHeight * -2f * _gravity) - _basicDown;
                AddForce(new Vector3(0f, _jumpHeight, 0f), false);
            }
        }

        //TODO check if public is needed at any time
        public void AddForce(Vector3 force, bool reset) => _velocity = reset ? force : force + _velocity;

        private int OppositeSign(float number) => number > 0f ? -1 : 1;
        //TODO maybe delete later, dont know if needed at any time
        private bool SameSign(float num1, float num2) => Mathf.Sign(num1) == Mathf.Sign(num2);
    }
}