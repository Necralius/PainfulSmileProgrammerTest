using NekraByte;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthBar : MonoBehaviour
{    
    //Private Data
    private float targetValue;
    private float lifeReduceSpeed = 2;
    private float currentHealth = 0;

    //Inspector Assigned 
    [SerializeField] private List<ShipHealthState>  _shipDeteriorationPhases    = new List<ShipHealthState>();
    [SerializeField] private GameObject             _currentShipSprite          = null;
    [SerializeField] private Image                  _healthBar                  = null;

    public void UpdateHealthAndVisual(float healthValue, float maxHealth)
    {
        currentHealth       = healthValue;
        targetValue         = healthValue / maxHealth;

        foreach(var obj  in _shipDeteriorationPhases)
        {
            if (currentHealth <= obj.HealthLimit)
            {
                Destroy(_currentShipSprite);
                _currentShipSprite = Instantiate(obj.HealthVisual, transform);
            }
            
        }
    }

    private void Update()
    {
        _healthBar.fillAmount = Mathf.MoveTowards(_healthBar.fillAmount, targetValue, lifeReduceSpeed * Time.deltaTime);
    }
}