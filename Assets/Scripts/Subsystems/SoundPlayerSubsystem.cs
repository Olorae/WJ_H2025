using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*you place this script on an empty object in the first scene of the game and add an AudioSource component
 which you give the first audio clip to play and check the play on awake and loop boxes*/

//Can make this class inherit from ISubsystem to access it with the subsystemPlugin
public class SoundPlayerSubsystem : MonoBehaviour, ISubSystem
{
    
    public static SoundPlayerSubsystem SoundPlayerSubsystemInstance;

    private AudioSource source;

    public AudioClip ArmureSFX;
    public AudioClip HitSFX;
    public AudioClip PortailSFX;
    public AudioClip BossMusic;
    public AudioClip DeadMusic;
    public AudioClip LivingMusic;
    public AudioClip SwordSFX;
    public AudioClip btnSound;

    public AudioClip LoseJingle;
    public AudioClip VictoryJingle;
    public AudioClip DeathEnemy;
    public AudioClip DeathBoss;
    public AudioClip EvilLaugh;
    public AudioClip MenuMusic;

    [Range(0.0f, 1.0f)]
    public float maxVolume = 1.0f;

    //initialize the singleton
    private void Awake()
    {
        if (SoundPlayerSubsystemInstance == null)
        {
            SoundPlayerSubsystemInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    //start
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //play a sound effect once
    public void PlaySFX(AudioClip sfx, float volumeMultiplier = 1.0f)
    {
        source?.PlayOneShot(sfx, volumeMultiplier);
    }

    //change the background music
    public void SetMusic(AudioClip newMusic, float transitionTime = 1.0f)
    {
        StartCoroutine(MusicTransitionRoutine(newMusic, transitionTime));
    }

    //handle the music transition
    IEnumerator MusicTransitionRoutine(AudioClip newMusic, float transitionTime)
    {
        if (source == null)
        {
            yield break;
        }
        
        float remainingTime = transitionTime;
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            source.volume = maxVolume * (remainingTime / transitionTime);
            yield return null;
        }
        
        source.Stop();
        source.clip = newMusic;
        source.Play();
        
        remainingTime = transitionTime;
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            source.volume = maxVolume * (1.0f - (remainingTime / transitionTime));
            yield return null;
        }
    }
    
    //if you also use the subsystems plugin
    public static ISubSystem GetSubSystem()
    {
        return SoundPlayerSubsystemInstance;
    }
 
}
