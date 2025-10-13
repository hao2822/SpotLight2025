using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Utils;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonBehaviour<AudioManager>
{
    public Dictionary<GameBGM, List<AudioClip>> GameBGMMaps;
    public Dictionary<GameAFX, AudioClip> GameAFXMaps;
    public AudioSource audioSource;
    public AudioSource AFXAudioSource;
    public AudioSource DialogueAudioSource;
    public GameBGM CurrentBGM;
    
    private int CurrentBGMIndex = -1;
    private const string _audioMapPath = "SOData/MapSO/AudioMaps";


    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
    
    public void Init()
    {
        var audioMap = Resources.Load<AudioMaps>(_audioMapPath);
        GameBGMMaps = audioMap?.GameBGMMaps;
        GameAFXMaps = audioMap?.GameAFXMaps;
        audioSource = GetComponent<AudioSource>();
        if (AFXAudioSource == null)
        {
            GameObject AFX = new GameObject("AFXAudioSource");
            AFX.transform.parent = transform;
            AFXAudioSource = AFX.AddComponent<AudioSource>();
        }
        if (DialogueAudioSource == null)
        {
            GameObject DLG = new GameObject("DialogueAudioSource");
            DLG.transform.parent = transform;
            DialogueAudioSource = DLG.AddComponent<AudioSource>();
        }
        audioSource.volume = LocalStorageUtil.HasKey(LocalStorageKeys.BGMVolume) ? LocalStorageUtil.GetFloat(LocalStorageKeys.BGMVolume) : 0.5f;
        AFXAudioSource.volume = LocalStorageUtil.HasKey(LocalStorageKeys.AFXVolume) ? LocalStorageUtil.GetFloat(LocalStorageKeys.AFXVolume) : 0.5f;

        //SetGlobalVolume(0.5f);
    }

    public void Play(GameBGM type, bool loop = false,int index = 0)
    {
        if (GameBGMMaps.ContainsKey(type))
        {
            if (type == CurrentBGM)
            {
                if(index == CurrentBGMIndex)
                    return;
            }
            //audioSource.volume = 1;
            audioSource.clip = GameBGMMaps[type][index];
            audioSource.loop = loop;
            audioSource.Play();
            CurrentBGM = type;
            CurrentBGMIndex = index;
        }
    }

    public void PlayAFX(GameAFX type, bool loop = false)
    {
        if (GameAFXMaps.ContainsKey(type))
        {
            AFXAudioSource.clip = GameAFXMaps[type];
            AFXAudioSource.loop = loop;
            AFXAudioSource.Play();
        }
    }

    public void PlayDLG(GameAFX type, bool loop = false)
    {
        if (GameAFXMaps.ContainsKey(type))
        {
            DialogueAudioSource.clip = GameAFXMaps[type];
            DialogueAudioSource.loop = loop;
            DialogueAudioSource.Play();
        }
    }

    public void StopBGM()
    {
        audioSource.clip = null;
        audioSource.loop = false;
        audioSource.Stop();
    }
    public void StopAFX()
    {
        AFXAudioSource.clip = null;
        AFXAudioSource.loop = false;
        AFXAudioSource.Stop();
    }

    public void StopDLG()
    {
        DialogueAudioSource.clip = null;
        DialogueAudioSource.loop = false;
        DialogueAudioSource.Stop();
    }

    public void FadeOut(float fadeDuration, bool stop, Action callback = null)
    {
        StartCoroutine(fadeOut(fadeDuration, stop, callback));
    }

    public void PlayAudio(bool fadeout, float fadeDuration, GameBGM type, Action callback = null)
    {
        StartCoroutine(playAudio(fadeout, fadeDuration, type, callback));
    }

    IEnumerator fadeOut(float fadeDuration, bool stop = false, Action callback = null)
    {
        float startVolume = audioSource.volume;
        var oriclip = audioSource.clip;
        while (audioSource.volume > 0 && oriclip == audioSource.clip)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = startVolume;
        if (stop && oriclip == audioSource.clip)
            audioSource.Stop();
        callback?.Invoke();
    }

    public AudioClip GetAudioClip(GameAFX type)
    {
        return GameAFXMaps[type];
    }
    IEnumerator playAudio(bool fadeout, float fadeDuration, GameBGM type, Action callback = null)
    {
        if (fadeout)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        else
        {
            audioSource.Stop();
        }

        Play(type);
        CurrentBGM = type;
        callback?.Invoke();
    }

    public void SetGlobalVolume(float v)
    {
        audioSource.volume = AFXAudioSource.volume = v;

    }
}

public enum GameBGM
{
    TitleScreen,
    
}

public enum GameAFX
{
    Turn,
    Get,
    Tap,
    Lost
    
    
}