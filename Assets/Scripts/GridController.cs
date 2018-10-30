using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GridController : MonoBehaviour {

    public GameObject tiles;
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

        //Tile test = GetTile(4, 6);
        //test.transform.position += new Vector3(2f, 2f, 2f);
    }
    //USE THIS TO CALL GRID!
    public Tile GetTile(int xCord, int zCord) {
        if (xCord > 24 || zCord > 24 || zCord < 1 || xCord < 1)
            return null;
        else
        return tileGrid[xCord - 1][zCord - 1];
    }

    public Tile GetTile(PositionContainer container)
    {
        if (container != null)
            return GetTile(container.x, container.z);
        else
        {
            Debug.Log("container is null!");
            return null;
        }
    }

    public List<Tile> GetTilesNextTo(int xCord, int zCord)
    {
        List<Tile> palautus = new List<Tile>();
        Tile tempTile;
        tempTile = GetTile(xCord + 1, zCord);
        if (tempTile != null) palautus.Add(tempTile);
        tempTile = GetTile(xCord - 1, zCord);
        if (tempTile != null) palautus.Add(tempTile);
        tempTile = GetTile(xCord, zCord + 1);
        if (tempTile != null) palautus.Add(tempTile);
        tempTile = GetTile(xCord, zCord - 1);
        if (tempTile != null) palautus.Add(tempTile);
        return palautus;
    }

	
	void Update () {
		
	}
}
