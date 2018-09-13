using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>

    [RequireComponent(typeof(PlayerMovement), typeof(PlayerActions), typeof(Abilities))]
public class PlayerBehaviour : MonoBehaviour {

    public enum CharacterClass { Tank1, Tank2 };
    public bool moving;  //e.g. Hover to get movement range vs ability range

    // Use this for initialization
    void Start () {
		//initalize abilitylist in abilities script
	}
	



	// Update is called once per frame
	void Update () {
		
	}
}
