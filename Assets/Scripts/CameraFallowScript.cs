 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallowScript : MonoBehaviour {

	public Transform player;
	Transform cameraT = null;	
	// Update is called once per 
	
	private void Awake() {
		cameraT = this.GetComponent<Transform>();
	}
	void Update () {
		transform.position = new Vector3(player.position.x + 3,cameraT.position.y,cameraT.position.z);
	}
}
