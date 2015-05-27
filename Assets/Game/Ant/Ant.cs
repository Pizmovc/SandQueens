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
	private List<MapNode> nodeList = new List<MapNode>();
	private MapNode homeNode;
    private float movementSpeed = 3;
    public float rotationSpeed = 2;
    private AntType antType;
    /// <summary>
    /// Creates basic worker ant.
    /// </summary>
	public Ant()
    {
        antType = AntType.worker;
	}
    /// <summary>
    /// Contructor for a new Ant of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Type of contructed ant.</param>
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
    /// <summary>
    /// Get homeNode.
    /// </summary>
    /// <returns>homeNode of the ant.</returns>
	public virtual MapNode GetHomeNode()
    {
		return(homeNode);
	}
    /// <summary>
    /// Sets homeNode to <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node to become homeNode.</param>
    public virtual void SetHomeNode(MapNode node)
    {
        homeNode = node;
    }

    

	public virtual void Eat()
    {
		//reduce amount of food by X (more if queen, or fighter, less if worker, nursery ant)
	}
    /// <summary>
    /// Sets the up direction of the transform according to underlying terrain.
    /// </summary>
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
        Debug.DrawLine(fr.point, frontRight);*/
    }
    /// <summary>
    /// Add node to <see cref="nodeList"/>.
    /// </summary>
    /// <param name="node">Node to be added.</param>
    public void AddNode(MapNode node)
    {
        if (node.IsOfType(NodeType.link))
            nodeList.Insert(0, node);
        else if (node.IsOfType(NodeType.antHill))
            SetHomeNode(node);
        else
            nodeList.Add(node);
    }
    
    public void AddNode(Vector3 loc, NodeType type)
    {
        MapNode newNode = new MapNode(loc, type);
        AddNode(newNode);
    }
    /// <summary>
    /// Get a node from <see cref="nodeList"/> if it exist, otherwise returns homeNode.
    /// </summary>
    /// <param name="type">Type of node to get.</param>
    /// <returns></returns>
    private MapNode GetNode(NodeType type)
    {
        if (nodeList.Count != 0)
        {
            return (nodeList.Find(item => item.IsOfType(type)));  
        }
        else
        {
            return GetHomeNode();
        }
    }
    /// <summary>
    /// Get a node from <see cref="nodeList"/>.
    /// </summary>
    /// <returns>If <see cref="nodeList"/> is not empty it returns first element, othewise returs <see cref="GetHomeNode()"/></returns>
    private MapNode GetNode()
    {
        return (nodeList.Count != 0) ? nodeList[0] : GetHomeNode();
    }
    /// <summary>
    /// Removes a node from NodeList as it was reached.
    /// <para>Checks it the node reached is homenode and destroys the object.</para>
    /// </summary>
    /// <param name="node"></param>
    private void NodeReached(MapNode node)
        {/*
            if(destinationList[0].type != NodeType.link)
                Debug.Log("Destination reached: " + destinationList[0].type.ToString());*/
            if (node == GetHomeNode())
            {
                Debug.Log("Welcome home " + gameObject.name);
                Destroy(gameObject);
            }
            else if (nodeList.Count == 0)
            {
                Debug.LogWarning("Can't remove destination, as destination list is empty.");
            }
            else if (nodeList.Exists(item => item == node))
            {
                nodeList.Remove(node);
            }
            else
            {
                Debug.LogError("Something went wrong! DestinationReached has failed!");
                Debug.Break();
            }
        }
    /// <summary>
    /// A function that looks at target node and creates linking nodes, 
    /// adds wiggle to the path and moves the ant in the desired direction.
    /// </summary>
    private void MoveTowardsTarget()
    {
        MapNode node = GetNode();
        if (node == null)
        {
            Debug.Log("GetNode returned null for " + gameObject.name);
            return;
        }
        Vector3 directionOfTravel = node.GetDirectionFrom(transform);

        if (!node.IsOfType(NodeType.link) && Vector3.Distance(transform.position, node.GetLocation(transform)) > 2*RM.AntSettings.nodeDistance)
        {
            AddNode(node.GenerateLinkNode(transform, directionOfTravel));
            MoveTowardsTarget();
            return;
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
        if (Vector3.Distance(transform.position, node.GetLocation(transform)) > 0.4f)
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
            NodeReached(node);
        }
    }

    /// <summary>
    /// Determine the signed angle between two vectors with normal as the rotation axis.
    /// </summary>
    /// <param name="firstVector">The first vector of angle checking.</param>
    /// <param name="secondVector">The second vector of angle checking.</param>
    /// <param name="normal">Vector of rotation to measure angle against.</param>
    /// <returns>A signed angle in </returns>
    public static float AngleSigned(Vector3 firstVector, Vector3 secondVector, Vector3 normal)
    {
        return Mathf.Atan2(
            Vector3.Dot(normal, Vector3.Cross(firstVector, secondVector)),
            Vector3.Dot(firstVector, secondVector)) * Mathf.Rad2Deg;
    }

    
}
public enum AntType { queen, worker, soldier, nurse};