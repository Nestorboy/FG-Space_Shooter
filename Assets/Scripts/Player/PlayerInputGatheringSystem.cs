using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nessie.SpaceShooter.Player.DOD
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputGatheringSystem : SystemBase, ShooterActions.IPlayerActions
    {
        private ShooterActions _shooterActions;
        private Camera _camera;

        private bool _usingMouse;
        private Vector2 _mouseWorldPos;
        
        private Vector2 _movement;
        private Vector2 _looking;
        private bool _shooting;
        private bool _dodging;
        
        private InputActionPhase _movementPhase;
        private InputActionPhase _lookingPhase;
        private InputActionPhase _shootingPhase;
        private InputActionPhase _dodgingPhase;
        
        protected override void OnCreate()
        {
            _shooterActions = new ShooterActions();
            _shooterActions.Player.SetCallbacks(this);
            _camera = Camera.main;
        }

        protected override void OnStartRunning()
        {
            _shooterActions.Enable();
        }

        protected override void OnStopRunning()
        {
            _shooterActions.Disable();
        }

        protected override void OnUpdate()
        {
            ShooterInput shooterInput = new ShooterInput()
            {
                UsingMouse = _usingMouse,
                MouseWorldPos = _mouseWorldPos,
                
                Movement = _movement,
                Looking = _looking,
                Shooting = _shooting,
                Dodging = _dodging,
                
                MovementPhase = _movementPhase,
                LookingPhase = _lookingPhase,
                ShootingPhase = _shootingPhase,
                DodgingPhase = _dodgingPhase,
            };

            Entities.ForEach((ref ShooterInput input) =>
            {
                input.UsingMouse = shooterInput.UsingMouse;
                input.MouseWorldPos = shooterInput.MouseWorldPos;
                
                input.Movement = shooterInput.Movement;
                input.Looking = shooterInput.Looking;
                input.Shooting = shooterInput.Shooting;
                input.Dodging = shooterInput.Dodging;
                
                input.MovementPhase = shooterInput.MovementPhase;
                input.LookingPhase = shooterInput.LookingPhase;
                input.ShootingPhase = shooterInput.ShootingPhase;
                input.DodgingPhase = shooterInput.DodgingPhase;
            }).ScheduleParallel();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
            _movementPhase = context.phase;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _usingMouse = context.control.device == Mouse.current;
            
            _looking = context.ReadValue<Vector2>();
            _mouseWorldPos = _usingMouse ? _camera.ScreenToWorldPoint(_looking) : Vector2.zero;
            _lookingPhase = context.phase;
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _shooting = context.ReadValueAsButton();
            _shootingPhase = context.phase;
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            _dodging = context.ReadValueAsButton();
            _dodgingPhase = context.phase;
        }
    }
}