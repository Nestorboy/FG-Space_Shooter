using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Enemy.DOD
{
    public class EnemyConfigAuthoring : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float Speed = 5f;
        [SerializeField] [Min(0f)] private float Acceleration = 10f;

        private class EnemyConfigAuthoringBaker : Baker<EnemyConfigAuthoring>
        {
            public override void Bake(EnemyConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemyConfig()
                {
                    Speed = authoring.Speed,
                    Acceleration = authoring.Acceleration,
                });
            }
        }
    }
    
    public struct EnemyConfig : IComponentData
    {
        public float Speed;
        public float Acceleration;
    }
}