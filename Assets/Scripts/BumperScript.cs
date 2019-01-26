using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BumperScript : MonoBehaviour
{
    private Collider2D thisCollider;
    public static Vector2 bumberPosition;
    private void Update()
    {
        bumberPosition = new Vector2(this.transform.position.x, this.transform.position.y);
    }
    private void Awake()
    {
        thisCollider = this.GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If Player 
        if (other.tag == "Player")
        {
            Debug.Break(); // To be Some Animation or something.. pause
                           //play animation blablablablabla//
            return;
        }

        if (other.tag == "BackGround")
        {
            //
            //
        }

        if (other.GetComponent<CompositeCollider2D>() != null)
        {
            Tilemap tileMap = other.GetComponent<Tilemap>();
            for (int y = Mathf.CeilToInt(-thisCollider.bounds.size.y)
                    ; y < Mathf.CeilToInt(thisCollider.bounds.size.y); y++)
            {
                tileMap.SetTile(new Vector3Int(Mathf.CeilToInt(this.transform.position.x
                - tileMap.transform.position.x), y, 0), null);
            }
        }
        
    }
}
