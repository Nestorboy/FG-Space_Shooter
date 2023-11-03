using Nessie.SpaceShooter.DOD;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Nessie.SpaceShooter.Gameplay.DOD
{
    public partial struct CollisionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new TriggerJob
            {
                HealthGroup = SystemAPI.GetComponentLookup<Health>(),
                DamageGroup = SystemAPI.GetComponentLookup<Damage>(),
                ECB = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
    
    [BurstCompile]
    struct TriggerJob : ITriggerEventsJob
    {
        public ComponentLookup<Health> HealthGroup;
        public ComponentLookup<Damage> DamageGroup;
        public EntityCommandBuffer ECB;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            
            bool aIsHealth = HealthGroup.HasComponent(entityA);
            bool bIsDamage = DamageGroup.HasComponent(entityB);
            if (aIsHealth && bIsDamage)
            {
                OnCollision(entityB, entityA);
            }
            
            bool bIsHealth = HealthGroup.HasComponent(entityB);
            bool aIsDamage = DamageGroup.HasComponent(entityA);
            if (bIsHealth && aIsDamage)
            {
                OnCollision(entityA, entityB);
            }
        }

        private void OnCollision(Entity damageEntity, Entity healthEntity)
        {
            RefRO<Damage> damage = DamageGroup.GetRefRO(damageEntity);
            RefRW<Health> health = HealthGroup.GetRefRW(healthEntity);
            float remaining = health.ValueRW.CurrentHealth - damage.ValueRO.Strength;
            health.ValueRW.CurrentHealth = remaining;
            if (remaining <= 0f)
            {
                ECB.DestroyEntity(healthEntity);
            }
        }
    }
}