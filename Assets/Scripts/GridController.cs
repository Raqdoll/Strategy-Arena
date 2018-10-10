using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour {

    public GameObject tiles;
    public Tile hoverTile;
    public Tile playerTile;
    private Tile[] tileList;
    private List<List<Tile>> tileGrid;


    // Use this for initialization
    void Start()
    {
        playerTile = GetTile(5, 5);

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
        for (int i = 0; i < tileList.Length; i++)
        {
            tileGrid[tileList[i].locX - 1][tileList[i].locZ - 1] = tileList[i];
        }
    }
    //USE THIS TO CALL GRID!
    public Tile GetTile(int xCord, int zCord) {
        return tileGrid[xCord - 1][zCord - 1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //public void setHovertile()
    //{
    //    hoverTile = x;
    //}
}
