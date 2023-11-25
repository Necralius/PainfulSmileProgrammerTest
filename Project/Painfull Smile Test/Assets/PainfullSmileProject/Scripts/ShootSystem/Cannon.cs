using NekraByte;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    float reloadTimer   = 0;
    float timeToReload  = 1.5f;

    [SerializeField] private    bool                _canShoot           = true;

    [SerializeField] private    string              _bulletTag          = string.Empty;
    [SerializeField] private    SpriteRenderer      _reloadSlider       = null;
    [SerializeField] private    Transform           _bulletSpawnPos     = null;

    private void Update()
    {
        _reloadSlider.gameObject.SetActive(!_canShoot);

        if (reloadTimer >= timeToReload)
        {
            reloadTimer = timeToReload;
            _canShoot    = true;
        }
        else reloadTimer += Time.deltaTime;

        _reloadSlider.material.SetFloat("_RemovedSegments", reloadTimer / timeToReload);
    }

    public void Shoot(BulletSettings settings)
    {
        if (_canShoot)
        {
            _canShoot        = false;
            reloadTimer     = 0;

            GameObject bullet = ObjectPooler.Instance.GetFromPool(_bulletTag, _bulletSpawnPos.position, _bulletSpawnPos.rotation);
            bullet.GetComponent<BulletBase>().SetUp(settings);

            SoundManager.Instance.ShootSound(SoundManager.Instance.shootClip, new Vector2(0.85f, 1f), transform.position);
        }
        else return;
    }
}