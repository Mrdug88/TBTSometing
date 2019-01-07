using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHavior : MonoBehaviour
{
    #region Declarations

    //Dive----------------
    [Range(0, 100)]
    public float fieldOfView = 5f;
    public float angleOfview = 90f;
    public GameObject target;
    enum Status { onAttack = 1, dead = 2, idle = 0 }
    Status status = 0;
    [Range(0.1f, 1)]
    public float rotationSpeed = 0.1f;
    public float shootInverval = 0.2f;

    //Flicker-------------
    Vector2 centerPosition;
    [Range(1, 100)]
    float angle;
    public float flickerSpeed = 5f;
    float radius = 0.05f;
    Transform parent;
    SpriteRenderer sr2d;
    #endregion


    void Awake()
    {
        centerPosition = transform.position;
        parent = transform.parent;
        sr2d = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    
    }

    void Update()
    {
        //Flickering---------------
        angle += flickerSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(-angle), Mathf.Cos(-angle)) * radius;
        transform.position = centerPosition + offset;

        if (status == Status.idle)
        {
            //OnSight---------------
            if (OnSight())
            {
                status = Status.onAttack;
            }
        }

        if (status == Status.onAttack)
        {
            if (OnSight())
            {
                //Color Lerp
                sr2d.color = Color.Lerp(sr2d.color,new Color(0.9f,0.72f,0.73f,1),0.1f);
                Quaternion rotation = Quaternion.LookRotation
                (target.transform.position - parent.position, parent.TransformDirection(Vector3.up));
                //keep rotation and shoot
                Debug.DrawLine(parent.transform.position, target.transform.position, Color.red, 1);

                parent.rotation = Quaternion.Slerp(parent.rotation,
                                             new Quaternion(0, 0, rotation.z, rotation.w),
                                             rotationSpeed * Time.time);
                //Shoot Projectile
                Shoot();
                //delay
            }
            else
            {
                sr2d.color = Color.Lerp(sr2d.color,Color.white,0.1f);
                status = Status.idle;
                
            }
        }

        if (status == Status.dead)
        {
            //kill it
        }


    }
    public void Shoot()
    {

        //thorw a needle direction of player
        //rotate 5 back 5 front on the original
        //

    }

    bool OnSight()
    {
        if ((Vector3.Angle(-transform.right, target.transform.position - transform.position)
            <= angleOfview)
        && (Vector3.Distance(transform.position, target.transform.position) <= fieldOfView))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
