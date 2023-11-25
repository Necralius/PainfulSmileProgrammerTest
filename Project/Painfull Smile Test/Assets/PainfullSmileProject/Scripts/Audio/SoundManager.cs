using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region - Singleton Pattern -
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();

                if (_instance == null)
                {
                    GameObject singletonInstance = new GameObject(typeof(SoundManager).Name);
                    _instance = singletonInstance.AddComponent<SoundManager>();
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

    //Dependencies
    [Header("Source")]
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource musicSource;

    //Public data
    [Header("Public Clips")]
    public AudioClip shootClip;

    //Inspector Assigned
    [Header("Loop Sound Layers")]
    [SerializeField] private List<LoopSoundLayer> loopSoundLayers;

    private void Start()
    {
       if (loopSoundLayers != null) 
            foreach(var layer in loopSoundLayers) layer.OnStart();
    }
    private void Update()
    {
        if (loopSoundLayers != null)
            foreach (var layer in loopSoundLayers) layer.OnUpdate();
    }

    public void ShootSound(AudioClip clip, Vector2 volumeRange, Vector3 position)
    {
        effectSource.pitch    = Random.Range(0.85f, 1f);
        effectSource.volume   = Random.Range(volumeRange.x, volumeRange.y);

        effectSource.gameObject.transform.position = position;

        effectSource.PlayOneShot(clip);
    }

    public void SoundEffect(AudioClip clip, Vector3 position)
    {
        effectSource.gameObject.transform.position = position;
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusicClip(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
    public void PlayAmbienceClip(AudioClip clip)
    {
        ambienceSource.PlayOneShot(clip);
    }

    public void StopMusics()    => musicSource.Stop();
    public void StopAmbience()  => musicSource.Stop();
}