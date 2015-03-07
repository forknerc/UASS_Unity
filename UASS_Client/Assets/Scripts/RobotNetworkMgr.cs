using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UASS.unitInfoStruct;
using CielaSpike;

public class RobotNetworkMgr : MonoBehaviour {
	
	// udpclient object
	private UdpClient client;
	
	// receive port
	private int port = 8053; // define > init
	
	// list of all units IP addresses

	private UnitMgr unitMgrScript;
	
	private string newUnitsID;
	private newRobotInfo newU;
	private bool makeNewUnit;
	private bool tripWire;
	
	// Use this for initialization
	void Start () 
	{
		makeNewUnit = false;
		tripWire = true;
		unitMgrScript = GetComponent<UnitMgr>();
		//InitSocket();
		this.StartCoroutineAsync(ReceiveData());
	}


	void OnDestroy()
	{
		this.StopAllCoroutines();
		if(client != null)
			client.Close ();
	}

	// receive thread
	IEnumerator ReceiveData()
	{
		yield return Ninja.JumpBack;
		bool existsFlag = false;
		bool SocketStable = true;
		byte[] data = new Byte[0];

		client = new UdpClient(port);
		while (SocketStable)
		{
			IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
			try
			{
				data = client.Receive(ref anyIP);
			}
			catch
			{
				Debug.Log("client receive port closed");
				SocketStable = false;
			}
				
			if(SocketStable)
			{

				//Debug.Log(anyIP.Address.ToString() + " " + anyIP.Port.ToString());
				string msg = Encoding.UTF8.GetString(data);
				//Debug.Log("Msg received from " + anyIP.Address.ToString() + " " + anyIP.Port.ToString() + ": " + msg);

				// split message into position vector
				string[] parsed = msg.Split(' ');
 
				switch(parsed[0])
				{
					// request to join
				case "0":
					// check to see if robot already requested to join
					existsFlag = false;
					/*foreach (GameObject gameO in unitMgrScript.allUnits)
					{
						if(gameO.GetComponent<Unit>().IPAddress == anyIP.Address.ToString())
						{
							existsFlag = true;
						}
					}*/
					if(existsFlag == false)
					{						
						// make new unit for this IP address
						Debug.Log("Creating new unit");
						newU = new newRobotInfo();
						newU.Position = new Vector3(0.0f,0.0f,0.0f);
						newU.Orientation = new Vector3(0.0f,0.0f,0.0f);
						newU.UnitType = Convert.ToInt32(parsed[1]);


						if(anyIP.Address.ToString() == "127.0.0.1")
						{
							if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
							{
								newU.IPAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
							}
						}
						else
						{
							newU.IPAddress = anyIP.Address.ToString();
						}
							
						newU.Port = 8052;
						newU.IsSelected = false;
					

						//make new unit
						yield return Ninja.JumpToUnity;
						newUnitsID = unitMgrScript.AddUnit(newU);
						yield return Ninja.JumpBack;

						// send message to ROS node 
						IPEndPoint sendDest = new IPEndPoint(anyIP.Address, 8051);
						Byte[] sendMsg = Encoding.ASCII.GetBytes("1 " + newUnitsID);
						client.Send(sendMsg, sendMsg.Length, sendDest);
						Debug.Log("Send message to " + anyIP.Address.ToString() + ": 1 " + newUnitsID); 
					}
					else
					{
						Debug.Log("Unit already exists");
					}
					break;
					// position update from robot
				case "3":

					yield return Ninja.JumpToUnity;
					GameObject test = unitMgrScript.FindUnit(parsed[1]);
					float roll = (float)Convert.ToDouble(parsed[5]);
					float pitch = (float)Convert.ToDouble(parsed[6]);
					float yaw = (float)Convert.ToDouble(parsed[7]);

					if(test != null)
					{
						test.transform.position = new Vector3(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
						test.transform.rotation = Quaternion.Euler(-pitch, yaw, -roll);
					}
					yield return Ninja.JumpBack;
					break;
				default:
					Debug.Log("Mgs not in protocol");
					break;
					
				}
			}
		}
	}
	

	// Update is called once per frame
	void Update () 
	{

	}
}
