using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AntMovement))]

public class Ant : MonoBehaviour
{
	private List<Vector3> destinationList;
	private Vector3 homeCoordinates;
    private float movementSpeed = 3;
    public float rotationSpeed = 2;

	public Ant()
    {
		destinationList = new List<Vector3> ();
	}

	public virtual void Start()
    {
        
	}
	
	public virtual void Update ()
    {
        SetUpDir();
        MoveTowardsTarget();
        
	}

	public virtual void SetHomeCoordinates(Vector3 home)
    {
		homeCoordinates = home;
	}

	public virtual Vector3 GetHomeCoordinates()
    {
		return(homeCoordinates);
	}

	public virtual void AddDestination(Vector3 destination)
    {
		//Debug.Log ("Destination: " + destination);
		destinationList.Add(destination);
	}

	public virtual Vector3 GetDestination()
    {
        if (destinationList.Count == 0)
        {
            Destroy(gameObject);
            return (homeCoordinates);
        }
        else
            return destinationList[0];
	}

    private void DestinationReached()
    {
        if (destinationList.Count != 0)
            destinationList.RemoveAt(0);
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

    //move towards a target at a set speed.
    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = GetDestination();
        targetPosition.y = transform.position.y;
        Vector3 currentPosition = this.transform.position;

        Vector3 directionOfTravel = targetPosition - currentPosition;
        directionOfTravel.Normalize();

        //get a vector that is ortogonal to transform.up and direction of travel
        Vector3 ortogonalToUpAndDirection = Vector3.Cross(transform.up, directionOfTravel);
        ortogonalToUpAndDirection.Normalize();
        //get a vector that is the actual direction of travel for the ant (ortogonal to up and 'right')
        directionOfTravel = Vector3.Cross(ortogonalToUpAndDirection, transform.up);
        /*
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + directionOfTravel, Color.white);
        
        Debug.Log(Vector3.Distance(currentPosition, targetPosition));
        */
        //first, check to see if we're close enough to the target
        if (Vector3.Distance(currentPosition, targetPosition) > 0.2f)
        {
            float angleBetweenForwardAndDirection = AngleSigned(transform.forward, directionOfTravel, transform.up);
            //Debug.Log(angleBetweenForwardAndDirection);
            if (Mathf.Abs(angleBetweenForwardAndDirection) > 5)
            {
                this.transform.Rotate(0, angleBetweenForwardAndDirection * rotationSpeed * Time.deltaTime, 0);
            }

            float adjustedMovementSpeed = movementSpeed * ((180 - angleBetweenForwardAndDirection) / 180);

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
