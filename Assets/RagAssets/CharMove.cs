using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharMove : MonoBehaviour {
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public float rotateSpeed = 3.0F;
	private Vector3 moveDirection = Vector3.zero;
	void Update() {
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		if (controller.enabled) {
			transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			float curSpeed = speed * Input.GetAxis("Vertical");
			controller.SimpleMove(forward * curSpeed);
		}

	}
}