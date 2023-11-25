using NekraByte;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;
using Random = UnityEngine.Random;

[Serializable]
public class LoopSoundLayer //This class holds an audio layer to be executed.
{
    private SoundManager _soundManager;

    private float   _loopClipTimer     = 0;
    private float   _clipLength    = 0f;

    [Header("Musics Settings")]
    [SerializeField] private bool               _eventLoops  = true;
    [SerializeField] private List<AudioClip>    _clips     = new List<AudioClip>();

    [SerializeField] private  EventSourceType   _eventType  = EventSourceType.Music;

    public void OnStart()
    {
        _soundManager = SoundManager.Instance;
    }

    public void OnUpdate()
    {
        if (_eventLoops)
        {
            if (_loopClipTimer >= _clipLength) 
                StartEvent(_clips[Random.Range(0, _clips.Count)]);
            else _loopClipTimer += Time.deltaTime;
        }
    }

    private void StartEvent(AudioClip clip)
    {
        _loopClipTimer  = 0f;
        _clipLength     = clip.length;

        switch (_eventType)
        {
            case EventSourceType.Music:     _soundManager.PlayMusicClip(clip);      break;
            case EventSourceType.Ambience:  _soundManager.PlayAmbienceClip(clip);   break;
        }
    }

    public void StopEvent(EventSourceType EventType)//Stops completly the layer.
    {
        switch(EventType)
        {
            case EventSourceType.Music:     _soundManager.StopMusics();     break;
            case EventSourceType.Ambience:  _soundManager.StopAmbience();   break;
        }
    }
}