using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler poolerInstance;
    public List<GameObject> pooledObjects;
    public List<EnemiesPoolClass> enemiesToPool;
    
    [System.Serializable]
    public class EnemiesPoolClass {
        public GameObject objectToPool;
        public int poolSize;
    }

    // Start is called before the first frame update
    private void Awake () {
        poolerInstance = this;
    }
    void Start () {

        pooledObjects = new List<GameObject> ();
        foreach (EnemiesPoolClass enemies in enemiesToPool) {
            for (int i = 0; i < enemies.poolSize; i++) {
                GameObject obj = (GameObject) Instantiate (enemies.objectToPool);
                obj.SetActive (false);
                pooledObjects.Add (obj);
            }
        }

    }

    // Update is called once per frame
    void Update () {

    }

    public GameObject GetPooledObject (string tag) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) {
                return pooledObjects[i];
            }
        }
        foreach (EnemiesPoolClass item in enemiesToPool) {
            if (item.objectToPool.tag == tag) {
                GameObject obj = (GameObject) Instantiate (item.objectToPool);
                obj.SetActive (false);
                pooledObjects.Add (obj);
                return obj;
            }
        }
        return null;
    }

    /* public void updatePiranhaPos (Vector2 Position) {

        piranhas[currentPiranhas].transform.position = Position;
        currentPiranhas++;

        if (currentPiranhas >= poolSize){
            currentPiranhas = 0;
            for (int i = 0; i < poolSize; i++)
            {
                piranhas[i].GetComponent<PiranhaEnemyBehavior>().ResetPiranha();
            }
        }
    } */

}