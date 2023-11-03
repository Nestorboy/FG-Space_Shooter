using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Enemy.DOD
{
    public class EnemyAuthoring : MonoBehaviour
    {
        private class EnemyAuthoringBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new EnemyTag());
            }
        }
    }
}