using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nessie.SpaceShooter.Player.DOD
{
    public class PlayerShooterAuthoring : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed = 10f;
        [SerializeField] private float Acceleration = 40f;
        [SerializeField] private float Deceleration = 30f;
        [SerializeField] private float Cooldown = 0.25f;
        [SerializeField] private float ProjectileSpeed = 20f;
        [SerializeField] private GameObject ProjectilePrefab;
        
        private class PlayerShooterBaker : Baker<PlayerShooterAuthoring>
        {
            public override void Bake(PlayerShooterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerTag());
                AddComponent(entity, new ShooterData
                {
                    MoveSpeed = authoring.MoveSpeed,
                    Acceleration = authoring.Acceleration,
                    Deceleration = authoring.Deceleration,
                });
                AddComponent(entity, new ShooterInput());
                AddComponent(entity, new Shooting()
                {
                    Prefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                    Speed = authoring.ProjectileSpeed,
                    Cooldown = authoring.Cooldown,
                });
            }
        }
    }
    
    public struct ShooterData : IComponentData
    {
        public float MoveSpeed;
        public float Acceleration;
        public float Deceleration;
    }
    
    public struct ShooterInput : IComponentData
    {
        public bool UsingMouse;
        public float2 MouseWorldPos;
        
        public float2 Movement;
        public float2 Looking;
        public bool Shooting;
        public bool Dodging;
        
        public InputActionPhase MovementPhase;
        public InputActionPhase LookingPhase;
        public InputActionPhase ShootingPhase;
        public InputActionPhase DodgingPhase;
    }
}