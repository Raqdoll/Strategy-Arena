using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public Tile selected; // <- This is hover tile
    List<Tile> tileList;
    public GameObject hitObject;
    SpellCast spellCast;
    Tile tile;
    GridController gridController;
    PlayerBehaviour playerBehaviour;
    public Material hovermaterial;
    public Material targetMaterial;
    public Material rangeMaterial;
    private Tile previousTile;
    List<Tile> targetedTiles;
    List<Tile> rangeTiles;
    Abilities abilities;

    void Start()
    {
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        playerBehaviour = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerBehaviour>();
        spellCast = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<SpellCast>();
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
                        if(rangeTiles != null)
                        {
                            foreach (var tile in rangeTiles)
                            {
                                Renderer tr = previousTile.GetComponent<Renderer>();
                                tr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                            }
                            rangeTiles = null;
                        }
                        if(targetedTiles != null)
                        {
                            foreach (var target in targetedTiles)
                            {
                                Renderer ar = previousTile.GetComponent<Renderer>();
                                ar.material = previousTile.GetComponent<Tile>().BaseMaterial;
                            }
                            targetedTiles = null;
                        }
                    }

                    if (spellCast.spellOpen == true)
                    {
                        Debug.Log("Spelll Open");
                            if (Input.GetMouseButtonDown(0) && playerBehaviour.currentCharacter.currentAp >= spellCast.currentSpell.spellApCost /*abilities.spellApCost*/)
                            {
                                foreach (var tile in targetedTiles)
                                {

                                }
                            }
                        rangeTiles = abilities.RangeType();
                        foreach (var tile in rangeTiles)
                        {
                            previousTile = selected;
                            Renderer aR = selected.GetComponent<Renderer>();
                            aR.material = rangeMaterial;
                            if (tile == selected)
                            {
                                targetedTiles = abilities.AreaType();
                                foreach (var target in targetedTiles)
                                {
                                    Renderer sr = selected.GetComponent<Renderer>();
                                    sr.material = targetMaterial;
                                }
                            }
                            else
                            { 
                                Renderer sr = selected.GetComponent<Renderer>();
                                sr.material = hovermaterial;
                            }
                        }
                        if (Input.GetMouseButtonDown(0) && playerBehaviour.currentCharacter.currentAp >= spellCast.currentSpell.spellApCost && spellCast.currentSpell != null)
                        {
                            foreach (var tile in targetedTiles)
                            {
                                
                                CharacterValues target = tile.charCurrentlyOnTile;
                                
                                spellCast.CastSpell(spellCast.currentSpell, playerBehaviour.currentCharacter, target);

                            }
                            if (rangeTiles != null)
                            {
                                foreach (var tile in rangeTiles)
                                {
                                    Renderer tr = previousTile.GetComponent<Renderer>();
                                    tr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                                }
                                rangeTiles = null;
                            }
                            if (targetedTiles != null)
                            {
                                foreach (var target in targetedTiles)
                                {
                                    Renderer ar = previousTile.GetComponent<Renderer>();
                                    ar.material = previousTile.GetComponent<Tile>().BaseMaterial;
                                }
                                targetedTiles = null;
                            }
                            spellCast.Aftermath();
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            if (rangeTiles != null)
                            {
                                foreach (var tile in rangeTiles)
                                {
                                    Renderer tr = previousTile.GetComponent<Renderer>();
                                    tr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                                }
                                rangeTiles = null;
                            }
                            if (targetedTiles != null)
                            {
                                foreach (var target in targetedTiles)
                                {
                                    Renderer ar = previousTile.GetComponent<Renderer>();
                                    ar.material = previousTile.GetComponent<Tile>().BaseMaterial;
                                }
                                targetedTiles = null;
                            }
                            spellCast.SpellCancel();
                        }
                    }
                    else
                    {
                        previousTile = selected;
                        Renderer sr = selected.GetComponent<Renderer>();
                        sr.material = hovermaterial;
                    }
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
