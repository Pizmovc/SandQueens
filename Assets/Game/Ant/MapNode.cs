using UnityEngine;
using System.Collections;
using ResourceManager;

//public enum NodeType { food, enemy, anObject, antHill, link, detour, waitForSec, waitForAnt };
public class MapNode {

    private string type;
    private Vector3 location;
    private float waitTime;

    /// <summary>
    /// Constructor for MapNode.
    /// </summary>
    /// <param name="destination">Nodes location.</param>
    /// <param name="type">Type of this node.</param>
    public MapNode(Vector3 destination, string type)
    {
        this.location = destination;
        this.type = type;   
    }

    /// <summary>
    /// Function for getting this nodes type.
    /// </summary>
    /// <returns><code>NodeType</code> type of this node.</returns>
    public string GetNodeType()
    {
        return (type);
    }

    /// <summary>
    /// Checks if supplide type is the same as this nodes type.
    /// </summary>
    /// <param name="type">NodeType used for checking.</param>
    /// <returns><code>True</code> if  this node is of type type, otherwise returns <code>false</code>.</returns>
    public bool IsOfType(string type)
    {
        if (this.type == type)
            return (true);
        else
            return (false);
    }

    /// <summary>
    /// Function for getting location data from a node.
    /// </summary>
    /// <param name="tran">Transform used to modify y component of location.</param>
    /// <returns>Location vector with modified y component so its the same as transforms.</returns>
    public Vector3 GetLocation(Transform tran)
    {
        location.y = tran.position.y;
        return (location);
    }

    /// <summary>
    /// Uses transform parameter for calculating the desired travel direction of the ant acording to MapNode location data.
    /// </summary>
    /// <param name="tran">Transfrom of calling object.</param>
    /// <returns>A normalized direction vector.</returns>
    public Vector3 GetDirectionFrom(Transform tran)
    {
        Vector3 targetPosition = this.GetLocation(tran);
        targetPosition.y = tran.position.y;
        Vector3 currentPosition = tran.position;
        Vector3 directionOfTravel = (targetPosition - currentPosition).normalized;
        //get a vector that is ortogonal to transform.up and direction of travel
        Vector3 ortogonalToUpAndDirection = (Vector3.Cross(tran.up, directionOfTravel)).normalized;
        //get a vector that is the actual direction of travel for the ant (ortogonal to up and 'right')
        directionOfTravel = Vector3.Cross(ortogonalToUpAndDirection, tran.up).normalized;

        return (directionOfTravel);
    }

    /// <summary>
    /// Creates a random link node in the general direction of travel.
    /// </summary>
    /// <param name="tran">Transform object of ant.</param>
    /// <param name="directionOfTravel">Current direction of travel for the ant.</param>
    /// <returns>A generated link node for ant to follow.</returns>
    public MapNode GenerateLinkNode(Transform tran, Vector3 directionOfTravel)
    {
        Quaternion rotation = Quaternion.Euler(0,RM.AntSettings.nodeSpreadAngle, 0);
        Vector3 nodeLocationLocal = rotation * directionOfTravel;
        nodeLocationLocal = nodeLocationLocal * RM.AntSettings.nodeDistance + tran.position;
        MapNode newNode = new MapNode(nodeLocationLocal, "link");
        //Debug.Log(newNode.nodeLocation);
        return (newNode);
    }
}