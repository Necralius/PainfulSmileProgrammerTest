using NekraByte;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : EnemyBase
{
    //Dependencies
    private CannonController controller;
    [SerializeField] private float minShootDistance = 10f;

    [SerializeField] private BulletSettings bulletSettings = new BulletSettings();

    protected override void Start()
    {
        controller = GetComponentInChildren<CannonController>();
        controller.SetUp(bulletSettings);
        base.Start();
    }

    private void Update()
    {
        if (CheckPlayerInRange())
        {
            _chasePlayer = false;
            controller.PrimaryShoot();
        }
        else _chasePlayer = true;
    }

    private bool CheckPlayerInRange()
    {
        return Vector2.Distance(transform.position, _target.transform.position) <= minShootDistance;
    }
}