using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    public GridController gridC;

public bool LoSCheck(Tile startpos, Tile target)
    {
        ////checking linears
        //if (startpos.locX == target.locX)
        //{
        //    for(int i = startpos.locZ; i < target.locZ; i++)
        //    {
        //        if (gridC.GetTile(target.locX, i).ShootThrough == false)
        //        {
        //            return false;
        //        }    
        //    }
        //    return true;
        //}

        //if (startpos.locZ == target.locZ)
        //{
        //    for (int i = startpos.locX; i < target.locX; i++)
        //    {
        //        if (gridC.GetTile(target.locZ, i).ShootThrough == false)
        //        {
        //            return false;
        //        }
        //    }               
        //        return true;
        //}
        ////checking diagonals
        //if(target.locX - startpos.locX == target.locZ - startpos.locZ || target.locX - startpos.locX == (target.locZ - startpos.locZ)*(-1))
        //{
        //    for (int i = 1; i <= Mathf.Abs(target.locX - startpos.locX); i++)
        //    {
        //        if (target.locX > startpos.locX && target.locZ > startpos.locZ) {
        //            if (gridC.GetTile(startpos.locX + i, startpos.locZ + i).ShootThrough == false)
        //            {
        //                return false;
        //            }
        //        }
        //        if (target.locX < startpos.locX && target.locZ > startpos.locZ)
        //        {
        //            if (gridC.GetTile(startpos.locX + i, startpos.locZ - i).ShootThrough == false)
        //            {
        //                return false;
        //            }
        //        }
        //        if (target.locX > startpos.locX && target.locZ < startpos.locZ)
        //        {
        //            if (gridC.GetTile(startpos.locX - i, startpos.locZ + i).ShootThrough == false)
        //            {
        //                return false;
        //            }
        //        }
        //        if (target.locX < startpos.locX && target.locZ < startpos.locZ)
        //        {
        //            if (gridC.GetTile(startpos.locX - i, startpos.locZ - i).ShootThrough == false)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        
        for (int i = 1; i <= 200; i++)
        {
            if(gridC.GetTile(Mathf.RoundToInt((startpos.locX+target.locX)/200 * i)+startpos.locX, Mathf.RoundToInt((startpos.locX + target.locX) / 200 * i)+startpos.locZ).ShootThrough == false && gridC.GetTile(Mathf.RoundToInt((startpos.locX + target.locX) / 200 * i)+startpos.locX, Mathf.RoundToInt((startpos.locX + target.locX) / 200 * i)+startpos.locZ) != startpos)
            {
                return false;
            }

        }
        return true;















        return false;
    }

    
}
