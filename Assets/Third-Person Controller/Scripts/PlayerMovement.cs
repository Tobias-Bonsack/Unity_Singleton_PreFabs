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
        [SerializeField] float _jumpPower;

        [Header("Move")]
        public bool _isMoving = false;
        public Vector3 _direction;
        [SerializeField] float _speed;
        [SerializeField] float _turnTime;
        private float _turnSmoothVelocity;


        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDir * _speed * Time.deltaTime);
            }
        }
    }
}
