using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

namespace Nessie.SpaceShooter.Player.DOD
{
    //[UpdateAfter(typeof(PlayerInputGatheringSystem))]
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            JobHandle job = new PlayerMovementJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel(state.Dependency);
            
            job.Complete();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
    
    [BurstCompile]
    public partial struct PlayerMovementJob : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(in PlayerTag player, ref PhysicsVelocity physics, in ShooterData data, in ShooterInput input)
        {
            float2 moveVector = input.Movement;
            float acceleration = math.lengthsq(moveVector) > 0f ? data.Acceleration : data.Deceleration;
            float2 velocityDir = math.normalize(physics.Linear.xy);
            float2 moveDir = math.normalize(moveVector);
            float recovery = 1f + math.max(0f, -math.dot(moveDir, velocityDir));
            physics.Linear.xy = MathUtilities.MoveTowards(physics.Linear.xy, moveVector * data.MoveSpeed, recovery * acceleration * DeltaTime);
        }
    }
}