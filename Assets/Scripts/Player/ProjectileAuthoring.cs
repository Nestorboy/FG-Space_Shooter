using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Player.DOD
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new ProjectileTag());
            }
        }
    }

    public struct ProjectileTag : IComponentData
    {

    }
}