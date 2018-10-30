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

    PlayerBehaviour playerController;
    PlayerInfo playerInfo;
    GridController gridController;
    MouseController mouseController;
    public Tile targetTile;
    public UnityEvent exampleEvents;

    //struct PathTile {
    //    Tile _tile;
    //    public List<Tile> neighbourTIles;
    //    public int distanceToTarget, movementPointsUsed;

    //    public PathTile(Tile tile, List<Tile> neighbours)
    //    {
    //        _tile = tile;
    //    }
    //}

    private void Start()
    {
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        mouseController = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseController>();
        if (!mouseController)
            Debug.LogWarning("Mousecontroller is null!");
        mouseController.currentMovement = this;
        if (!playerController)
            playerController = gameObject.GetComponentInParent<PlayerBehaviour>();
        if (!playerController)
            Debug.Log("Could not find playerbehaviour component in parents!");
        if (!playerInfo)
            playerInfo = gameObject.GetComponent<PlayerInfo>();
        if (!playerInfo)
            Debug.Log("Could not find playerinfo component");

        PositionContainer debuggerPositionContainer = new PositionContainer(7, 7);  //Remove this when starting positions are set properly!
        Tile tempTile = gridController.GetTile(debuggerPositionContainer);

        //playerInfo.thisCharacter.currentTile = new PositionContainer(7, 7);
        //Tile tempTile = gridController.GetTile(playerInfo.thisCharacter.currentTile.x, playerInfo.thisCharacter.currentTile.z);  //CurrentTile is null!
        if (tempTile != null)
            MoveToTile(tempTile, MovementMethod.Teleport);
    }

    private void Update()
    {
        
    }

    public List<Tile> TilesInRange()
    {
        //PositionContainer container = playerInfo.thisCharacter.currentTile;
        //Tile tile = gridController.GetTile(container.x, container.z);
        Tile tile = gridController.GetTile((int)transform.localPosition.x, (int)transform.localPosition.z);
        //Debug.Log("Getting tiles... " + tile + " and " + playerInfo.thisCharacter.currentMp + " and " + MovementMethod.Teleport);

        return TilesInRange(tile, playerInfo.thisCharacter.currentMp, MovementMethod.Teleport);
    }

    /// <summary>
    /// Returns all tiles that are in movement range, taking into account blockyblocks etc.
    /// </summary>
    //KESKEN
    public List<Tile> TilesInRange(Tile startTile, int movementPoints, MovementMethod method)
    {
        List<Tile> returnables = new List<Tile>();
        List<Tile> truereturnables = new List<Tile>();
        int movementLeft = movementPoints;

        switch (method)
        {
            case MovementMethod.Teleport:
                List<Tile> lastIteration = new List<Tile>();
                lastIteration.Add(startTile);
                while (movementLeft > 0)
                {
                    List<Tile> tempList = new List<Tile>();
                    foreach (var tile1 in lastIteration)
                    {
                        if (tile1 != null)
                        {
                            tempList = tempList.Union(tile1.GetTNeighbouringTiles()).ToList();
                            //Debug.Log("Returnables count: " + returnables.Count());
                        }
                    }
                    returnables = returnables.Union(tempList).ToList();
                    lastIteration = tempList;
                    movementLeft--;
                }
                break;

            //case MovementMethod.walk:
            //    List<Tile> unProcessed = new List<Tile>();

            //    break;

            default:
                break;

        }
        foreach (var tile in returnables)
        {
            if (tile != null)
            {
                if (tile.myType == Tile.BlockType.BaseBlock)
                    truereturnables.Add(tile);
            }         
        }
        return truereturnables;
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
        playerController.currentCharacter.currentTile = new PositionContainer(destinationTile.transform.localPosition);
    }

    public void MyTurn()
    {
        mouseController.currentMovement = this;
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
