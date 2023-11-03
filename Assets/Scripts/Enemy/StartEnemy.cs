using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class StartEnemy : MonoBehaviour
    {
        [SerializeField] private float Damage = 1f;
        [SerializeField] private float AttackCooldown = 0.25f;

        private Health _health;
        
        private bool _isAttackReady = true;

        private void Awake()
        {
            _health = GetComponent<Health>();
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

        private void OnAttackCooldown()
        {
            _isAttackReady = true;
        }

        private void OnDeath()
        {
            GameManager.Instance.Begin();
        }
    }
}
