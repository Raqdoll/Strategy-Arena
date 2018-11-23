using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {

    public GameObject tooltip;
    public Text tooltipText;
    public Button menuButton;
    public Button returnButton;
    public GameObject menu;
    public string _text;

	void Start () {
        HideTooltip();
        menuButton = menuButton.GetComponent<Button>();
        returnButton = returnButton.GetComponent<Button>();
        CloseMenu();
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
    public void OpenMenu()
    {
        menu.gameObject.SetActive(true);
        
        returnButton.onClick.AddListener(CloseMenu);
        menuButton.onClick.AddListener(CloseMenu);
    }
    public void CloseMenu()
    {
        menu.gameObject.SetActive(false);
        menuButton.onClick.AddListener(OpenMenu);
    }
	
}
