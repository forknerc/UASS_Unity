using UnityEngine;
using System.Collections;

public class pioneerUnit : MonoBehaviour {
	
	private UDPReceivePioneer netReader;
	
	public GameObject UDPobj;
	UDPReceivePioneer UDPrecieve;
	public GameObject myCircle;
	
	UDPsend sendUDP;
	
	public bool isSelected;
	
	// Use this for initialization
	void Start () {
		
		
		//netReader = FindObjectOfType(typeof(UDPReceive)); 
		UDPobj = GameObject.Find("networkControlPioneer");
		UDPrecieve = UDPobj.GetComponent<UDPReceivePioneer>();
		sendUDP = UDPobj.GetComponent<UDPsend>();
		myCircle = GameObject.Find("selectionCircle1");
		
		isSelected = false;
		

		
	}
	
	// Update is called once per frame
	void Update () {
		
		// update drone position
		transform.position = UDPrecieve.getPos();
		
		// update drone orientation
		Quaternion ori = UDPrecieve.getOri();
		Vector3 vec = ori.eulerAngles;
		vec.y = -vec.y;
		ori.eulerAngles = vec;
		transform.rotation = ori;	
		
		
		if(isSelected)
		{
			myCircle.gameObject.SetActive(true);
		}
		else 
		{
			myCircle.gameObject.SetActive(false);
		}		
	}
	
	public void sendCommand(Vector3 pos)
	{
		string msg = "c goto " + pos.x + " " + pos.z + " " + transform.position.y + " 0";
		sendUDP.sendString(msg);
		Debug.Log("sent msg: " + msg);
	}
}
