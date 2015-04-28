using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResourceManager;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AntMovement))]

public class Ant : MonoBehaviour
{
	private List<MapNode> destinationList = new List<MapNode>();
	private MapNode homeNode;
    private float movementSpeed = 3;
    public float rotationSpeed = 2;
    private AntType antType;

	public Ant()
    {
        antType = AntType.worker;
	}

    public Ant(AntType type)
    {
        antType = type;
    }

	public virtual void Start()
    {
        SetUpDir();
	}
	
	public virtual void Update ()
    {
        SetUpDir();
        MoveTowardsTarget();
	}

	public virtual MapNode GetHomeCoordinates()
    {
		return(homeNode);
	}

    private void DestinationReached()
    {/*
        if(destinationList[0].type != NodeType.link)
            Debug.Log("Destination reached: " + destinationList[0].type.ToString());*/
        if (destinationList.Count > 0)
        {
            destinationList.RemoveAt(0);
        }
        else
        {   
            Destroy(gameObject);
        }
    }

	public virtual void Eat()
    {
		//reduce amount of food by X (more if queen, or fighter, less if worker, nursery ant)
	}

    public virtual void SetUpDir()
    {
        RaycastHit raycastHit;
        
        Physics.Raycast(transform.position, Vector3.down, out raycastHit);
        transform.position = new Vector3(raycastHit.point.x,
            raycastHit.point.y + 0.7f * transform.localScale.y,
            raycastHit.point.z);
        
        Vector3 backLeft, backRight, frontLeft, frontRight;
        RaycastHit bl, br, fl, fr;
        Vector3 upDirection;

        backLeft = transform.position - 0.7f * transform.forward * transform.localScale.z - 0.7f * transform.right * transform.localScale.x;
        backRight = transform.position - 0.7f * transform.forward * transform.localScale.z + 0.7f * transform.right * transform.localScale.x;
        frontLeft = transform.position + 0.7f * transform.forward * transform.localScale.z - 0.7f * transform.right * transform.localScale.x;
        frontRight = transform.position + 0.7f * transform.forward * transform.localScale.z + 0.7f * transform.right * transform.localScale.x;

        Physics.Raycast(backLeft, Vector3.down, out bl);
        Physics.Raycast(backRight, Vector3.down, out br);
        Physics.Raycast(frontLeft, Vector3.down, out fl);
        Physics.Raycast(frontRight, Vector3.down, out fr);

        upDirection = (Vector3.Cross(br.point, bl.point) +
                 Vector3.Cross(bl.point, fl.point) +
                 Vector3.Cross(fl.point, fr.point) +
                 Vector3.Cross(fr.point, br.point)
                 ).normalized;

        float rotationAngleZ = AngleSigned(transform.up, upDirection, transform.forward);
        float rotationAngleX = AngleSigned(transform.up, upDirection, transform.right);

        this.transform.Rotate(rotationAngleX, 0, rotationAngleZ);

        /*
        Debug.DrawLine(backLeft, backRight, Color.red);
        Debug.DrawLine(backLeft, frontLeft, Color.red);
        Debug.DrawLine(backRight, frontRight, Color.red);
        Debug.DrawLine(frontLeft, frontRight, Color.red);

        Debug.DrawLine(bl.point, backLeft);
        Debug.DrawLine(br.point, backRight);
        Debug.DrawLine(fl.point, frontLeft);
        Debug.DrawLine(fr.point, frontRight);
        */
    }

    public void AddNode(MapNode node)
    {
        if (node.IsOfType(NodeType.link))
            destinationList.Insert(0, node);
        else if (node.IsOfType(NodeType.antHill))
            homeNode = node;
        else
            destinationList.Add(node);
    }

    public void AddNode(Vector3 loc, NodeType type)
    {
        MapNode newNode = new MapNode(loc, type);
        AddNode(newNode);
    }

    private MapNode GetNode(NodeType type)
    {
        if (destinationList.Count != 0)
        {
            foreach (MapNode node in destinationList)
            {
                if (node.IsOfType(type))
                {
                    return node;
                }
            }
            return (null);
           
        }
        else
        {
            return homeNode;
        }
    }

    private MapNode GetNode()
    {
        if (destinationList.Count != 0)
            return destinationList[0];
        else
        {
            return homeNode;
        }
    }

    private void MoveTowardsTarget()
    {
        MapNode node = GetNode();
        if (node == null)
        {
            Debug.Log("GetNode returned null for " + gameObject.name);
            return;
        }
        Vector3 directionOfTravel = node.GetDirectionFrom(transform);

        if (!node.IsOfType(NodeType.link))
        {
            if (Vector3.Distance(transform.position, node.GetLocation(transform)) > 2*RM.AntSettings.nodeDistance)
            {
                AddNode(node.GenerateLinkNode(transform, directionOfTravel));
                MoveTowardsTarget();
                return;
            }
        }
        /*
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + directionOfTravel, Color.white);
        Debug.DrawLine(transform.position, node.GetLocation(transform));
        if (GetNode(NodeType.food) != null)
        {
            Debug.DrawLine(transform.position, GetNode(NodeType.food).location, Color.red);
            Debug.DrawRay(GetNode(NodeType.food).location, Vector3.up*100);
        }
        */
        //first, check to see if we're close enough to the target
        if (Vector3.Distance(transform.position, node.GetLocation(transform)) > 0.1f)
        {
            float angleBetweenForwardAndDirection = AngleSigned(transform.forward, directionOfTravel, transform.up);
            //Debug.Log(angleBetweenForwardAndDirection);
            if (Mathf.Abs(angleBetweenForwardAndDirection) > 5)
            {
                this.transform.Rotate(0, angleBetweenForwardAndDirection * rotationSpeed * Time.deltaTime, 0);
            }
            float adjustedMovementSpeed = movementSpeed * ((180 - Mathf.Abs(angleBetweenForwardAndDirection)) / 180);
            //scale the movement on each axis by the directionOfTravel vector components
            this.transform.Translate(
                (transform.forward.x * adjustedMovementSpeed * Time.deltaTime),
                (transform.forward.y * adjustedMovementSpeed * Time.deltaTime),
                (transform.forward.z * adjustedMovementSpeed * Time.deltaTime),
                Space.World);
        }
        else
        {
            DestinationReached();
        }
    }

    /// <summary>
    /// Determine the signed angle between two vectors, with normal 'n'
    /// as the rotation axis.
    /// </summary>
    public static float AngleSigned(Vector3 firstVector, Vector3 secondVector, Vector3 normal)
    {
        return Mathf.Atan2(
            Vector3.Dot(normal, Vector3.Cross(firstVector, secondVector)),
            Vector3.Dot(firstVector, secondVector)) * Mathf.Rad2Deg;
    }

    
}
public enum AntType { queen, worker, soldier, nurse};