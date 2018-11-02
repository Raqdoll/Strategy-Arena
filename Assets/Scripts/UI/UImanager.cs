using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {

    public GameObject tooltip;
    public Text tooltipText;
    public string _text;

	void Start () {
        HideTooltip();
    }

    public void ShowTooltip()
    {
        tooltipText.text = _text;
        tooltip.SetActive(true);
    }
    public void HideTooltip()
    {
        tooltipText.text = "";
        tooltip.SetActive(false);
    }
	
}
