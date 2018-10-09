using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the movement executing and validity checking.
/// </summary>


public class PlayerMovement : MonoBehaviour {

    public enum MovementMethod { NotSpecified, Teleport, walk, push };  //Pidetään notspecified nollassa -> tällöin kutsu on tehty huolimattomasti eli liikkumismuotoa ei olla valittu

    PlayerBehaviour _behaviour;
    public Tile targetTile;
    public UnityEvent exampleEvents;

    private void Start()
    {
        if (!_behaviour)
            _behaviour = gameObject.GetComponent<PlayerBehaviour>();
        if (!_behaviour)
            Debug.Log("Player " + gameObject.name + " does not have playerbehaviour component!");
    }

    /// <summary>
    /// Returns all tiles that are in movement range, taking into account blockyblocks etc.
    /// </summary>

    public List<Tile> TilesInRange(Tile startTile, int movementPoints)
    {
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
        _behaviour.currentTile = destinationTile;
    }

    public void ExampleEventsForEditor()
    {
        exampleEvents.Invoke();
    }


#if UNITY_EDITOR
    //using UnityEditor;

    [UnityEditor.CustomEditor(typeof(PlayerMovement))]
    public class LaserButtonEditor : UnityEditor.Editor
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
