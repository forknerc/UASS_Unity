using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UpdatePosition : MonoBehaviour {
	public Text PosText;
	Ray ray;
	Vector3 point;
	public Vector3 pos;
	public float distance = 5.0f;


 	// Use this for initialization
	void Start () {
		pos = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		point = ray.origin + (ray.direction * distance);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit)) {
			// Create a particle if hit
			pos=hit.point;
		}


		PosText.text = pos.ToString();
	}
}
