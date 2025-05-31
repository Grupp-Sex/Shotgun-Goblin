using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    #region Player
    PlayerWalking,
    PlayerJump, 
    PlayerDash, 
    PlayerLanding,
    TakenDamage, 
    HealthSpecSFX, 
    OffGroundSFX,
    #endregion
    #region Player Attacks
    Telekinesis, 
    ShotgunShot, 
    ShotgunReload,
    #endregion
    #region Goblin
    GoblinWalking, 
    GoblinDeath, 
    GoblinMeleeAttack,
    GoblinRangedAttack, 
    GoblinIdle,
    #endregion
    #region Environment
    Breeze,
    TreeRustling,
    Cricket,
    Hen,
    DrippingDroplets,
    HeavyWind,
    FallingStones,
    #endregion
    #region General
    Explosion,
    DebrisColliding,
    ShotsOnSurface,
    #endregion
}

//This makes sure that the soundmanager always has an audiosource
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    /*The GetComponent is a Unity function that finds and stores the (audiosource component, in this case)
     attached to the same GameObject. Without this, the script wouldn't know which audiosource to use*/
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /*Takes in a soundtype from the enum, and the volume is set to it's max, which is 1
     Since the method is static and the audiosource isn't, we need to call it using
    instance. This allows us to call SoundManager.PlayPooledSound() from anywhere, without 
    needing to have a reference to a SoundManager component.
    
     PlayOneShot plays an audioclip immediately on top of any other audio that is already playing. This method
    is great to use for SFX, as it doesn't disturb or cuts off other sounds..*/
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
