using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

	public float translationSpeed;
	public float zoomSpeed;
	public float rotateSpeed; // soon^tm

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float vert, horiz, zoom, rotate;	

		if((vert = Input.GetAxis("Vertical")) != 0)
		{
			transform.position += Vector3.forward * vert * translationSpeed * Time.deltaTime;
		}

		if((horiz = Input.GetAxis("Horizontal")) != 0)
		{
			transform.position += Vector3.right * horiz * translationSpeed * Time.deltaTime;
		}
		if((zoom = Input.GetAxis("Zoom")) != 0)
		{
			transform.position += transform.forward * zoom * zoomSpeed * Time.deltaTime;
		}
		if((rotate = Input.GetAxis("Rotate")) != 0)
		{
			Vector3 temp = transform.rotation.eulerAngles;
			temp.x += rotate * rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(temp);
		}
	}
}
