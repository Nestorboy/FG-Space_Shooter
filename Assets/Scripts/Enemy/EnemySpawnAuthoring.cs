using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Enemy.DOD
{
    public class EnemySpawnAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject EnemyPrefab;

        private class EnemySpawnAuthoringBaker : Baker<EnemySpawnAuthoring>
        {
            public override void Bake(EnemySpawnAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new EnemySpawn()
                {
                    Prefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }

    public struct EnemySpawn : IComponentData
    {
        public Entity Prefab;
    }
}