using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Gameplay.DOD
{
    public class DamageAuthoring : MonoBehaviour
    {
        [SerializeField] private float Strength;
        
        private class DamageAuthoringBaker : Baker<DamageAuthoring>
        {
            public override void Bake(DamageAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Damage()
                {
                    Strength = authoring.Strength,
                });
            }
        }
    }
    
    public struct Damage : IComponentData
    {
        public float Strength;
    }
}