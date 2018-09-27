using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public Tile selected; // <- This is hover tile
    List<Tile> tileList;
    public GameObject hitObject;
    Abilities abilities;
    Tile tile;
    GridController gridController;
    PlayerBehaviour playerBehaviour;
    public Material hovermaterial;
    private Tile previousTile;

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
                //if (selected)
                //    selected.GetComponent<Renderer>().material = selected.BaseMaterial;
                selected = hitObject.gameObject.GetComponent<Tile>();
                
                if (selected.myType == Tile.BlockType.BaseBlock && selected != previousTile)
                {
                    Debug.Log("Tämä on baseblock");
                    if (previousTile)
                    {
                        Renderer pr = previousTile.GetComponent<Renderer>();
                        pr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                        previousTile = null;
                    }
                        
                    previousTile = selected;
                    Renderer sr = selected.GetComponent<Renderer>();
                    sr.material = hovermaterial;

                }
                
            }
            //Jos hovertile on jotain muuta, resettaa hovertilet
            if(!hitObject.CompareTag("Tile") || (hitObject.CompareTag("Tile") && selected.myType != Tile.BlockType.BaseBlock))
            {
                if (previousTile)
                {
                    Renderer pr = previousTile.GetComponent<Renderer>();
                    pr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                    previousTile = null;
                }
            }
        }

    }
}
