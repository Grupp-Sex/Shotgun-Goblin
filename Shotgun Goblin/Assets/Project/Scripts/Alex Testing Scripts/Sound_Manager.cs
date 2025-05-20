using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This enum will represent all the sounds that are to be used
public enum SoundType
{
    //Remember to switch out the names as soon as we have sounds
    SoundEx1,
    SoundEx2,
    SoundEx3,
    SoundEx4,
}

//This requirement makes sure that the audiosource always has a component
[RequireComponent(typeof(AudioSource))]
public class Sound_Manager : MonoBehaviour
{
    //serialized field allows visibility of private non-static fields in the inspector,
    //This happens automatically to public fields by Unity
    //We need to pass through all the sounds we want to play, thus we make an array
    [SerializeField] private AudioClip[] soundArray;
    private static Sound_Manager instance;
    private AudioSource audioSource;

    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //We want the paremeter to represent what we want to pass through, which is what soundType of sound and at what volume.
    //The volume is set and will always be at 1, so that it's always at maximum (1:1) volume. Should be set between 0-1
    //to avoid clipping/distortion
    public static void PlaySound(SoundType soundType, float volume = 1)
    {
        //We need to get the audio source, but because this method is static, and the audio source is not, we go through the 
        //instance.
        //The PlayOneShot allows 
        instance.audioSource.PlayOneShot(instance.soundArray[(int)soundType], volume);
    }
}
