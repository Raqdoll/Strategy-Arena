using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioSource mainMusicControl;
    public AudioSource temp;
    public AudioSource temp2;
    public AudioClip mainMusic;

	void Start () {

        if (mainMusicControl && mainMusic)
        {
            mainMusicControl.loop = true;
            mainMusicControl.clip = mainMusic;
            mainMusicControl.Play();
        }
	}

    public void PlaySpell(SpellValues spell, AudioSource audio)
    {
        audio.loop = false;

        audio.clip = spell.spellSound;
        audio.Play();
    }
    
    public void PlayMovementStartLoop(CharacterValues character, AudioSource audio)
    {
        audio.loop = true;

        audio.clip = character.walkSoundLoop;
        audio.Play();
    }
    public void PlayMovementStopLoop(AudioSource audio)
    {
        audio.Stop();
    }

    public void PlayAttack(CharacterValues character, AudioSource audio)
    {
        audio.loop = false;

        audio.clip = character.attackSound;
        audio.Play();
    }

    public void PlayDamageTaken(CharacterValues character, AudioSource audio)
    {
        audio.loop = false;

        audio.clip = character.takeDamageSound;
        audio.Play();
    }

	
}
