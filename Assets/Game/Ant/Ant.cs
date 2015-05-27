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
    /// Add node to <see cref="nodeList"/>. If the NodeType is Link, then it is inserted to the beggining of the list,
    ///  if its of type antHill it is set as homeNode, 
    ///  otherwise it is added to the end of the list
    /// </summary>
    /// <param name="node">Node to be added.</param>
    public void AddNode(MapNode node)
    {
        if (node.IsOfType(NodeType.link) || node.IsOfType(NodeType.detour))
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
    /// Get a node from <see cref="nodeList"/> if it exist, otherwise returns null.
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
            return null;
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
            Debug.LogError("Something went wrong! DestinationReached has failed! " + node);
            Debug.Break();
        }
    }
    private void RemoveNode(MapNode node)
    {
        if (node == GetHomeNode())
        {
            Debug.LogError("Tried to remove homeNode: " + gameObject.name);
            Debug.Break();
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
            Debug.LogError("Something went wrong! RemoveNode has failed! " + node);
            Debug.Break();
        }
    }
    /// <summary>
    /// A function that looks at target node and creates linking nodes, 
    /// adds wiggle to the path and moves the ant in the desired direction.
    /// </summary>
    private void MoveTowardsTarget()
    {
        checkForCollisions();
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
        

        
        //Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
        //Debug.DrawLine(transform.position, transform.position + directionOfTravel, Color.white);
        /*Debug.DrawLine(transform.position, node.GetLocation(transform));
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

    private void checkForCollisions()
    {
        Vector3[] directions = {    (transform.forward - transform.right).normalized * 0.5f,  //outermost left (not used in raycasting)
                                (2 * transform.forward - transform.right).normalized * 0.8f,  //a lot to left
                                (4 * transform.forward - transform.right).normalized,  //a bit to left
                                transform.forward, //forward
                                (4 * transform.forward + transform.right).normalized,  // a bit to right
                                (2 * transform.forward + transform.right).normalized * 0.8f,  //a lot to right
                                    (transform.forward + transform.right).normalized * 0.5f,  //outermost right (not used in raycasting)
                            };
        /*
        Debug.DrawLine(transform.position, transform.position + directions[0], Color.red);
        Debug.DrawLine(transform.position, transform.position + directions[1], Color.blue);
        Debug.DrawLine(transform.position, transform.position + directions[2], Color.white);
        Debug.DrawLine(transform.position, transform.position + directions[3], Color.yellow);
        Debug.DrawLine(transform.position, transform.position + directions[4], Color.white);
        Debug.DrawLine(transform.position, transform.position + directions[5], Color.blue);
        Debug.DrawLine(transform.position, transform.position + directions[6], Color.red);
        */
        RaycastHit[] hits = new RaycastHit[7];
        bool[] bools = new bool[7];

        bool isColliding = false;

        for (int i = 0; i < 7; i++)
        {
            bools[i] = Physics.Raycast(transform.position, directions[i], out hits[i], 1.0f);
            //Debug.Log(i + " " + bools[i]);
            if (bools[i])
                isColliding = true;
        }
        if (!isColliding)
            return;

        int leftCount = 0;
        int rightCount = 0;

        if (bools[1])
            leftCount++;
        if (bools[2])
            leftCount++;
        if (bools[4])
            rightCount++;
        if (bools[5])
            rightCount++;

        int index;
        if (leftCount < rightCount)
            index = 0;
        else if (leftCount > rightCount)
            index = 6;
        else
            index = Random.Range(0, 2) * 6;

        MapNode toDelete = GetNode(NodeType.link);
        while (toDelete != null)
        {
            RemoveNode(toDelete);
            toDelete = GetNode(NodeType.link);
        }

        toDelete = GetNode(NodeType.detour);
        while (toDelete != null)
        {
            RemoveNode(toDelete);
            toDelete = GetNode(NodeType.detour);
        }
        AddNode(transform.position + directions[index], NodeType.detour);

        
    }
}
public enum AntType { queen, worker, soldier, nurse};