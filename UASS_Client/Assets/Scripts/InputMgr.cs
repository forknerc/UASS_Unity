using UnityEngine;
using System.Collections;

public class InputMgr : MonoBehaviour {
	public float distance = 5.0f;
	Ray ray;
	Vector3 point;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		point = ray.origin + (ray.direction * distance);
	}

	void OnGUI()
	{	
		GUI.TextField(new Rect(200, 000, 200, 25), point.ToString());
	}

}
