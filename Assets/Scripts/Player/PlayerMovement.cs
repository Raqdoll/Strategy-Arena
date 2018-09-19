using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement executing and validity checking.
/// </summary>


public class PlayerMovement : MonoBehaviour {

    /// <summary>
    /// Returns all tiles that are in movement range, taking into account blockyblocks etc.
    /// </summary>

    public List<Tile> TilesInRange(Tile startTile, int movementPoints)
    {
        throw new NotImplementedException();
        //for(int i = 1; i <= movementPoints; i++)
        //{
        //    for (int j = 1; j <= movementPoints; j++)
        //    {

        //    }
        //}
    }

    /// <summary>
    /// Returns the shortest route to the destination.
    /// </summary>

    public List<Tile> CalculateRoute(Tile startTile, Tile destinationTile)
    {
        throw new NotImplementedException();
    }

}
