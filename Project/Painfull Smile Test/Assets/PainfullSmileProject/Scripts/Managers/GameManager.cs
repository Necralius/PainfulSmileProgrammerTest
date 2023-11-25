using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region - Singleton Pattern -
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonInstance = new GameObject(typeof(GameManager).Name);
                    _instance = singletonInstance.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public GameObject playerInstance;
    [SerializeField] private int                _currentScore = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI    _timerTxt       = null;
    [SerializeField] private TextMeshProUGUI    _scoreTxt       = null;
    [SerializeField] private TextMeshProUGUI    _endReasonTxt   = null;

    [Header("End Screen")]
    [SerializeField] private GameObject         _endScreen  = null;
    [SerializeField] private TextMeshProUGUI    _scoreText  = null;

    [Header("Time System")]
    [SerializeField, Range(0f, 180f)]   private float   _sessionTime    = 60f;
    [Range(0f, 20f)]                    public  float   enemySpawnTime  = 6f;

    private float   _timer          = 0;
    private int     _seconds        = 0;
    private int     _minutes        = 0;

    public bool     _gameEnded      = false;

    private void Start()
    {
        _sessionTime    = PlayerPrefs.GetFloat("SessionTime");
        enemySpawnTime  = PlayerPrefs.GetFloat("EnemySpawnTime");
    }

    private void Update()
    {
        if (_timer >= _sessionTime) EndGame("Time is up!");
        else _timer += Time.deltaTime;

        _minutes         = Mathf.FloorToInt(_timer / 60);
        _seconds         = Mathf.FloorToInt(_timer % 60);

        _timerTxt.text   = string.Format("Time:{0:00}:{1:00}", _minutes, _seconds);
        _scoreTxt.text   = $"Score:{_currentScore}";
    }

    public void EndGame(string endReason)
    {
        _gameEnded          = true;
        _scoreText.text     = $"Score:{_currentScore}";
        _endReasonTxt.text  = endReason;

        _endScreen.SetActive(true);
    }
    public void AddScore()                      => _currentScore++;
    public void ReloadScene()                   => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void LoadSceneByIndex(int index)     => SceneManager.LoadScene(index);
}