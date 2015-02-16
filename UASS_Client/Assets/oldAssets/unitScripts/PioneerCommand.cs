using UnityEngine;
using System.Collections;

public class PioneerCommand : MonoBehaviour {
	
	private UDPReceivePioneer netReader;
	
	public GameObject UDPobj;
	public UDPReceivePioneer UDPScript;
	
	// Use this for initialization
	void Start () {
		
		
		//netReader = FindObjectOfType(typeof(UDPReceive));
		UDPobj = GameObject.Find("networkControlPioneer");
		UDPScript = UDPobj.GetComponent<UDPReceivePioneer>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// update drone position
		transform.position = UDPScript.getPos();
		
		// update drone orientation
		Quaternion ori = UDPScript.getOri();
		Vector3 vec = ori.eulerAngles;
		vec.y = -vec.y;
		ori.eulerAngles = vec;
		transform.rotation = ori;	
	}
}
