using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour {

    public GameObject tiles;
    private Tile[] tileList;
    private List<List<Tile>> tileGrid;


    // Use this for initialization
    void Start()
    {
        tileList = tiles.GetComponentsInChildren<Tile>();
        tileGrid = new List<List<Tile>>(); // Initialize overlist
        for (int j = 0; j < 24; j++)
        { // Initialize each list
            List<Tile> tempList = new List<Tile>();
            for (int z = 0; z < 24; z++)
            {
                tempList.Add(null); // Initialize each object
            }
            tileGrid.Add(tempList);
        }
        Debug.Log(tileGrid.Count + "  " + tileGrid[0].Count);
        for (int i = 0; i < tileList.Length; i++)
        {
            Debug.Log(tileList[i].locX + "  " + tileList[i].locZ);
            tileGrid[tileList[i].locX - 1][tileList[i].locZ - 1] = tileList[i];
        }

        GetTile(3, 4).transform.position += new Vector3(0, 2f, 0);

    }
    //USE THIS TO CALL GRID!
    public Tile GetTile(int xCord, int zCord) {
        return tileGrid[xCord - 1][zCord - 1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
