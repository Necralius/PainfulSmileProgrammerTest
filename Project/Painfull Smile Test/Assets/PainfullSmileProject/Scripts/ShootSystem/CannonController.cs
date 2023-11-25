using NekraByte;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Ship Cannons")]
    [SerializeField] private Cannon         _primaryCannon      = null;
    [SerializeField] private List<Cannon>   _secondaryCannons   = new List<Cannon>();

    BulletSettings _bulletSettings;

    public void SetUp(BulletSettings settings) => _bulletSettings = settings;

    public void PrimaryShoot()
    {
        if (_primaryCannon == null) return;
        _primaryCannon.Shoot(_bulletSettings);      
    }

    public void SecondaryShoot()
    {
        if (_secondaryCannons == null || _secondaryCannons.Count <= 0) return;

        StartCoroutine(ShootDelay());
    }
    IEnumerator ShootDelay()
    {
        foreach (Cannon cannon in _secondaryCannons)
        {
            cannon.Shoot(_bulletSettings);
            yield return new WaitForSeconds(0.1f);
        }
    }
}