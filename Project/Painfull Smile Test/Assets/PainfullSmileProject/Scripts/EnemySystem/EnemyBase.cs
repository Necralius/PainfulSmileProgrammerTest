using NekraByte;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour, Damageable, IPolled
{
    //Dependencies
    protected                   Rigidbody2D     _rb         => GetComponent<Rigidbody2D>();
    [SerializeField] private    ShipHealthBar   _healthBar;


    //Inspector Assigned
    [Header("Target Settings")]
    [SerializeField] protected GameObject _target;

    [Header("Movment Settings")]
    [SerializeField, Range(0.1f, 10)] protected float   _travelSpeed    = 2f;
    [SerializeField, Range(0.1f, 10)] private   float   _rotationSpeed  = 5f;

    [SerializeField, Range(10, 200)]  private float     _maxHealth      = 200f;

    [Header("Audio")]
    [SerializeField] private AudioClip _deathClip = null;

    private float _currentHealth = 200f;

    protected bool _chasePlayer = true;

    protected Vector3 direction;

    public float Health
    {
        get => _currentHealth;
        set
        {
            if (value <= 0)
            {
                Die();
                _currentHealth = 0;
            }
            else _currentHealth = value;

            _healthBar.UpdateHealthAndVisual(_currentHealth, _maxHealth);
        }
    }

    protected virtual void Start()
    { 
        _currentHealth = _maxHealth;
        _healthBar.UpdateHealthAndVisual(_currentHealth, _maxHealth);
    }

    private void OnEnable()
    {
        _target = GameManager.Instance.playerInstance;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;

        if (_chasePlayer) FollowPlayer();
        RotateTowardsPlayer();
    }

    protected virtual void FollowPlayer()
    {
        direction       = transform.position - _target.transform.position;
        _rb.velocity    = -direction * _travelSpeed / 100;
    }

    private void RotateTowardsPlayer()
    {
        Vector2 dir = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    protected void Die()
    {
        GameManager.Instance.AddScore();

        GameObject explosion = ObjectPooler.Instance.GetFromPool("Explosion", transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector3.one;

        SoundManager.Instance.SoundEffect(_deathClip, transform.position);

        _chasePlayer            = false;
        gameObject.SetActive(false);
    }

    public void Pooled()
    {
        _currentHealth = _maxHealth;
        _chasePlayer    = true;
    }
}