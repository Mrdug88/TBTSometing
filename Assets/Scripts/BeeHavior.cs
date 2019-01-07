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
    [Range(1, 100)]
    public float diveSpeed;
    enum Status { onDive = 1, dead = 2, idle = 0 }
    Status status = 0;
    [Range(0.1f, 1)]
    public float rotationSpeed = 0.1f;

    bool fez = false;

    //Flicker-------------
    Vector2 centerPosition;
    [Range(1, 100)]
    float angle;
    public float flickerSpeed = 5f;
    float radius = 0.05f;
    Transform parent;
    #endregion


    void Awake()
    {
        centerPosition = transform.position;
        parent = transform.parent;
    }

    private void Start()
    {



    }

    void Update()
    {

    }

    void LateUpdate()
    {

        if (status == Status.idle)
        {
            //Flickering---------------
            angle += flickerSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(-angle), Mathf.Cos(-angle)) * radius;
            transform.position = centerPosition + offset;

            //OnSight---------------
            if ((Vector3.Angle(-transform.right, target.transform.position - transform.position)
                <= angleOfview)
            && (Vector3.Distance(transform.position, target.transform.position) <= fieldOfView))
            {
                StartCoroutine(Dive());
                //Alert thing
                //Do it Once Rotation (x) 
                //Add particle effect
                //"Atack Player" -- Straight line with a slight curve to the future point (based on speed)?                             
            }
        }

    }

    IEnumerator Dive()
    {
        status = Status.onDive;
        Quaternion rotation = Quaternion.LookRotation
            (target.transform.position - parent.position, parent.TransformDirection(Vector3.up));
        
        for (float f = 1f; f >= 0; f -= rotationSpeed)
        {
            Debug.DrawLine(parent.transform.position, target.transform.position, Color.red, 1);

            parent.rotation = Quaternion.Slerp(parent.rotation,
                                         new Quaternion(0, 0, rotation.z, rotation.w),
                                         rotationSpeed * Time.time);
             yield return null;                             
        }

      


       
    }

}
