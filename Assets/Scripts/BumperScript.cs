using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Break();
            return;
        }

        // it should be delegated to the object, but whatever
        if (other.tag == "BackGround")
        {
            Debug.Log("BG");
            Transform tf = other.transform;
            other.gameObject.transform.position =
            new Vector3(tf.position.x + 47.56f, tf.position.y, tf.position.z);
        }
        
        if(other.tag == "Platform"){
            Debug.Log("Platform");
            other.transform.position = new Vector3
             (other.transform.position.x + (9 * 5),other.transform.position.y,other.transform.position.z);

        }

        if (other.gameObject.transform.parent && other.tag != "BackGround" && other.tag != "Platform")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        else if (other.tag != "BackGround" && other.tag != "Platform")
		{
			Destroy(other.gameObject);
		}
    }

}
