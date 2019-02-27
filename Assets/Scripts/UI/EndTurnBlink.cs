using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnBlink : MonoBehaviour {

    private Image buttonImage;
    public Color normal_color = Color.white;
    public Color highlight_low;
    public Color highlight_high;
    public bool timer = false;
    private float time = 0f;
    [Range(0f, 5f)]
    public float frequency = 1f;

	void Start () {
        buttonImage = GetComponent<Image>();
        buttonImage.color = normal_color;
	}
	
	void Update () {
		
        if(timer == true)
        {
            time += Time.deltaTime;

            if(time < frequency)
            {
                buttonImage.color = highlight_low;
            }
            if((time > frequency) && (time < (frequency * 2)))
            {
                buttonImage.color = highlight_high;
            }
            if(time > (frequency * 2))
            {
                time = 0f;
            }
        }
	}

    public void Blink()
    {
        timer = true;
    }
    public void StopBlinking()
    {
        timer = false;
        time = 0f;
        buttonImage.color = normal_color;
    }

    public bool s1 = false; //Check if spells are castable, blinks end turn if not
    public bool s2 = false;
    public bool s3 = false;
    public bool s4 = false;
    public bool s5 = false;
    public bool s6 = false;

    public void CheckBlink(CharacterValues character)
    {
        if(!s1 && !s2 && !s3 && !s4 && !s5 && !s6 && (character.currentMp <= 0))
        {
            Blink();
        }
        else
        {
            StopBlinking();
        }
    }
}
