using System;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float Damage = 1f;
        [SerializeField] private float Speed = 20f;
        [SerializeField] private float Acceleration = 20f;
        [SerializeField] private float AttackCooldown = 0.25f;

        private Rigidbody _rb;
        
        private bool _isAttackReady = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_isAttackReady) return;
            _isAttackReady = false;
            
            if (!other.TryGetComponent(out Health health))
            {
                return;
            }
            
            health.Damage(Damage);
            
            Invoke(nameof(OnAttackCooldown), AttackCooldown);
        }

        private void Update()
        {
            if (GameManager.Instance.Player)
            {
                Vector3 toPlayer = (GameManager.Instance.Player.transform.position - transform.position).normalized;
                float recover = 1f + 0.5f * Mathf.Max(0f, -Vector2.Dot(_rb.velocity.normalized, toPlayer));
                _rb.velocity = Vector2.MoveTowards(_rb.velocity, toPlayer * Speed, recover * Acceleration * Time.deltaTime);
            }

            transform.up = _rb.velocity;
        }

        private void OnAttackCooldown()
        {
            _isAttackReady = true;
        }
    }
}
