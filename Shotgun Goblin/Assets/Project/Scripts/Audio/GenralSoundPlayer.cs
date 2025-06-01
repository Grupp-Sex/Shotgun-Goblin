using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenralSoundPlayer : MonoBehaviour
{
    protected System.Random rd = new System.Random();
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] Vector2 VolumeRange = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 PitchRange = new Vector2(0.5f, 1);

    protected void PlaySounds()
    {
        if (GameAudioManager.AudioManager != null)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                GameAudioManager.AudioManager.PlayPooledSound(sounds[i], false, false, default, default, default, RandomizeValue(VolumeRange), RandomizeValue(PitchRange));
            }
        }
    }

    protected float RandomizeValue(Vector2 range)
    {
        int scale = 100;

        return rd.Next((int)(range.x * scale), (int)(range.y * scale)) / (float)scale;
    }
}
