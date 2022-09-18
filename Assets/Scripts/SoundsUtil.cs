using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds<T> : MonoBehaviour where T : Enum
{
    [SerializeField] protected MinigameAudioClip<T>[] clips;

    private AudioSource audioManager;

    protected virtual void Awake()
    {
        audioManager = FindObjectOfType<AudioSource>();

        clips ??= new MinigameAudioClip<T>[0];
    }

    public void AddClips(T clipType, AudioClip[] newClips)
    {
        clips = new List<MinigameAudioClip<T>>(clips) {new MinigameAudioClip<T>(clipType, newClips)}.ToArray();
    }

    public void AddClip(MinigameAudioClip<T> newClip)
    {
        clips = new List<MinigameAudioClip<T>>(clips) {newClip}.ToArray();
    }

    public void AddClips(IEnumerable<MinigameAudioClip<T>> newClips)
    {
        var clipsList = new List<MinigameAudioClip<T>>(clips);
        clipsList.AddRange(newClips);
        clips = clipsList.ToArray();
    }

    public void PlayClip(T clipType, Vector3 position = default)
    {
        if (!audioManager)
            return;

        foreach (var clip in clips)
        {
            if (!Equals(clip.ClipType, clipType))
                continue;

            audioManager.PlayOneShot(clip.Clip/*, position*/);
            break;
        }
    }
}

[Serializable]
public class MinigameAudioClip<T> where T : Enum
{
    public MinigameAudioClip() { }

    public MinigameAudioClip(T newClipType, AudioClip[] newClips)
    {
        clipType = newClipType;
        clips = newClips;
    }

    public T ClipType => clipType;
    public AudioClip Clip => clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;

    [SerializeField] private T clipType;
    [SerializeField] private AudioClip[] clips;
}
