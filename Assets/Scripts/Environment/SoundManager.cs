using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupItemSound;
    public AudioSource eatSound;

    public AudioSource backgroundMusic;
    public AudioSource ambientSounds;
    public AudioSource voiceovers;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
            soundToPlay.Play();
    }

    public void PlayVoiceOver(AudioClip clip)
    {
        voiceovers.clip = clip;

        if (!voiceovers.isPlaying)
            voiceovers.Play();
        else
        {
            voiceovers.Stop();
            voiceovers.Play();
        }
    }

    public bool IsPlayingVoiceOver()
    {
        return voiceovers.isPlaying;
    }

    public void StopSound(AudioSource soundToStop)
    {
        if (soundToStop.isPlaying)
            soundToStop.Stop();
    }
}