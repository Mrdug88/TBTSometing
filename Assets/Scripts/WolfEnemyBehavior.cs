using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEnemyBehavior : MonoBehaviour {
    Rigidbody2D rgb2d;
    Transform target;
    public float fieldOfView = 2f;
    public float angleOfview = 90f;
    public float attackInterval = 4f;
    public GameObject arrowPrefab;
    float attackTimer = 0f;
    private bool canAttack = true;
    Animator animController;

    void Awake () {
        rgb2d = GetComponent<Rigidbody2D> ();
        rgb2d.bodyType = RigidbodyType2D.Static;
        animController = GetComponent<Animator> ();
        attackTimer = attackInterval;
    }
    void Start () {
        target = GameObject.FindWithTag ("Player").transform;
    }

    private void Update () {

        attackTimer = attackTimer + Time.deltaTime;

        if (attackTimer >= attackInterval) {
            canAttack = true;
        }

        if (OnSight ()) {
            if (canAttack) {

                canAttack = false;
                animController.SetTrigger ("Attack");
                //shootArrow(); // linked to animator
                attackTimer = 0f;

            }
        }

        float rotateSpeed = rgb2d.velocity.magnitude * 10f;
        transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);

    }

    void shootArrow()
    {
        //straight for now // willShoot on player direction 45 angle
        Vector2 arrowPos = new Vector2(transform.position.x - 0.5f,transform.position.y+ 0.32f);
        GameObject arrow = GameObject.Instantiate(arrowPrefab,arrowPos,Quaternion.identity);

        //angle get here
        //fixes
        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(-750f,0));
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

    //reset wareWolf for pool;

}