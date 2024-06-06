using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Main theme");
        Play("Ambient");
    }

    public void Play(string name)
    {
        if (sounds == null)
        {
            Debug.LogWarning("Audio name : " + name + " not found");
            return;
        }
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
