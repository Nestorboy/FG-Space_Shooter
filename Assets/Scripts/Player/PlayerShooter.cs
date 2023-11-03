using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nessie.SpaceShooter.OOP
{
    public class PlayerShooter : MonoBehaviour, ShooterActions.IPlayerActions
    {
        [Header("Movement")]
        [SerializeField] [Min(0f)] private float MoveSpeed = 10f;
        [SerializeField] [Min(0f)] private float Acceleration = 40f;
        [SerializeField] [Min(0f)] private float Deceleration = 30f;

        [Header("Shooting")]
        [SerializeField] private Projectile ProjectilePrefab;
        [SerializeField] [Min(0f)] private float ProjectileCooldown = 0.25f;
        [SerializeField] [Min(0f)] private float ProjectileSpeed = 20f;
        [SerializeField] [Min(0f)] private float ProjectileDamage = 1f;
        
        private Rigidbody _rb;
        private ShooterActions _actions;
        private Camera _mainCamera;

        private bool _isHoldMove;
        private Vector2 _moveVec;
        private bool _isHoldLook;
        private Vector2 _lookVec;
        private bool _isHoldFire;
        private bool _isHoldDodge;
        private bool _isAimDir;

        private bool _fireReady = true;
        
        private void Awake()
        {
            GameManager.Instance.Player = this;
            _actions = new ShooterActions();
            _actions.Player.SetCallbacks(this);
            //actions.Player.Fire.started += Fire;
            //PlayerInput input = GetComponent<PlayerInput>();
            //actions.Player = input.player;
            //actions = (ShooterActions)input.user.actions;
            //input.actions["Fire"].started += Fire;
            //input.onActionTriggered += OnActionTriggered;
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void Update()
        {
            ApplyMove();
            ApplyLook();
            ApplyFire();
        }

        #region Input Events
        
        public void OnMove(InputAction.CallbackContext context)
        {
            //Debug.Log("Move: " + context.action.name);
            _moveVec = context.ReadValue<Vector2>();
            if (context.started)
            {
                _isHoldMove = true;
            }
            else if (context.canceled)
            {
                _isHoldMove = false;
            }
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            Debug.Log("Look: " + context.action.name);
            _lookVec = context.ReadValue<Vector2>();
            _isAimDir = context.control.device != Mouse.current;
            
            if (context.started)
            {
                _isHoldLook = true;
            }
            else if (context.canceled)
            {
                _isHoldLook = false;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            Debug.Log("Fire: " + context.action.name);
            if (context.started)
            {
                _isHoldFire = true;
            }
            else if (context.canceled)
            {
                _isHoldFire = false;
            }
        }
        
        public void OnDodge(InputAction.CallbackContext context)
        {
            Debug.Log("Dodge: " + context.action.name);
        }
        
        #endregion Input Events

        private void ApplyMove()
        {
            float acceleration = _isHoldMove ? Acceleration : Deceleration;
            float recover = 1f + Mathf.Max(0f, -Vector3.Dot(_rb.velocity.normalized, _moveVec.normalized));
            _rb.velocity = Vector2.MoveTowards(_rb.velocity, _moveVec * MoveSpeed, acceleration * recover * Time.deltaTime);
        }

        private void ApplyLook()
        {
            if (_isHoldFire && _lookVec.sqrMagnitude > 0.001f)
            {
                Vector2 fwd;
                if (!_isAimDir && _mainCamera)
                {
                    Vector3 worldPos = _mainCamera.ScreenToWorldPoint(_lookVec);
                    fwd = worldPos - transform.position;
                }
                else
                {
                    fwd = _lookVec;
                }
                    
                transform.up = fwd;
            }
            else
            {
                if (_rb.velocity.sqrMagnitude > 0.001f)
                {
                    transform.up = _rb.velocity;
                }
            }
        }

        private void ApplyFire()
        {
            if (!_isHoldFire || !_fireReady) return;
            _fireReady = false;
            
            Projectile projectile = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
            projectile.Damage = ProjectileDamage;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.velocity = projectile.transform.up * ProjectileSpeed;
            
            
            Invoke(nameof(OnFireCooldown), ProjectileCooldown);
        }

        private void OnFireCooldown()
        {
            _fireReady = true;
        }
    }
}
