using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour {
	private CharacterController controller;
	public bool isJump = false;
	public float jumpForce = 10.0f;
	public float VerticalVelocity;
	public float gravity = 14.0f;
	public GameObject player;
	public float disToGround = 0.5f;
	public LayerMask ground;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.isGrounded == true) {
			VerticalVelocity = -gravity * Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.Space)) {
				VerticalVelocity = jumpForce;
			}
		}
		else {
				VerticalVelocity -= gravity * Time.deltaTime;
			}
		//Vector3 MoveDirection = new Vector3 (0,VerticalVelocity,0);
		//controller.Move (MoveDirection * Time.deltaTime);


	}
}

