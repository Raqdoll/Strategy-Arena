using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineArrowMover : MonoBehaviour {

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject arrow4;
    public GameObject arrow5;
    public GameObject arrow6;
    public GameObject arrow7;
    public GameObject arrow8;
    public GameObject arrow9;
    public GameObject arrow10;

    void Start () {

        //ResetArrows();
        MoveArrow(1);
    }
    // call this with player number to move arrow to it's position in the timeline
    void MoveArrow(int currentPlayerNr)
    {
        ResetArrows();
        if (currentPlayerNr <= 10 && currentPlayerNr > 0)
        {
            if (currentPlayerNr == 1)
            {
                arrow1.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 2)
            {
                arrow2.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 3)
            {
                arrow3.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 4)
            {
                arrow4.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 5)
            {
                arrow5.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 6)
            {
                arrow6.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 7)
            {
                arrow7.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 8)
            {
                arrow8.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 9)
            {
                arrow9.gameObject.SetActive(true);
            }
            if (currentPlayerNr == 10)
            {
                arrow10.gameObject.SetActive(true);
            }
        }     
    }
    public void ResetArrows()
    {
        arrow1.gameObject.SetActive(false);
        arrow2.gameObject.SetActive(false);
        arrow3.gameObject.SetActive(false);
        arrow4.gameObject.SetActive(false);
        arrow5.gameObject.SetActive(false);
        arrow6.gameObject.SetActive(false);
        arrow7.gameObject.SetActive(false);
        arrow8.gameObject.SetActive(false);
        arrow9.gameObject.SetActive(false);
        arrow10.gameObject.SetActive(false);
    }
}
