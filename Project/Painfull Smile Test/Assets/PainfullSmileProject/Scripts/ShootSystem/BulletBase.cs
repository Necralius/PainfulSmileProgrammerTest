using NekraByte;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBase : MonoBehaviour
{
    //Dependencies
    private Rigidbody2D _rb => GetComponent<Rigidbody2D>();

    //Inspector Assigned Data
    [Header("Speed Settings")]
    [SerializeField, Range(0, 100)] private float   _bulletSpeed = 10;

    [Header("Effects Settings")]
    [SerializeField]                private string  _explosionPrefabTag = string.Empty;

    [Header("Audio")]
    [SerializeField] private AudioClip _impactClip = null;

    //Private Data
    private float _deactivateTimer   = 0;
    private float _deactiveTime      = 5;

    private float _damage = 10;
    private void Update()
    {
        if (_deactivateTimer >= _deactiveTime)
        {
            gameObject.SetActive(false);
            _deactivateTimer = 0;
        }
        else _deactivateTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.right * _bulletSpeed / 10;
    }

    public void SetUp(BulletSettings settings)
    {
        _bulletSpeed    = settings.Speed;
        _damage         = settings.Damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);

        Damageable objectFinded = collision.gameObject.GetComponent<Damageable>();

        if (objectFinded == null || objectFinded.Equals(null)) return;
         collision.gameObject.GetComponent<Damageable>().TakeDamage(_damage);

        ObjectPooler.Instance.GetFromPool("Explosion", collision.transform.position, Quaternion.identity);
        SoundManager.Instance.SoundEffect(_impactClip, transform.position);
    }
}