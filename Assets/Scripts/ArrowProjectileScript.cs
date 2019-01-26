using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectileScript : MonoBehaviour
{

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
       Physics2D.IgnoreLayerCollision(13,12);
    }
    
    void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(13,12);
    }

    //OnCollisionEnter;
}
