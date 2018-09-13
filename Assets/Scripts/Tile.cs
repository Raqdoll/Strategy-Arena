using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public int locX;
    public int locZ;

    public Material ShootThroughBlockMaterial;
    public Material BlockyBlockMaterial;
    private Material thisMaterial;

    public bool isFree = true;
    private bool ShootThrough;
    private bool Targetable;
    private bool WalkThrough;
    public enum BlockType { BaseBlock, ShootThroughBlock, BlockyBlock, StartA, StartB};
    public BlockType myType;

	// Use this for initialization
	void Start () {

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
                //thisMaterial.color =  ShootThroughBlockMaterial.color;
                GetComponent<Renderer>().gameObject.SetActive(false);
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

}
