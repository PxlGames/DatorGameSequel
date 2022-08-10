using System;
using UnityEngine;

namespace PxlDev
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _gravity = 9f;
        
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] public Transform _playerRotation;

        public bool IsMoving { get; private set; }
        public bool IsMovementAllowed { get; set; }
        public bool IsAttacking;

        private CharacterController _controller;
        private PlayerInteractions _playerInteractions;
        public Vector2Int MovementDirection;
        


        private void Awake()
        {
            IsMovementAllowed = true;
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerInteractions = GetComponent<PlayerInteractions>();
        }

        [HideInInspector] public float x;
        [HideInInspector] public float y;

        private void Update()
        {
            if (!IsMovementAllowed || IsAttacking) return;
            
            var t = transform;
            
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            IsMoving = x != 0 || y != 0;

            if (IsMoving)
            {
                MovementDirection = new Vector2Int((int)x, (int)y);
                _playerRotation.forward = new Vector3(x, 0, y);
            }

            var move = _playerInteractions.IsAttacking || _playerInteractions.IsInteracting ? Vector3.zero : new Vector3(x, 0, y);
			move *= _speed;

			if(!_controller.isGrounded)
            {
                move.y -= _gravity;
			}

            _controller.Move(move * Time.deltaTime);
        }
    }
}