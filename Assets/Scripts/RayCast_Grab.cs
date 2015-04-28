using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class RayCast_Grab : MonoBehaviour {

	public GameObject head;
	[SerializeField] float mRotationSensitivity;
	
	private FirstPersonController fpControl;
	private RaycastHit hit;
	private float sightDistance = 10.0f;

	void Start () {
		// reference this game object to access FirstPersonController script
		// from Unity standard assets.
		fpControl = gameObject.GetComponent<FirstPersonController> ();
	}
	

	void Update () 
	{
		if (Input.GetMouseButton (0))
		{
			if(Physics.Raycast(head.transform.position, head.transform.forward, out hit, sightDistance))
			{
				float horizontal = CrossPlatformInputManager.GetAxis("Mouse X");
				float vertical = CrossPlatformInputManager.GetAxis("Mouse Y");
				print (horizontal);
				GameObject other = hit.transform.gameObject;
				Rigidbody otherRBody = other.GetComponent<Rigidbody>();
				if(other.tag == "Grabbable"){
					other.transform.RotateAround(head.transform.position, head.transform.up, horizontal);
					other.transform.RotateAround(head.transform.position, head.transform.right, -vertical);
				}
			}
		}
	}
}