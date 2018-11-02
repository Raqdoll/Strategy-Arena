using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public SpellValues spell;
    public CharacterValues character;
    public EffectValues effect;
    UImanager ui;
    Text _text;

    void Start () {
        if (!ui)
            ui = GameObject.FindGameObjectWithTag("UIcanvas").GetComponent<UImanager>();
        _text.text = "";
    }

    public void UpdateInfo(SpellValues x)
    {
        spell = x;
    }
    public void UpdateInfo(CharacterValues x)
    {
        character = x;
    }
    public void UpdateInfo(EffectValues x)
    {
        effect = x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (spell && !character && !effect)
        {
            ui._text = "This is a spot for spells";
            ui.ShowTooltip();
        }
        else if (!spell && character && !effect)
        {
            ui._text = "This is a spot for characters";
            ui.ShowTooltip();
        }
        else if (!spell && !character && effect)
        {
            ui._text = "This is a spot for effects";
            ui.ShowTooltip();
        }
        else
        {
            Debug.Log("error");
        }
        ui.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.HideTooltip();
        
    }
}
