using UnityEngine;
using System.Collections;
using ResourceManager;

public class AntCommunication : MonoBehaviour {
    private Ant ant;

    void Start ()
    {
        ant = transform.GetComponent<Ant>();
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void AvoidCollision(RaycastHit hitAnt)
    {

    }

    public Vector3 GetIntent()
    {
        return (ant.transform.forward);
    }
}
