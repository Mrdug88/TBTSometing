using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityPlayerController : MonoBehaviour
{

    // Use this for initialization
    public float maxSpeed = 5f;

    [Range(0, 1)]
    public float moveSpeed = 1f;
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    bool facingRight = true;
    public Animator anim = null;
    private Rigidbody2D rgb2d = null;
    public float jumpForce = 700f;
    bool isSprinting = false;
    public bool djump = false;


    private void Awake()
    {
        rgb2d = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        if (grounded)
        {
            djump = false;
        }
        anim.SetBool("Ground", grounded);
        anim.SetBool("Sprint", isSprinting);
        anim.SetFloat("vSpeed", rgb2d.velocity.y);

        /*float move = Input.GetAxis("Horizontal");
		*/

        //Debug.Log(rgb2d.velocity);
        rgb2d.velocity = new Vector2(moveSpeed * maxSpeed, rgb2d.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveSpeed));

        /*if (move > 0 &&!facingRight)
			flip();
		else if (move < 0 &&facingRight)
			flip();
        */

    }

    //better for inputs
    private void Update()
    {
        //jump per se
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Ground", false);
            rgb2d.AddForce(new Vector2(0, jumpForce));
        }

		if (!grounded && Input.GetKeyDown(KeyCode.Space) && !djump)
		{
			// zero out velocity so Double jump works betters
			rgb2d.velocity = new Vector2(rgb2d.velocity.x,0f); 
			//djump = true;
            rgb2d.AddForce(new Vector2(0, jumpForce-50f));
		}

        //sprinting
        if (Input.GetKey(KeyCode.LeftShift) && grounded)
        {
            Debug.Log("LShiftDown");
            isSprinting = true;
            maxSpeed = Mathf.Clamp(maxSpeed + 3f * Time.deltaTime, 5f, 8f);

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            Debug.Log("LShiftUP");
            maxSpeed = 5f;
        }

    }


    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }
}
