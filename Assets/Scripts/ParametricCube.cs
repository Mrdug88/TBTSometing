using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametricCube : MonoBehaviour {
	public int band;
	public float startScale , scaleMultiplier;
	public bool useBuffer;
	public AudioVV audioController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (useBuffer)
		{
			transform.localScale = new Vector3(transform.localScale.x,(audioController.audioBandBuffer[band] * scaleMultiplier) * startScale,transform.localScale.z);
		}else
		{
			transform.localScale = new Vector3(transform.localScale.x,(audioController.audioBand[band] * scaleMultiplier) * startScale,transform.localScale.z);
		}
	}
}
