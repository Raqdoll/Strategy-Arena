using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour {

    public GameObject tiles;
    public Tile hoverTile;
    private Tile[] tileList;
    private List<List<Tile>> tileGrid;

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
        for (int i = 0; i < tileList.Length; i++)
        {
            tileGrid[tileList[i].locX - 1][tileList[i].locZ - 1] = tileList[i];
        }
    }
    //USE THIS TO CALL GRID!
    public Tile GetTile(int xCord, int zCord) {
        return tileGrid[xCord - 1][zCord - 1];
    }

    public List<Tile> GetTilesNextTo(int xCord, int zCord)
    {
        List<Tile> palautus = new List<Tile>();
        palautus.Add(GetTile(xCord + 1, zCord));
        palautus.Add(GetTile(xCord - 1, zCord));
        palautus.Add(GetTile(xCord, zCord + 1));
        palautus.Add(GetTile(xCord, zCord - 1));
        return palautus;

    }

	
	void Update () {
		
	}
}
