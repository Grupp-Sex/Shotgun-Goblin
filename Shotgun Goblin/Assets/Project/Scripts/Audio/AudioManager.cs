using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    /*The Header is a Unity attribute, it just makes a header/title for the groups, 
    making it look more neat and organized in the inspector. 
        
    **NOTE TO SELF**
    *KNOW THE DIFFERENCE*
    - AudioMixer: contain and controls all groups
    - AudioMixerGroup: routes audio sources
    - AudioManager: Controls playback and volume via the mixer
    
     We're making these AudioMixerGroups to have a better organization and navigation over the sounds,
    however they still need to be manually added in unity after adding a mixer, so just REMEMBER that.*/

    [Header("Mixer & SubGroups")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixerGroup musicMixGroup, sfxMixGroup, ambientMixGroup, uiMixGroup;

    
    [Header("3D Audio Source Prefab")]
    [SerializeField] private AudioSource audio3DPrefab;
    // SFXPoolSize is just an integer that hold a maximum of Active sounds
    [SerializeField] private int sfxPoolMaxSize = 20;

    private ObjectPool<AudioSource> sourcePool;

    void Awake()
    {
        /*The if-section checks if there's already an instance of the AudioManager in the scene,
         and if it's the current one if there is.
        If there already is one, and it's not this one, then this one will be destroyed, as there
        should only be one AudioManager. It returns early, skipping the rest of the steps. */
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupPool();
    }

    private void SetupPool()
    {
        sourcePool = new ObjectPool<AudioSource> (
            createFunc: (CreateAudioSourcePool),
            actionOnGet: GetPooledAudioSource,
            actionOnRelease: ReleasePooledAudioSource,
            actionOnDestroy: DestroyPooledAudioSource
            );
    }

    private AudioSource CreateAudioSourcePool()
    {
        var src = Instantiate(audio3DPrefab, transform);
        //makes sure it doesn't play when it's created
        src.playOnAwake = false;
        //inactivates the audiosource so it's stored disabled in the pool, ready to be used
        src.gameObject.SetActive(false);
        return src;
    }

    private void GetPooledAudioSource(AudioSource src)
    {
        src.gameObject.SetActive(true);
    }

    private void ReleasePooledAudioSource(AudioSource src)
    {
        src.gameObject?.SetActive(false);
    }

    private void DestroyPooledAudioSource(AudioSource src)
    {
        Destroy(src.gameObject);
    }
}
