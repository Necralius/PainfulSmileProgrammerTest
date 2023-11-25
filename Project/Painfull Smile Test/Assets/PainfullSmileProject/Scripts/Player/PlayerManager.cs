using NekraByte;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(InputManager))]
public class PlayerManager : MonoBehaviour, Damageable
{

    //Dependencies
    private Rigidbody2D        _rb                  => GetComponent<Rigidbody2D>();
    private InputManager       _inputManager        => GetComponent<InputManager>();
    private CannonController   _cannonController    => GetComponentInChildren<CannonController>();
    private ShipHealthBar      _healthBar           => GetComponentInChildren<ShipHealthBar>();

    //Inspector Assigned
    [Header("Player State")]
    [SerializeField, Range(0, 200)] private float _playerHealth     = 100;

    [Header("Movement Settings")]
    [SerializeField, Range(1, 100)] private float _forwardSpeed     = 10;
    [SerializeField, Range(1, 100)] private float _rotateSpeed      = 10;

    [Header("Player State")]
    [SerializeField] private bool _isDead    = false;

    [Header("Bullet Settings")]
    [SerializeField] private BulletSettings       _bulletSettings   = new BulletSettings();

    //Encapsulated Data
    public float PlayerHealth
    {
        get => _playerHealth;
        set
        {
            if (value <= 0)
            {
                _playerHealth = 0;
                Death();              
            }
            else _playerHealth = value;
            _healthBar.UpdateHealthAndVisual(_playerHealth, 200);
        }
    }

    private void Start()
    {
        _cannonController.SetUp(_bulletSettings);
        _healthBar.UpdateHealthAndVisual(_playerHealth, 200);
    }

    private void Update()
    {
        if (_isDead || GameManager.Instance._gameEnded) return;

        if (_inputManager.primaryShoot.WasPressedThisFrame())   _cannonController.PrimaryShoot();
        if (_inputManager.secondaryShoot.WasPressedThisFrame()) _cannonController.SecondaryShoot();
    }

    private void FixedUpdate()
    {
        if (_isDead || GameManager.Instance._gameEnded) return;
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        //Forward Thurst
        if (_inputManager.moveAction.IsPressed())
        {
            if (_inputManager._moveInput.y > 0)
            {
                Vector2 forceValue = -transform.up * _forwardSpeed / 10;
                _rb.velocity = forceValue;
            }
        }

        //Rotation
        if (_inputManager.moveAction.IsPressed())
        {
            float rotationAmount = _inputManager._moveInput.x * _rotateSpeed;
            transform.Rotate(-Vector3.forward, rotationAmount);
        }
    }

    public void TakeDamage(float amount)
    {
        PlayerHealth -= amount;
    }

    private void Death()
    {
        _rb.freezeRotation   = false;
        _isDead              = true;
        GameManager.Instance.EndGame("You died!");
    }
}