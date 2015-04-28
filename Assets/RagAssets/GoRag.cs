using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoRag : MonoBehaviour {
	//private Component rb;
	private Animator animator;
	private static GoRag _ragdoll;
	public bool knockOut;
	public CharacterController cc;
	public CharMove charmove;
	
	// Use this for initialization
	void Start () {

		_ragdoll = this;//set a reference to the ragdoll class to call nonstatic members from static function

		animator = GetComponent<Animator> ();

		//rb = GetComponentInChildren<Rigidbody> ();
		cc = GetComponent<CharacterController> ();

	}

	public static void KnockOut(Transform dir){
		_ragdoll.animator.enabled = false;
		_ragdoll.knockOut = true;
	}

	void FixedUpdate(){
		Rigidbody[] bodies=GetComponentsInChildren<Rigidbody>();
		if (knockOut) {
			cc.enabled = false;
			foreach(Rigidbody rib in bodies) {
				
				rib.isKinematic = false; //disables the animated ragdoll and turns it into just a regular ragdoll with no movement
			}
			animator.enabled=false;
			animator.SetBool ("Kill", knockOut);
			//charmove.enabled = false;//disable charmove script

		} else {
			/*foreach(Rigidbody rib in bodies) {
				
				rib.isKinematic = true;
			}*/
			animator.enabled = true;
			animator.SetBool ("Kill", knockOut);
			//cc.enabled = true;
			//charmove.enabled = true;
		}
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
		{
			SetKinematic(false);
			GetComponent<Animator>().enabled=false;
			GetComponent<CharacterController>().enabled=false;
			//GetComponent<Animator>().updateMode(AnimatePhysics);
		}
		}
	void SetKinematic(bool newValue)
	{
		//Get an array of components that are of type Rigidbody
		Rigidbody[] bodies=GetComponentsInChildren<Rigidbody>();
		
		//For each of the components in the array, treat the component as a Rigidbody and set its isKinematic property
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic=newValue;

		}
	}
}
