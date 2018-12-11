using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{

    SpawnScript[] platforms;
    public int platformPoolSize = 15;
    public SpawnScript platformSpawner;
    //int currentPlatforms = 0;
    int currentLevel = 3;
    Vector3 lastPosition = new Vector3(4, 0, -1f);

    //instantiate a number of platforms off-camera(x)
    private void Awake()
    {
        platforms = new SpawnScript[platformPoolSize];
        for (int i = 0; i < platformPoolSize; i++)
        {
            platforms[i] = (SpawnScript)Instantiate(platformSpawner, new Vector2(-30f, -30f), Quaternion.identity);
            //por no lugar
            PutOnPlace(platforms[i]);
        }

    }
    // randomly assign a position to each one (x)
    void PutOnPlace(SpawnScript platform)
    {
        //chose the level (x)
        switch (currentLevel)
        {
            case 3:
                platform.level = 3;
                currentLevel--;
                platform.transform.position = new Vector3(Random.Range(lastPosition.x - 1f, lastPosition.x + 1f), 1.5f, -1f);
                break;

            case 2:
                platform.level = 2;
                currentLevel--;
                platform.transform.position = new Vector3(Random.Range((lastPosition.x + 4) - 1f, (lastPosition.x + 4) + 1f), 0f, -1f);
                break;

            case 1:
                
                platform.level = 1;
                platform.transform.position = new Vector3(Random.Range((lastPosition.x + 2) - 1f, (lastPosition.x + 2) + 1f), -1.6f, -1f);
                lastPosition = new Vector3(lastPosition.x + 9f, lastPosition.y, lastPosition.z);
                //this level only spawn 2 types of platforms (the bigger ones) (x)
                currentLevel = 3;
                break;


            default:
                Debug.Log("Default");
                break;
        }

    }



    //on hit (bumper) realocate the platform
    // they have to be on spawned 3 at the same time, each on a level (1-2-3) (x)
    // the lowest level can only use the bigger one platforms. (x)


}
