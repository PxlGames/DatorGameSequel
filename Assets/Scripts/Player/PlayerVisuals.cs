using System.Collections;
using UnityEngine;

namespace PxlDev
{   
    public class PlayerVisuals : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerInteractions _playerInteractions;

        public Animator SwordAnimator;
        
        [HideInInspector] public AnimationController _animController;

        private void Start()
        {
            var player = GetComponent<Player>();
            _playerMovement = player.Movement;
            _playerInteractions = player.Interactions;

            _animController = GetComponent<AnimationController>();
        }

        private void Update()
        {
            var x = _playerMovement.x;
            var y = _playerMovement.y;
            
            if(x != 0 || y != 0)
            {
                _animController.SetDirection(new Vector2(x, y));
                _animController.Still = false;
            }
            else
            {
                _animController.Still = true;
            }
        }

        public void Damaged()
        {
            _animController.Damaged();
        }
    }
}