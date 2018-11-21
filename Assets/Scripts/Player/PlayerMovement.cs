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

    public enum MovementMethod { NotSpecified, Teleport, Walk, Push };

    PlayerBehaviour playerController;
    public PlayerInfo playerInfo;
    GridController gridController;
    MouseController mouseController;
    Tile _tile;
    public List<PathTile> pathTiles;

    public Tile CurrentTile
    {
        get
        {
            if (_tile == null)
            {
                //Debug.Log("Haettiin tile jännästi, tee paremmin");
                return gridController.GetTile((int)transform.localPosition.x, (int)transform.localPosition.z);
            }
            else
            {
                return _tile;
            }
        }
        set
        {
            _tile = value;
            AnnounceTileChange(value);
            value.CharCurrentlyOnTile = playerInfo;
        }
    }

    public UnityEvent exampleEvents;
    public delegate void TileEvent(Tile tile);
    public event TileEvent ChangeTile;

    public class PathTile
    {
        public Tile _tile;
        public Tile _destination;
        public List<Tile> _neighbours;
        public int? _distanceToTarget;
        public int _movementPointsLeft;
        public PathTile _previousTile;

        public PathTile(Tile currentTile, Tile destination, int movementPointsLeft, PathTile previousTile)
        {
            _tile = currentTile;
            _destination = destination;
            _neighbours = GetWalkableTiles(_tile.GetTNeighbouringTiles());
            if (destination == null)
                _distanceToTarget = null;
            else
                _distanceToTarget = currentTile.GetCardinalDistance(destination);
            _movementPointsLeft = movementPointsLeft;
            _previousTile = previousTile;
        }

    }

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

        if (tempTile != null)
            MoveToTile(tempTile, MovementMethod.Teleport);
        pathTiles = new List<PathTile>();
    }

    public List<Tile> TilesInRange()
    {

        return TilesInRange(CurrentTile, playerInfo.thisCharacter.currentMp, MovementMethod.Walk);
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

            case MovementMethod.Walk:
                pathTiles = WithinWalkingDistance(startTile, movementPoints);
                foreach (var pathTile in pathTiles)
                {
                    truereturnables.Add(pathTile._tile);
                }
                break;

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
    /// Returns the route by calculating back from destination. Starting tile should have null in variable _previousTile
    /// </summary>

    public static List<Tile> CalculateRouteBack(PathTile destinationTile)
    {
        List<PathTile> route = new List<PathTile>();
        route.Add(destinationTile);
        PathTile tempTile = destinationTile;
        while (tempTile != null)
        {
            route.Add(tempTile);
            tempTile = tempTile._previousTile;
        }
        List<PathTile> orderedRoute = route.OrderByDescending(x => x._movementPointsLeft).ToList();
        List<Tile> wishIHadMeatballs = new List<Tile>();
        foreach (var pathTile in orderedRoute)
        {
            wishIHadMeatballs.Add(pathTile._tile);
        }

        return wishIHadMeatballs;
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

            case MovementMethod.Walk:

                break;

            default:
                Debug.Log("Error with movement method selection!");  //Not yet implemented?
                break;

        }
        CurrentTile = destinationTile;
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

    private List<PathTile> WithinWalkingDistance(Tile startTile, int movementPoints)
    {
        PathTile startPathTile = new PathTile(startTile, null, movementPoints, null);
        List<PathTile> unprocessedTiles = null;
        List<PathTile> processedTiles = new List<PathTile>();
        unprocessedTiles = ProcessPathTile(startPathTile);

        while (unprocessedTiles.Count > 0)
        {
            PathTile tempTile = unprocessedTiles[0];
            List<PathTile> tempList = null;
            if (Upsert(processedTiles, tempTile))
            {
                tempList = ProcessPathTile(tempTile);
            }
            unprocessedTiles.Remove(tempTile);
            if (tempList != null)
                unprocessedTiles.AddRange(tempList);
        }

        return processedTiles;

        //List<Tile> returnables = new List<Tile>();
        //foreach (var pathTile in processedTiles)
        //{
        //    returnables.Add(pathTile._tile);
        //}
        //return returnables;
    }

    /// <summary>
    /// Returns null if out of movement points. Astar might not be implemented at all!
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="dontUseAStar"></param>
    /// <returns></returns>

    private List<PathTile> ProcessPathTile(PathTile startTile, bool dontUseAStar = false)
    {
        List<PathTile> neighbours = null;
        if (startTile._movementPointsLeft > 0)
        {
            neighbours = CreatePathTileNeighbours(startTile);
            if (!dontUseAStar)
            {
                //Do A* stuff
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Returns true if an existing tile was updated or a new one inserted instead of being rejected. Updates only if the new tile has a larger value in movementPointsLeft.
    /// </summary>
    /// <param name="targetList"></param>
    /// <param name="addedTile"></param>
    /// <returns></returns>

    private bool Upsert(List<PathTile> targetList, PathTile addedTile)
    {
        var sameTile = targetList.Where(x => x._tile == addedTile._tile).FirstOrDefault();
        if (sameTile != null)
        {
            if (addedTile._movementPointsLeft > sameTile._movementPointsLeft)
            {
                sameTile._movementPointsLeft = addedTile._movementPointsLeft;
                return true;    //Existing PathTile was updated
            }
            else
            {
                return false;   //Returning false should be captured and tile's neighbours should not be reprocessed!
            }
        }
        targetList.Add(addedTile);  
        return true;    //Added missing PathTile to list
    }


    private List<PathTile> CreatePathTileNeighbours(PathTile currentTile)
    {
        List<PathTile> pathTiles = new List<PathTile>();
        foreach (var tile in currentTile._neighbours)
        {
            PathTile pathTile = new PathTile(tile, currentTile._destination, currentTile._movementPointsLeft - 1, currentTile);
            pathTiles.Add(pathTile);
        }
        return pathTiles;
    }

    void AnnounceTileChange(Tile tile)
    {
        if (ChangeTile != null && tile != null)
        {
            ChangeTile(tile);
        }
    }

    public void ExampleEventsForEditor()
    {
        exampleEvents.Invoke();
    }

    public static List<Tile> GetWalkableTiles(List<Tile> sourceTiles)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var tile in sourceTiles)
        {
            if (tile.WalkThrough && tile.isFree)
            {
                tiles.Add(tile);
            }
        }
        return tiles;
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
            if (GUILayout.Button("Invoke example events"))
            {
                PlayerMovementScript.ExampleEventsForEditor();
            }
        }
    }
#endif

}
