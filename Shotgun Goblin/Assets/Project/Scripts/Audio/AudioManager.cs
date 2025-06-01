using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.UIElements;


public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambientSource;
    private Coroutine ambientFadeCoroutine;
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
    [SerializeField] private AudioMixerGroup sfxMixGroup, ambientMixGroup, uiMixGroup;

    
    // sfxPoolSize is just an integer that hold a maximum of Active sounds
    [Header("3D Audio Source Prefab")]
    [SerializeField] private AudioSource audio3DPrefab;
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
        //DontDestroyOnLoad(gameObject);
        SetupPool();
        SetupAmbientSource();

        GameAudioManager.SetAudioManager(this);
    }

    private void SetupPool()
    {
        sourcePool = new ObjectPool<AudioSource> (
            createFunc: (CreateAudioSourcePool),
            actionOnGet: GetPooledAudioSource,
            actionOnRelease: ReleasePooledAudioSource,
            actionOnDestroy: DestroyPooledAudioSource,
            collectionCheck: false,
            defaultCapacity: sfxPoolMaxSize, 
            maxSize: sfxPoolMaxSize * 2
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


    public void PlayPooledSound(AudioClip clip, bool is3D, bool follow, bool isPooled = true, Transform position = default, AudioMixerGroup group = null, float volume = 1f, float pitch = 1f)
    {
        //Make sure to not play the sound if there's no audioclip
        if (clip == null)
            return;


        AudioSource source;
        if (isPooled)
        {
            source = sourcePool.Get();
        }
        else
        {
            GameObject g = new GameObject("AudioSouce");

            source = g.AddComponent<AudioSource>();
        }
    
        source.clip = clip;
        source.outputAudioMixerGroup = group ?? sfxMixGroup; //Assigns the input group into the source, and if it's null it'll by default go into sfxMixGroup
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = is3D ? 1f : 0f; //1f == 3D sound, 0f == 2D sound


        source.transform.position = Vector3.zero;

        if (is3D)
        {
            
            source.transform.SetParent(position);
            source.transform.localPosition = Vector3.zero;
            if (!follow)
            {
                source.transform.parent = null;
            }
        }
        

        source.Play();

        /*To avoid the source from releasing it back to the pool right away, risking it to cut off the audio,
         we use a coroutine. Coroutines is a feature that lets you pause execution and resume it later. Since
         Unity's main Update loop doesn't support waiting/sleeping, the best option is to use a coroutine 
         to delay an action asynchronously.*/

        if (isPooled)
        {
            StartCoroutine(ReleaseAfterPlay(source));
        }
        else
        {
            StartCoroutine(DestroyAfterPlay(source));
        }
    }

    private void SetupAmbientSource()
    {
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.outputAudioMixerGroup = ambientMixGroup;
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;
        ambientSource.spatialBlend = 0f;
    }
    public void PlayAmbient(AudioClip clip, float volume = 1f, float fadeDuration = 0f)
    {
        if (clip == null) return;

        if (ambientFadeCoroutine != null)
        {
            StopCoroutine(ambientFadeCoroutine);
        }

        /*Only if a fadeDuration is requested and something is already playing will
        the "fade out - switch clip - fade in" be performed.*/
        if (fadeDuration > 0f && ambientSource.isPlaying)
        {
            ambientFadeCoroutine = StartCoroutine(FadeAmbientOutAndIn(clip, volume, fadeDuration));
        }
        else
        {
            ambientSource.clip = clip;
            ambientSource.volume = volume;
            ambientSource.Play();
        }
    }

    private IEnumerator ReleaseAfterPlay(AudioSource source)
    {
        /*The yield here is what gives us the delay, and instead of using WaitForSeconds
         we wait until the source finish playing. This makes sure that the audioclip in the
         source gets to finish before it get released. */
        yield return new WaitWhile(() => source.isPlaying);
        sourcePool.Release(source);
    }

    private IEnumerator DestroyAfterPlay(AudioSource source)
    {
        /*The yield here is what gives us the delay, and instead of using WaitForSeconds
         we wait until the source finish playing. This makes sure that the audioclip in the
         source gets to finish before it get released. */
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(source.gameObject);
    }

    private IEnumerator FadeAmbientOutAndIn(AudioClip newClip, float targetVolume, float duration)
    {
        float t = 0f;
        float startVol = ambientSource.volume;

        //Fading the audio out
        while (t < duration)
        {
            t += Time.deltaTime;
            ambientSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }

        ambientSource.Stop();
        ambientSource.clip = newClip;
        ambientSource.Play();

        //Fading the audio in
        while (t < duration)
        {
            t += Time.deltaTime;
            ambientSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        ambientSource.volume = targetVolume;
        ambientFadeCoroutine = null;
    }

    //private IEnumerator StopAmbience()

    private void GetPooledAudioSource(AudioSource src)
    {
        src.gameObject.SetActive(true);
    }

    private void ReleasePooledAudioSource(AudioSource src)
    {
        if (src != null)
        {
            src.gameObject.SetActive(false);
        }
    }

    private void DestroyPooledAudioSource(AudioSource src)
    {
        Destroy(src.gameObject);
    }
}
