using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioSource mainMusicControl;
    public AudioClip mainMusic;

	void Start () {

        if (mainMusicControl && mainMusic)
        {
            mainMusicControl.loop = true;
            mainMusicControl.clip = mainMusic;
            mainMusicControl.Play();
        }
	}

    public void PlaySpell(AudioSource audio, SpellValues spell)
    {
        audio.loop = false;
        
        audio.clip = spell.spellSound;
        audio.Play();
    }
    
    public void PlayMovementStartLoop(AudioSource audio, CharacterValues character)
    {
        audio.loop = true;
       
        audio.clip = character.walkSoundLoop;
        audio.Play();
    }
    public void PlayMovementStopLoop(AudioSource audio)
    {
        audio.Stop();
    }

    public void PlayAttack(AudioSource audio, CharacterValues character)
    {
        audio.loop = false;

        audio.clip = character.attackSound;
        audio.Play();
    }

    public void PlayDamageTaken(AudioSource audio, CharacterValues character)
    {
        audio.loop = false;

        audio.clip = character.takeDamageSound;
        audio.Play();
    }

	
}
