using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ansgar
public class GenralSoundPlayer : MonoBehaviour
{
    protected System.Random rd = new System.Random();
    [SerializeField] protected List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] Vector2 VolumeRange = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 PitchRange = new Vector2(0.5f, 1);
    public Transform TargetPosition;
    [SerializeField] bool follow = true;
    [SerializeField] bool sound3d = true;
    private void Awake()
    {
        if(TargetPosition == null) TargetPosition = transform;
    }

    protected void PlaySounds(List<AudioClip> clips, float soundMult = 1, float pitchMult = 1)
    {
        if (GameAudioManager.AudioManager != null)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                GameAudioManager.AudioManager.PlayPooledSound(sounds[i], sound3d, follow, false, TargetPosition, default, RandomizeValue(VolumeRange) * soundMult, RandomizeValue(PitchRange) * pitchMult);
            }
        }
    }

    protected float RandomizeValue(Vector2 range)
    {
        int scale = 100;

        return rd.Next((int)(range.x * scale), (int)(range.y * scale)) / (float)scale;
    }
}
