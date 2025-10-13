using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioMaps", menuName = "ScriptableObjects/Map/AudioMaps")]
public class AudioMaps : SerializedScriptableObject
{
    public Dictionary<GameBGM,List<AudioClip>> GameBGMMaps = new Dictionary<GameBGM, List<AudioClip>>();
    public Dictionary<GameAFX, AudioClip> GameAFXMaps = new Dictionary<GameAFX, AudioClip>();
}
