using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    public GridController gridC;

public bool LoSCheck(Tile startpos, Tile target)
    {
        //checking linears
        if (startpos.locX == target.locX)
        {
            for(int i = startpos.locZ; i < target.locZ; i++)
            {
                if (gridC.GetTile(target.locX, i).ShootThrough == false)
                {
                    return false;
                }
            }
        }
        else
        {
            return true;
        }
        if (startpos.locZ == target.locZ)
        {
            for (int i = startpos.locX; i < target.locX; i++)
            {
                if (gridC.GetTile(target.locZ, i).ShootThrough == false)
                {
                    return false;
                }
            }
        }
        else
        {
            return true;
        }
        return false;
    }

    
}
