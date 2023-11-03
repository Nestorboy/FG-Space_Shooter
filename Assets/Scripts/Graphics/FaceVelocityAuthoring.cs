using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Graphics.DOD
{
    public class FaceVelocityAuthoring : MonoBehaviour
    {
        private class FaceVelocityBaker : Baker<FaceVelocityAuthoring>
        {
            public override void Bake(FaceVelocityAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new FaceVelocity());
            }
        }
    }
}