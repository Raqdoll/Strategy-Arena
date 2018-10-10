using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the movement executing and validity checking. Movement happens only in cardinal directions.
/// </summary>


public class PlayerMovement : MonoBehaviour {

    public enum MovementMethod { NotSpecified, Teleport, walk, push };  //Pidetään notspecified nollassa -> tällöin kutsu on tehty huolimattomasti eli liikkumismuotoa ei olla valittu

    PlayerBehaviour behaviour;
    GridController gridController;
    public Tile targetTile;
    public UnityEvent exampleEvents;

    private void Start()
    {
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        if (!behaviour)
            behaviour = gameObject.GetComponent<PlayerBehaviour>();
        if (!behaviour)
            Debug.Log("Player " + gameObject.name + " does not have playerbehaviour component!");

    }

    /// <summary>
    /// Returns all tiles that are in movement range, taking into account blockyblocks etc.
    /// </summary>
    //KESKEN
    public List<Tile> TilesInRange(Tile startTile, int movementPoints)
    {
        int movementLeft = movementPoints;
        List<Tile> palautus = new List<Tile>();
        List<Tile> lastIteration = new List<Tile>();
        lastIteration.Add(startTile);
        while (movementLeft > 0)
        {
            List<Tile> tempList = new List<Tile>();
            foreach(var tile1 in lastIteration)
            {
                tempList.Union(tile1.GetTNeighbouringTiles());
            }

            //palautus = palautus.Union(lastIteration);

            //foreach (var tile2 in tempList)
            //{
            //    if (tile2.WalkThrough)
            //    {
            //        (tile2);
            //    }
            //}


            movementLeft--;
        }


        throw new NotImplementedException();
        //for(int i = 1; i <= movementPoints; i++)
        //{
        //    for (int j = 1; j <= movementPoints; j++)
        //    {

        //    }
        //}
    }

    /// <summary>
    /// Returns the shortest route to the destination.
    /// </summary>

    public List<Tile> CalculateRoute(Tile startTile, Tile destinationTile)
    {
        throw new NotImplementedException();
    }

    public void TestMovement()
    {
        MoveToTile(targetTile, MovementMethod.Teleport);
    }

    public void MoveToTile(Tile destinationTile, MovementMethod method)
    {
        switch (method)
        {
            case MovementMethod.NotSpecified:
                Debug.Log("Movement method not selected!");
                break;

            case MovementMethod.Teleport:
                Teleport(destinationTile);
                break;

            default:
                Debug.Log("Error with movement method selection!");  //Not yet implemented?
                break;

        }
    }

    void Teleport(Tile destinationTile)
    {
        transform.localPosition = destinationTile.transform.localPosition;
        behaviour.currentTile = destinationTile;
    }

    public void ExampleEventsForEditor()
    {
        exampleEvents.Invoke();
    }


#if UNITY_EDITOR
    //using UnityEditor;

    [UnityEditor.CustomEditor(typeof(PlayerMovement))]
    public class MovementButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlayerMovement PlayerMovementScript = (PlayerMovement)target;
            if (GUILayout.Button("Test movement to target tile"))
            {
                PlayerMovementScript.TestMovement();
            }
            if (GUILayout.Button("Invoke example events"))
            {
                PlayerMovementScript.ExampleEventsForEditor();
            }
        }
    }
#endif

}
