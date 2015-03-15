using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

	public float trasnlationSpeed;
	public float zoomSpeed;
	public float rotateSpeed; // soon^tm

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float vert, horiz, zoom;	

		if((vert = Input.GetAxis("Vertical")) != 0)
		{
			transform.position += Vector3.forward * vert * trasnlationSpeed * Time.deltaTime;
		}

		if((horiz = Input.GetAxis("Horizontal")) != 0)
		{
			transform.position += Vector3.right * horiz * trasnlationSpeed * Time.deltaTime;
		}
		if((zoom = Input.GetAxis("Zoom")) != 0)
		{
			transform.position += transform.forward * zoom * zoomSpeed * Time.deltaTime;
		}
	}
}
