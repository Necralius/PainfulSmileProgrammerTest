using NekraByte;
using UnityEngine;

public class Chaser : EnemyBase
{
    protected override void FollowPlayer()
    {
        if (Vector2.Distance(transform.position, _target.transform.position) > 5) 
            direction = transform.position - _target.transform.position;
        _rb.velocity = -direction * _travelSpeed / 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable objectFinded = collision.gameObject.GetComponent<Damageable>();

        if (objectFinded == null || objectFinded.Equals(null)) return;

        if (collision.transform.CompareTag("Player")) 
            collision.gameObject.GetComponent<Damageable>().TakeDamage(50);
        Die();
    }
}