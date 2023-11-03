using Nessie.SpaceShooter.Player.DOD;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Nessie.SpaceShooter.Enemy.DOD
{
    public partial struct EnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EnemyConfig config = SystemAPI.GetSingleton<EnemyConfig>();
            float2 playerPos = float2.zero;
            foreach (LocalToWorld toWorld in SystemAPI.Query<LocalToWorld>().WithAll<PlayerTag>())
            {
                // Any better way to get the first/only players LocalToWorld?
                playerPos = toWorld.Position.xy;
                break;
            }
            
            JobHandle job = new EnemyMovementJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                Speed = config.Speed,
                Acceleration = config.Acceleration,
                PlayerPosition = playerPos,
            }.ScheduleParallel(state.Dependency);
            
            job.Complete();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
    }
    
    [BurstCompile]
    public partial struct EnemyMovementJob : IJobEntity
    {
        public float DeltaTime;
        
        public float Speed;
        public float Acceleration;
        public float2 PlayerPosition;
        
        public void Execute(in EnemyTag enemy, ref PhysicsVelocity physics, in LocalTransform transform)
        {
            float2 velocityDir = math.normalize(physics.Linear.xy);
            float2 toPlayer = math.normalize(PlayerPosition - transform.Position.xy);
            float recovery = 1f + 0.5f * math.max(0f, -math.dot(toPlayer, velocityDir));
            physics.Linear.xy = MathUtilities.MoveTowards(physics.Linear.xy, toPlayer * Speed, recovery * Acceleration * DeltaTime);
        }
    }
}