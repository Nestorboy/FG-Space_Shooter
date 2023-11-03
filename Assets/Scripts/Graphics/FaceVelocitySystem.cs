using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Nessie.SpaceShooter.Graphics.DOD
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true)]
    public partial struct FaceVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new FaceVelocityJob().ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

    [BurstCompile]
    public partial struct FaceVelocityJob : IJobEntity
    {
        public void Execute(in FaceVelocity face, in PhysicsVelocity physics, ref LocalTransform transform)
        {
            float2 velocity = physics.Linear.xy;
            if (math.lengthsq(velocity) <= 0f)
                return;
            float radians = math.atan2(velocity.y, velocity.x) - math.radians(90f);
            transform.Rotation = quaternion.Euler(0f, 0f, radians);
        }
    }

    public struct FaceVelocity : IComponentData, IEnableableComponent
    {
        
    }
}