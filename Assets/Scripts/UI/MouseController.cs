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
    public Material movementRangeMaterial;
    private Tile previousTile;
    List<Tile> targetedTiles;
    List<Tile> rangeTiles;
    List<Tile> tilesToBeReset;
    Abilities abilities;
    public PlayerMovement currentMovement;
    private bool rangeTilesPainted;

    void Start()
    {
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        playerBehaviour = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerBehaviour>();
        spellCast = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<SpellCast>();
        abilities = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Abilities>();
        tilesToBeReset = new List<Tile>();
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
                   // Debug.Log("Tämä on baseblock");


                    if (previousTile)
                    {
                        Renderer pr = previousTile.GetComponent<Renderer>();
                        pr.material = previousTile.GetComponent<Tile>().BaseMaterial;
                        previousTile = null;
                        //if(rangeTiles != null)
                        //{
                        //    ResetTileMaterials(rangeTiles);
                        //    rangeTiles = null;
                        //}
                        if(targetedTiles != null)
                        {
                            ResetTileMaterials(targetedTiles);
                            targetedTiles = null;
                            ChangeTileMaterials(rangeTiles, rangeMaterial);
                        }
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

                    if (targetedTiles != null)
                    {
                        ResetTileMaterials(targetedTiles);
                        targetedTiles = null;
                        ChangeTileMaterials(rangeTiles, rangeMaterial);
                    }
                }
            }
        }

        // kun spell nappulaa on painettu
        if (spellCast.spellOpen == true)
        {
            //pitäisi maalata target range
            if (rangeTiles == null)
            {
                rangeTiles = abilities.RangeType(spellCast.currentSpell.mySpellRangeType);
                foreach (var tile in rangeTiles)
                {
                    Renderer aR = tile.GetComponent<Renderer>();
                    aR.material = rangeMaterial;
                } 
            }

            //pitäisi maalata AOE tilet
            targetedTiles = abilities.AreaType(spellCast.currentSpell.mySpellAreaType);
            foreach (var tile in rangeTiles)
            {
                if (tile == selected)
                {
                    ChangeTileMaterials(targetedTiles, targetMaterial);
                }
            }

            // kun spell castataan
            if (Input.GetMouseButtonDown(0) && playerBehaviour.currentCharacter.currentAp >= spellCast.currentSpell.spellApCost && spellCast.currentSpell != null)
            {
                if (targetedTiles != null)
                {
                    foreach (var tile in targetedTiles)
                    {
                        CharacterValues target = tile.charCurrentlyOnTile;
                        spellCast.CastSpell(spellCast.currentSpell, playerBehaviour.currentCharacter, target);
                    }
                    foreach (var tile in rangeTiles)
                        {
                            Renderer tr = tile.GetComponent<Renderer>();
                            tr.material = tile.GetComponent<Tile>().BaseMaterial;
                        }
                    rangeTiles = null;
                    foreach (var target in targetedTiles)
                        {
                            Renderer ar = target.GetComponent<Renderer>();
                            ar.material = target.GetComponent<Tile>().BaseMaterial;
                        }
                    targetedTiles = null; 
                    spellCast.Aftermath();
                }
                else
                {
                    spellCast.SpellCancel();
                }
            }

            // spell cansellataan
            if (Input.GetMouseButtonDown(1))
            {
                if (rangeTiles != null)
                {
                    ResetTileMaterials(rangeTiles);
                    rangeTiles = null;
                }
                if (targetedTiles != null)
                {
                    ResetTileMaterials(targetedTiles);
                    targetedTiles = null;
                }
                spellCast.SpellCancel();
            }
        }

        if (spellCast.spellOpen == false)
        {
            if (!currentMovement)
            {
                Debug.Log("Movement still not set!");
            }

            if (!rangeTilesPainted)
            {
                ResetTileMaterials(tilesToBeReset);
                tilesToBeReset.Clear();
                ChangeTileMaterials(currentMovement.TilesInRange(), movementRangeMaterial);
                rangeTilesPainted = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentMovement.MoveToTile(selected, PlayerMovement.MovementMethod.Teleport);

                ResetTileMaterials(tilesToBeReset);
                tilesToBeReset.Clear();
                rangeTilesPainted = false;
            }
        }
    }

    void ResetTileMaterials(List<Tile> tileList)
    {
        foreach(var tile in tileList)
        {
            Renderer ar = tile.GetComponent<Renderer>();
            ar.material = tile.GetComponent<Tile>().BaseMaterial;
        }
    }

    void ChangeTileMaterials(List<Tile> tileList, Material material)
    {
        foreach (var tile in tileList)
        {
            Renderer pr = tile.GetComponent<Renderer>();
            pr.material = material;
        }
    }

}
