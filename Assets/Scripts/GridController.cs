using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

    public GameObject tiles;
    public Tile[] tileList;

	// Use this for initialization
	void Start () {
        tileList = tiles.GetComponentsInChildren<Tile>();

        //for(int i = 0; i < 24; i++)
        //{
        //    for (int j = 0; j < 24; j++)
        //    {
        //        int count = i * 24 + j; /*BaseBlock, ShootThroughBlock, BlockyBlock, StartA, StartB*/
        //        switch (tileList[count].myType)
        //        {
        //            case :
        //                break;
        //        }
        //    }
        //}





	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
