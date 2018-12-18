using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour {

	public float maxSpeed = 5f;
	public bool grounded = false;
	public Transform groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	bool facingRight = true;
	public Animator anim = null;
	private Rigidbody2D rgb2d = null;
	public float jumpForce = 700f;
	bool isSprinting = false;


	private void Awake() {
		rgb2d = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponent<Animator>();
	}

	// Update is called once per frame
	private void FixedUpdate() {

		Physics2D.IgnoreLayerCollision(9,11);

		grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,whatIsGround);
		anim.SetBool("Ground",grounded);
		anim.SetBool("Sprint",isSprinting);
		anim.SetFloat("vSpeed",rgb2d.velocity.y);

		float move = Input.GetAxis("Horizontal");

		rgb2d.velocity = new Vector2(move * maxSpeed, rgb2d.velocity.y);
		anim.SetFloat("Speed",Mathf.Abs(move)); 

		if (move > 0 &&!facingRight)
			flip();
		else if (move < 0 &&facingRight)
			flip();


	}

    //better for inputs
	private void Update() {
		//jump per se
		if (grounded && Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool("Ground",false);
			rgb2d.AddForce(new Vector2(0,jumpForce));
		}

        //sprinting
		if (Input.GetKey(KeyCode.LeftShift))
		{
			//Debug.Log("LShiftDown");
			isSprinting = true;
			maxSpeed = Mathf.Clamp(maxSpeed + 3f * Time.deltaTime,5f,8f);
			
		} else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			isSprinting = false;
			//Debug.Log("LShiftUP");
			maxSpeed = 5f;
		}

        
	}


	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x = theScale.x * -1;
		transform.localScale = theScale;
	}
}
