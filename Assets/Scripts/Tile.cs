using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Abilities))]

public class Tile : MonoBehaviour {
    public GameObject hoveryTile;
    public GameObject targetyTile;
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
    List<Tile> tileList;
    Abilities abilities;

    public bool isFree = true;
    public bool ShootThrough;
    public bool Targetable;
    public bool WalkThrough;
    public enum BlockType { BaseBlock, ShootThroughBlock, BlockyBlock, StartA, StartB};
    public BlockType myType;

	// Use this for initialization
	void Start () {
        gridController = GetComponent<GridController>();
        locX = (int)transform.localPosition.x;
        locZ = (int)transform.localPosition.z;
        thisMaterial = GetComponent<Renderer>().material;
        BaseMaterial = GetComponent<Renderer>().material;

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
        if (myType == BlockType.BaseBlock/* && gridController != null*/)
        {
            //Instantiate(hoveryTile ,new Vector3(this.locX, 0f, this.locZ),Quaternion.identity);
           // GridController.FindObjectOfType<GridController>().hoverTile = this;

            if (Abilities.spellOpen == true)
            {
                //gridController.hoverTile = gridController.GetTile(this.locX, this.locZ);
                //gridController.hoverTile.locZ = (int)transform.localPosition.x;
                //gridController.hoverTile.locX = (int)transform.localPosition.z;
                //Debug.Log(gridController.hoverTile.locX);
                //Debug.Log(gridController.hoverTile.locZ);
                abilities.AreaType(); //updateen
                tileList = abilities.AreaType();
                foreach (var tile in tileList)
                {
                    GetComponent<MeshRenderer>().material = TargetMaterial;
                }
            }
            else
            {
                GetComponent<MeshRenderer>().material = GridHoverMaterial;
            }

        }
    }

    void OnMouseExit()
    {
        if (myType == BlockType.BaseBlock)
        {
            
            GetComponent<MeshRenderer>().material = BaseMaterial;
            //tileList.Clear();
            //gridController.hoverTile = null;
        }
    }
}
