using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SingleShotGunSound, MultipleShootGunSound, BulletHit, BG, Explosion
}

[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool playonAwake;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] sounds;

    private void Awake() {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playonAwake;
            sound.source.loop = sound.loop;
        }
    }

    private void Start() 
    {
        Play(SoundType.BG);
    }

    public void Play(SoundType soundType) {
        Sound audio = Array.Find(sounds, sound => sound.soundType == soundType);
        //audio.source.PlayOneShot(audio.audioClip);
        audio.source.Play();
    }

    public void Stop(SoundType soundType) {
        Sound audio = Array.Find(sounds, sound => sound.soundType == soundType);
        audio.source.Stop();
    }
}