using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

    public enum Map { baseMap, one, two};
    public Map currentMap;

    // Use this for initialization
    void Start()
    {
        switch (currentMap)
        {
            case Map.baseMap:
                break;
            case Map.one:
                break;
            case Map.two:
                break;
            default:
                break;
        }





    }
	// Update is called once per frame
	void Update () {
		
	}
}
