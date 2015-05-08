using UnityEngine;
using System.Collections;

public class collider : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		print ("You Won!");
	}
}
