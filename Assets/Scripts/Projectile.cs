using System;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ParticleSystem HitVFX;
        
        [NonSerialized] public float Damage = 0f;
        
        private Camera _mainCamera;
        private Plane[] _clipPlanes = new Plane[6];
        private Renderer[] _renderers;

        private void Awake()
        {
            _mainCamera = Camera.main;
            GeometryUtility.CalculateFrustumPlanes(_mainCamera, _clipPlanes);
            _renderers = GetComponentsInChildren<Renderer>();
        }

        private void Update()
        {
            foreach (Renderer renderer in _renderers)
            {
                if (GeometryUtility.TestPlanesAABB(_clipPlanes, renderer.bounds))
                {
                    return;
                }
            }
            
            Destroy(gameObject);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent(out Health health))
            {
                return;
            }
            
            ParticleSystem hitParticles = Instantiate(HitVFX, transform.position, transform.rotation);
            //hitParticles.transform.up = transform.position - other.transform.position;
            
            health.Damage(Damage);
            Destroy(gameObject);
        }
    }
}
