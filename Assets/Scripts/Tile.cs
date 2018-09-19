using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public int locX;
    public int locZ;

    public Material ShootThroughBlockMaterial;
    public Material BlockyBlockMaterial;
    private Material thisMaterial;
    public Material GridHoverMaterial;
    public Material BaseMaterial;
    public Material TargetMaterial;
    public Material RangeMaterial;
    public Material MovementMaterial;
    GridController gridController;
    Abilities testAbility;

    public bool isFree = true;
    public bool ShootThrough;
    public bool Targetable;
    public bool WalkThrough;
    public enum BlockType { BaseBlock, ShootThroughBlock, BlockyBlock, StartA, StartB};
    public BlockType myType;

	// Use this for initialization
	void Start () {
        testAbility = GameController.activePlayer.GetComponent<Abilities>();
        gridController = GetComponent<GridController>();
        locX = (int)transform.localPosition.x;
        locZ = (int)transform.localPosition.z;
        thisMaterial = GetComponent<Renderer>().material;

        switch (myType)
        {
            case BlockType.BaseBlock:
                ShootThrough = true;
                Targetable = false;
                WalkThrough = true;
            break;

            case BlockType.ShootThroughBlock:
                ShootThrough = true;
                Targetable = false;
                WalkThrough = false;
                thisMaterial.color =  ShootThroughBlockMaterial.color;
                //GetComponent<Renderer>().gameObject.SetActive(false);
            break;

            case BlockType.BlockyBlock:
                ShootThrough = false;
                Targetable = false;
                WalkThrough = false;
                thisMaterial.color = BlockyBlockMaterial.color;
                transform.localScale += new Vector3(0.1f, 0.55f, 0.1f);
                transform.position += new Vector3(0, 0.3f, 0);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseOver()
    {
        if (myType == BlockType.BaseBlock)
        {
            if (Abilities.spellOpen == true)
            {
                List<Tile> tiles = Abilities.Area();
                foreach (var tile in tiles)
                {
                    tile.thisMaterial = 
                    thisMaterial.color = .color;
                }
            }
            else
            {
                thisMaterial.color = GridHoverMaterial.color;
            }
            gridController.hoverTile = this;
        }
    }

    void OnMouseExit()
    {
        if (myType == BlockType.BaseBlock)
        {
            thisMaterial.color = BaseMaterial.color;
            Abilities.targetTiles.Clear();
            gridController.hoverTile = null;
        }
    }
}
