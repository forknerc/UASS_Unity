using UnityEngine;
using System.Collections;

public class propeller : MonoBehaviour {

	public float rotateSpeed;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(new Vector3(0,0,1), rotateSpeed);
	}
}
