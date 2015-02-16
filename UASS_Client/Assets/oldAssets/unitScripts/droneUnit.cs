using UnityEngine;
using System.Collections;

public class droneUnit : MonoBehaviour {

	private UDPReceive netReader;
	
	public GameObject UDPobj;
	 UDPReceive UDPrecieve;
	public GameObject myCircle;

	UDPsend sendUDP;

	public bool isSelected;
	
	// Use this for initialization
	void Start () {
		
		
		//netReader = FindObjectOfType(typeof(UDPReceive)); 
		UDPobj = GameObject.Find("networkControl");
		UDPrecieve = UDPobj.GetComponent<UDPReceive>();
		sendUDP = UDPobj.GetComponent<UDPsend>();
		myCircle = GameObject.Find("selectionCircle1");

		isSelected = false;

		// TODO - send initialization to drone
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// update drone position
		transform.position = UDPrecieve.getPos();
		
		// update drone orientation
		Vector3 ori = UDPrecieve.getOri();
		transform.rotation = Quaternion.Euler(ori.x, ori.y, ori.z);	


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
