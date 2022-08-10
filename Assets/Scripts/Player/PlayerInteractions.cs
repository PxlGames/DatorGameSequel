using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace PxlDev
{
    public class PlayerInteractions : MonoBehaviour
    {
        [Header("Attacking")]
        [SerializeField] private bool _disableAttack; 
        [SerializeField] private int _damage = 3;
        [SerializeField] private float _attackRadius = 0.5f;
        [SerializeField] private float _attackCooldown = 0.25f;
        [SerializeField] private Transform _attackPoint;

        [Header("Interaction")]
        [SerializeField] private float _maxInteractionDistance;
        [SerializeField] private LayerMask _dettectableLayer;
        private float _currentAttackTime;
        
        // Scripts
        private PlayerMovement _playerMovement;
        private PlayerVisuals _playerVisuals;
        
        [Header("States")]
        public bool IsAttacking;
        public bool CanInteract;
        public bool IsInteracting;

        private bool _inRange;
        private Transform _playerRotation;

        private void Start()
        {
            var player = GetComponent<Player>();
            _playerMovement = player.Movement;
            _playerVisuals = player.Visuals;
            CanInteract = true;

            _playerRotation = _playerMovement._playerRotation;
        }

        private void Update()
        {
            if(Input.GetButtonDown("Jump") && !IsInteracting)
                Interaction();
            
            if(Input.GetButtonDown("Jump") && !IsInteracting)
            {
                PlayAttackAnimation();
            }
        }
        
        private Interactable _interactable;
        private Vector3 _direction;
        private void Interaction()
        {
            var visuals = Player.Instance.Visuals._animController;
            _direction = new Vector3(visuals.Direction.x, 0, visuals.Direction.y);

            if (!Physics.Raycast(transform.position, _playerRotation.forward, out var hit, _maxInteractionDistance, _dettectableLayer))
            {
                _inRange = false;
                return;
            }
            else
            {
                _inRange = true;
                hit.transform.gameObject.TryGetComponent<Interactable>(out _interactable);
                if(_interactable == null)
                    return;
            }

            if (!_inRange || IsInteracting) return;
            
            _interactable.Interact();
        }

        public void Interacting(bool isInteracting)
        {
            IsInteracting = isInteracting;
        }

        private void PlayAttackAnimation()
        {
            var visuals = Player.Instance.Visuals._animController;

            _direction = new Vector3(visuals.Direction.x, 0, visuals.Direction.y);

            if (Physics.Raycast(transform.position, _playerRotation.forward, out var hit, _maxInteractionDistance, _dettectableLayer))
            {
                return;
            }

            visuals._anim.SetTrigger("Attack");
            _playerVisuals.SwordAnimator.SetTrigger("Attack");

            IsAttacking = true;
            Attack();
        }

        public void Attack()
        {
            IsAttacking = false;
            var hitColliders = Physics.OverlapSphere(_attackPoint.position, _attackRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform == transform) continue;
                if (hitCollider.TryGetComponent<Damageable>(out var damageable))
                {
                    damageable.Damage(_damage, transform);
                }
            }

            StartCoroutine(MovementCooldown());
        }

        IEnumerator MovementCooldown()
        {
            _playerMovement.IsAttacking = true;
            yield return new WaitForSeconds(_attackCooldown);
            _playerMovement.IsAttacking = false;
        }

        private void OnDrawGizmos()
        {
            if (_attackPoint == null) 
                return;

            Gizmos.color = Color.red;
            if(_attackPoint != null)
                Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }
    }
}