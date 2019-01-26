using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPool : MonoBehaviour {
    public int poolSize = 5;
    public GameObject piranhaPrefab;
    public Vector2 poolPosition = new Vector2 (-25, -25);

    private GameObject[] piranhas;
    private int currentPiranhas = 0;

    void Start () {
        //Fill our Pool
        piranhas = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++) {
            piranhas[i] = (GameObject) Instantiate (piranhaPrefab, poolPosition, Quaternion.identity);
        }
    }

    //Update Piranha Position
    public void updatePiranhaPos (Vector2 Position) {

        piranhas[currentPiranhas].transform.position = Position;
        currentPiranhas++;

        if (currentPiranhas >= poolSize){
            currentPiranhas = 0;
            for (int i = 0; i < poolSize; i++)
            {
                piranhas[i].GetComponent<PiranhaEnemyBehavior>().ResetPiranha();
            }
        }
    }

}