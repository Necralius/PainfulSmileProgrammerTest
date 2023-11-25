using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField, Range(0f, 20f)] private float _spawnByTime = 4f;
    [SerializeField, Range(0f, 70f)] private float _spawnRadius = 15f;

    [Header("Enemys Types")]
    [SerializeField] private List<string>          _enemysTypesTags = new List<string>();

    private GameObject  _playerObject   = null;
    private float       _spawnTimer     = 0;

    [SerializeField] private Transform[] squareCorners;

    private int maxTries = 6;

    private void Start()
    {
        _playerObject   = GameManager.Instance.playerInstance;
        _spawnByTime    = GameManager.Instance.enemySpawnTime;
    }

    private void Update()
    {
        EnemySpawnBehavior();
    }

    private void EnemySpawnBehavior()//Spawn enemy with time
    {
        if (_spawnTimer >= _spawnByTime)
        {
            SpawnEnemyOutsideRadius();
            _spawnTimer = 0;
        }
        else _spawnTimer += Time.deltaTime;
    }

    //Spawn an enemy outside an certain circle radius.
    private void SpawnEnemyOutsideRadius()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);

        Vector3 spawnPosition = _playerObject.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _spawnRadius;

        //Trie to find an valid point inside the game square.
        for (int i = 0; i <= maxTries; i++)
        {
            if (PointIsInsideSquare(spawnPosition)) break;
            else spawnPosition = _playerObject.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _spawnRadius;
        }

        spawnPosition.z = 0;
        
        //Get the enemy from the object pooler and spawns it.
        ObjectPooler.Instance.GetFromPool(_enemysTypesTags[Random.Range(0, _enemysTypesTags.Count)], spawnPosition, Quaternion.identity);
    }

    bool PointIsInsideSquare(Vector2 point)//Verify if the given point is iside of an square.
    {
        int j = squareCorners.Length - 1;
        bool isInside = false;

        for (int i = 0; i < squareCorners.Length; i++)
        {
            if (((squareCorners[i].position.y <= point.y 
                && point.y < squareCorners[j].position.y) 
                || (squareCorners[j].position.y <= point.y 
                && point.y < squareCorners[i].position.y))
                && (point.x < (squareCorners[j].position.x - squareCorners[i].position.x) * (point.y - squareCorners[i].position.y) / (squareCorners[j].position.y - squareCorners[i].position.y) + squareCorners[i].position.x))
            {
                isInside = !isInside;
            }
        }
        return isInside;
    }
}