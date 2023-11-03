using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.DOD
{
    public class HealthAuthoring : MonoBehaviour
    {
        [SerializeField] private float MaxHealth;
        
        private class HealthAuthoringBaker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Health()
                {
                    MaxHealth = authoring.MaxHealth,
                    CurrentHealth = authoring.MaxHealth,
                });
            }
        }
    }
    
    public struct Health : IComponentData
    {
        public float MaxHealth;
        public float CurrentHealth;
    }
}