using UnityEngine;
using System.Collections;
using ResourceManager;

public class MapNode {

    private NodeType type;
    private Vector3 location;

    public MapNode(Vector3 destination, NodeType type)
    {
        this.location = destination;
        this.type = type;   
    }

    public bool IsOfType(NodeType type)
    {
        if (this.type == type)
            return (true);
        else
            return (false);
    }

    public Vector3 GetLocation(Transform tran)
    {
        location.y = tran.position.y;
        return (location);
    }

    public Vector3 GetDirectionFrom(Transform tran)
    {
        Vector3 targetPosition = this.GetLocation(tran);
        targetPosition.y = tran.position.y;
        Vector3 currentPosition = tran.position;
        Vector3 directionOfTravel = targetPosition - currentPosition;
        directionOfTravel.Normalize();
        //get a vector that is ortogonal to transform.up and direction of travel
        Vector3 ortogonalToUpAndDirection = Vector3.Cross(tran.up, directionOfTravel);
        ortogonalToUpAndDirection.Normalize();
        //get a vector that is the actual direction of travel for the ant (ortogonal to up and 'right')
        directionOfTravel = Vector3.Cross(ortogonalToUpAndDirection, tran.up);

        return (directionOfTravel);
    }

    public MapNode GenerateLinkNode(Transform tran, Vector3 directionOfTravel)
    {
        Quaternion rotation = Quaternion.Euler(0,RM.AntSettings.nodeSpreadAngle, 0);
        Vector3 nodeLocationLocal = rotation * directionOfTravel;
        nodeLocationLocal = nodeLocationLocal * RM.AntSettings.nodeDistance + tran.position;
        MapNode newNode = new MapNode(nodeLocationLocal, NodeType.link);
        //Debug.Log(newNode.nodeLocation);
        return (newNode);
    }
}
public enum NodeType { food, enemy, anObject, antHill, link };