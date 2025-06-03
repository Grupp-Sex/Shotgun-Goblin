using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ansgar
public class GenralSoundPlayer : MonoBehaviour
{
    protected System.Random rd = new System.Random();

    [Header("Always played Sounds")]
    [SerializeField] protected List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] Vector2 VolumeRange = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 PitchRange = new Vector2(0.5f, 1);

    [Header("RandomSounds")]
    [SerializeField] protected List<SoundClipData> randomSounds = new List<SoundClipData>();

    [Header("Other")]
    public Transform TargetPosition;
    [SerializeField] bool follow = true;
    [SerializeField] bool sound3d = true;
    private void Awake()
    {
        if(TargetPosition == null) TargetPosition = transform;
    }

    
    public virtual void PlaySounds()
    {
        PlaySounds(sounds);
    }
    protected void PlaySounds(List<AudioClip> clips, float soundMult = 1, float pitchMult = 1)
    {
        if (GameAudioManager.AudioManager != null)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                PlaySound(sounds[i], VolumeRange, PitchRange, soundMult, pitchMult);
            }

            PlayRandomSounds(randomSounds, soundMult, pitchMult);
        }
    }

    protected void PlaySound(AudioClip clip,Vector2 volumeRange, Vector2 pitchRange, float soundMult = 1, float pitchMult = 1)
    {
        GameAudioManager.AudioManager.PlayPooledSound(clip, sound3d, follow, false, TargetPosition, default, RandomizeValue(volumeRange) * soundMult, RandomizeValue(pitchRange) * pitchMult);
    }

    protected void PlayRandomSounds(List<SoundClipData> clips, float soundMult = 1, float pitchMult = 1)
    {
        if (clips.Count > 0)
        {
            SoundClipData soundClipData = clips[rd.Next(0, clips.Count)];

            PlaySound(soundClipData.audioClip, soundClipData.VolumeRange, soundClipData.PitchRange, soundMult, pitchMult);

        }
    }

    


    protected float RandomizeValue(Vector2 range)
    {
        int scale = 100;

        return rd.Next((int)(range.x * scale), (int)(range.y * scale)) / (float)scale;
    }
}

[System.Serializable]
public struct SoundClipData
{
    public AudioClip audioClip;
    public Vector2 VolumeRange;
    public Vector2 PitchRange;
    
}
