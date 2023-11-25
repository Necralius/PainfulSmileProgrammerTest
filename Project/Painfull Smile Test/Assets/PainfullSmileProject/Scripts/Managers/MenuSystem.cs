using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SliderFloatField spawnTime     = null;
    [SerializeField] private SliderFloatField sessionTime   = null;

    public void LoadSceneByIndex(int index) => SceneManager.LoadScene(index);

    public void Apply()//Apply the options settings
    {
        PlayerPrefs.SetFloat("EnemySpawnTime", spawnTime.GetValue());
        PlayerPrefs.SetFloat("SessionTime", sessionTime.GetValue());
    }
}