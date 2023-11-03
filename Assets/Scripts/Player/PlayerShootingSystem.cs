using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Nessie.SpaceShooter.Player.DOD
{
    //[UpdateAfter(typeof(PlayerInputGatheringSystem))]
    public partial struct PlayerShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            JobHandle aimJob = new PlayerAimingJob().ScheduleParallel(state.Dependency);
            
            aimJob.Complete();
            
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            JobHandle shootJob = new PlayerShootingJob()
            {
                ECB = ecb,
                DeltaTime = SystemAPI.Time.DeltaTime,
            }.Schedule(state.Dependency);
            
            shootJob.Complete();
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
    
    [BurstCompile]
    public partial struct PlayerAimingJob : IJobEntity
    {
        public void Execute(in PlayerTag player, in ShooterInput input, ref LocalTransform transform)
        {
            if (!input.Shooting || math.lengthsq(input.Looking) <= 0f)
            {
                return;
            }
            
            float2 fwd = input.UsingMouse ? input.MouseWorldPos - transform.Position.xy : input.Looking;
            float radians = math.atan2(fwd.y, fwd.x) - math.radians(90f);
            transform.Rotation = quaternion.Euler(0f, 0f, radians);
        }
    }
    
    [BurstCompile]
    public partial struct PlayerShootingJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public float DeltaTime;
        
        public void Execute(in PlayerTag player, ref Shooting data, in ShooterInput input, ref LocalTransform transform)
        {
            data.Timer -= DeltaTime;
            if (data.Timer > 0f)
            {
                return;
            }

            if (input.Shooting)
            {
                data.Timer = data.Cooldown;
                Entity entity = ECB.Instantiate(data.Prefab);
                ECB.SetComponent(entity, new LocalTransform()
                {
                    Position = transform.Position,
                    Rotation = transform.Rotation,
                    Scale = 1f,
                });
                ECB.SetComponent(entity, new PhysicsVelocity()
                {
                    Linear = transform.Up() * data.Speed,
                });
            }
            else
            {
                data.Timer = 0f;
            }
        }
    }

    public struct Shooting : IComponentData
    {
        public Entity Prefab;
        public float Speed;

        public float Cooldown;
        public float Timer;
    }
}