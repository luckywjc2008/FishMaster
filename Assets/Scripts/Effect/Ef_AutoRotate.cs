using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_AutoRotate : MonoBehaviour
{
    public float speed = 10f;

	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward,speed*Time.deltaTime);
	}
}
