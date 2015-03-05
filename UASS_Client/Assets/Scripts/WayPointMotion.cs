using UnityEngine;
using System.Collections;

public class WayPointMotion : MonoBehaviour {

	public float offset;
	public float amplitude;
	public float freq;

	// Use this for initialization
	void Start () {
	
	}
	
	// FixedUpdate is called once per physics call
	void FixedUpdate () {
	
		transform.position = new Vector3(0.0f, Mathf.Abs((Mathf.Sin(offset + Time.time * freq)) * amplitude), 0.0f);

	}
}
