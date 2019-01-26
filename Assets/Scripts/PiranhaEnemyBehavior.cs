using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaEnemyBehavior : MonoBehaviour {
    Rigidbody2D rgb2d;
    Transform target;
    public float fieldOfView = 5f;
    public float angleOfview = 90f;
    public float jumpForce = 300f;
    private bool onJump = false;

    void Awake () {
        rgb2d = GetComponent<Rigidbody2D> ();
        rgb2d.bodyType = RigidbodyType2D.Static;
    }
    void Start () {
        target = GameObject.FindWithTag ("Player").transform;
    }
    private void Update () {

        
        if (OnSight ()) {
            
            if (!onJump) {
                rgb2d.bodyType = RigidbodyType2D.Dynamic;
                rgb2d.AddForce (new Vector2 (0, 300f));
                onJump = true;
            }

        }

        float rotateSpeed = rgb2d.velocity.magnitude * 10f;
        transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);

    }
    bool OnSight () {
        if ((Vector3.Angle (-transform.right, target.transform.position - transform.position) <=
                angleOfview) &&
            (Vector3.Distance (transform.position, target.transform.position) <= fieldOfView)) {
            return true;
        } else {
            return false;
        }

    }

    public void ResetPiranha(){
        transform.rotation = Quaternion.identity;
        rgb2d.bodyType = RigidbodyType2D.Static;
        rgb2d.velocity = new Vector2(0,0);
        onJump = false;
    }

}