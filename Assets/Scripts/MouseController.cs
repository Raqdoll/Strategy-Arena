using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public Tile selected;
    List<Tile> tileList;
    public GameObject hitObject;
    Abilities abilities;
    Tile tile;
    GridController gridController;
    PlayerBehaviour playerBehaviour;

    void Start()
    {
    }


    void Update()
    {

        //Mouse/ray setup

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))
        {
            hitObject = hitInfo.transform.gameObject;

            if (hitObject.CompareTag("Tile"))
            {
                selected = hitObject.gameObject.GetComponent<Tile>();
                if (selected.myType == Tile.BlockType.BaseBlock/* && gridController != null*/)
                {
                    //Instantiate(hoveryTile ,new Vector3(this.locX, 0f, this.locZ),Quaternion.identity);
                    // GridController.FindObjectOfType<GridController>().hoverTile = this;
                    Debug.Log("Tämä on baseblock");
                    if (playerBehaviour.spellOpen == true)
                    {

                        gridController.hoverTile = gridController.GetTile(selected.locX, selected.locZ);
                        //gridController.hoverTile.locZ = (int)transform.localPosition.x;
                        //gridController.hoverTile.locX = (int)transform.localPosition.z;
                        Debug.Log(gridController.hoverTile.locX);
                        Debug.Log(gridController.hoverTile.locZ);
                        //abilities.AreaType();
                        tileList = abilities.AreaType();
                        foreach (var tile in tileList)
                        {
                            GetComponent<MeshRenderer>().material = tile.TargetMaterial;
                        }
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material = tile.GridHoverMaterial;
                    }
                }
            }
        }

    }
}
