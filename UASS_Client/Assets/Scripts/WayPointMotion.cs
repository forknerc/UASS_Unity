using UnityEngine;
using System.Collections;

public class WayPointMotion : MonoBehaviour {

	public float offsetNum;
	public float amplitude;
	public float heightOffset;
	public float freq;
	private float offset;

	// Use this for initialization
	void Start () {
	
		offset = Mathf.PI * offsetNum / 4;

	}
	
	// FixedUpdate is called once per physics call
	void FixedUpdate () {
	
		transform.position = new Vector3(0.0f, ((Mathf.Sin(offset + Time.time * freq)) * amplitude) + heightOffset, 0.0f);
	}
}
