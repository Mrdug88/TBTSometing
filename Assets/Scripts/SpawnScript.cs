using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{

    public int level;

    public GameObject[] objects;
    //public float spawnRateMin = 1f;
    //public float spawnRateMax = 2f;

    private void Start()
    {
        Spawn(level);
    }

    void Spawn(int level)
    {
        if (level != 1)
        {
            Instantiate(objects[Random.Range(0, objects.GetLength(0))], transform.position, Quaternion.identity);
            
        }
        else
        {
            Instantiate(objects[Random.Range(0, objects.GetLength(0) - 1)], transform.position, Quaternion.identity);
            
        }

    }
}
