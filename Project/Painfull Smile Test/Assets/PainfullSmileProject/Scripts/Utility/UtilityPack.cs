using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraByte
{
    [Serializable]
    public class PoolData
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    [Serializable]
    public class ShipHealthState
    {
        public float        HealthLimit     = 100;
        public GameObject   HealthVisual    = null;
    }

    [Serializable]
    public class BulletSettings
    {
        public float Speed      = 80;
        public float Damage     = 10;
    }

    public enum EventSourceType { Music, Ambience }
    public interface IPolled
    {
        public void Pooled();
    }

    public interface Damageable
    {
        public void TakeDamage(float amount);
    }
}