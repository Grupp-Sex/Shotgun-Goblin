using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    /*The Header is a Unity attribute, it just makes a header/title for the groups, 
    making it look more neat and organized in the inspector. 
        
    AudioMixer: contain and controls all groups
    AudioMixerGroup: routes audio sources
    AudioManager: Controls playback and volume via the mixer
    
     We're making these AudioMixerGroups to have a better organization and navigation over the sounds,
    however they still need to be manually added in unity, so we must remember that.*/

    [Header("Mixer & MixerGroups")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixerGroup musicMixGroup;
    [SerializeField] private AudioMixerGroup sfxMixGroup;
    [SerializeField] private AudioMixerGroup ambientMixGroup;

    [Header("3D SFX Pool")]
    /*The Gameobject here holds a reference to a prefab, so it's a field in the Inspector where
     you can place your relevant prefab. This instntiates copies of the prefab to build a pool
    of ready-to-use audiosources.*/
    [SerializeField] private GameObject audio3DPrefab;
    /*SFXPoolSize is just an integer that hold a maximum of Active sounnds*/
    [SerializeField] private int sfxPoolSize = 20;
    

    
}
