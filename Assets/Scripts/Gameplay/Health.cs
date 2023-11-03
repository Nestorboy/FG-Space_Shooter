using UnityEngine;
using UnityEngine.Events;

namespace Nessie.SpaceShooter.OOP
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float MaxHealth;
        [SerializeField] private ParticleSystem DeathVFX;

        public UnityAction OnDeath;
        
        private float _health;

        private void Awake()
        {
            _health = MaxHealth;
        }

        public void Damage(float amount)
        {
            _health -= amount;
            if (_health <= 0f)
            {
                Death();
            }
        }

        private void Death()
        {
            if (DeathVFX)
            {
                Instantiate(DeathVFX, transform.position, transform.rotation);
            }

            OnDeath?.Invoke();

            Destroy(gameObject);
        }
    }
}
