using UnityEngine;
using System.Collections;

public class RayCast_Grab : MonoBehaviour {

	public GameObject head;

	private RaycastHit hit;
	private float sightDistance = 10.0f;

	void Start () {
	
	}
	

	void Update () 
	{
		if (Input.GetMouseButton (0)) 
		{
			if(Physics.Raycast(head.transform.position, head.transform.forward, out hit, sightDistance))
			{
				GameObject other = hit.transform.gameObject;
				if(other.tag == "Grabbable"){
					// insert grab logic here.
				}
			}
		}
	}
}