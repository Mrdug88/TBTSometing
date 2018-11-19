using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour {

    [Range(0f,10f)]
	public float maxSpeed = 5f;
	
	public Animator anim = null;

	private Rigidbody2D rgb2d = null;

	private void Awake() {
		rgb2d = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	private void FixedUpdate() {
		float move = Input.GetAxis("Horizontal");

		rgb2d.velocity = new Vector2(move * maxSpeed, rgb2d.velocity.y);
		anim.SetFloat("Speed",move);


	}
}
