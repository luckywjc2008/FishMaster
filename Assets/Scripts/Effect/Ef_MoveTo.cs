using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_MoveTo : MonoBehaviour
{
    private GameObject goldCollect;

	// Use this for initialization
	void Start () {
		goldCollect = GameObject.Find("GoldCollect");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position,goldCollect.transform.position,5*Time.deltaTime);
	}
}
