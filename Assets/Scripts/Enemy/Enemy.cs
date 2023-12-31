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
        private Health _health;
        private PlayerShooter _player;
        
        private bool _isAttackReady = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _health = GetComponent<Health>();
            _player = GameManager.Instance.Player;
        }

        private void OnEnable()
        {
            _health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnDeath;
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
            if (_player)
            {
                Vector3 toPlayer = (_player.transform.position - transform.position).normalized;
                float recover = 1f + 0.5f * Mathf.Max(0f, -Vector2.Dot(_rb.velocity.normalized, toPlayer));
                _rb.velocity = Vector2.MoveTowards(_rb.velocity, toPlayer * Speed, recover * Acceleration * Time.deltaTime);
            }

            transform.up = _rb.velocity;
        }

        private void OnAttackCooldown()
        {
            _isAttackReady = true;
        }

        private void OnDeath()
        {
            GameManager.Instance.OnKilledEnemy();
        }
    }
}
