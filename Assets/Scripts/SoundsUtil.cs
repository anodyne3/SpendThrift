//unused tool

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds<T> : MonoBehaviour where T : Enum
{
    [SerializeField] protected SoundsClip<T>[] clips;

    private AudioSource audioManager;

    protected virtual void Awake()
    {
        audioManager = FindObjectOfType<AudioSource>();

        clips ??= Array.Empty<SoundsClip<T>>();
    }

    public void AddClips(T clipType, AudioClip[] newClips)
    {
        clips = new List<SoundsClip<T>>(clips) {new(clipType, newClips)}.ToArray();
    }

    public void AddClip(SoundsClip<T> newClip)
    {
        clips = new List<SoundsClip<T>>(clips) {newClip}.ToArray();
    }

    public void AddClips(IEnumerable<SoundsClip<T>> newClips)
    {
        var clipsList = new List<SoundsClip<T>>(clips);
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

            audioManager.PlayOneShot(clip.Clip /*, position*/);
            break;
        }
    }
}

[Serializable]
public class SoundsClip<T> where T : Enum
{
    public SoundsClip() { }

    public SoundsClip(T newClipType, AudioClip[] newClips)
    {
        clipType = newClipType;
        clips = newClips;
    }

    public T ClipType => clipType;
    public AudioClip Clip => clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;

    [SerializeField] private T clipType;
    [SerializeField] private AudioClip[] clips;
}
