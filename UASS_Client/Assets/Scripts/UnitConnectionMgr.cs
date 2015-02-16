using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UnitConnectionMgr : MonoBehaviour {

	// mutex lock
	public Mutex posMutex;

	// receiving Thread
	private Thread receiveThread;
	
	// udpclient object
	private UdpClient client;

	// receive port
	private int port = 8053; // define > init

	// list of all units IP addresses
	public List<IPAddress> unitIPs;

	// reference to unit manager
	public GameObject unitMgr;

	private UnitMgr unitMgrScript;

	// Use this for initialization
	void Start () 
	{
	
		posMutex = new Mutex();

		unitIPs = new List<IPAddress>();

		unitMgrScript = unitMgr.GetComponent<UnitMgr>();

		InitSocket();
	}

	void InitSocket()
	{	
		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	// receive thread
	private  void ReceiveData()
	{
		bool existsFlag = false;
		
		client = new UdpClient(port);
		while (true)
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);

				Debug.Log(anyIP.Address.ToString() + " " + anyIP.Port.ToString());
				string msg = Encoding.UTF8.GetString(data);
				Debug.Log("Msg received from " + anyIP.Address.ToString() + " " + anyIP.Port.ToString() + ": " + msg);

				// split message into position vector
				posMutex.WaitOne(); // MOVE ME
				string[] parsed = msg.Split(' ');
				// 
				switch(parsed[0])
				{
					// request to join
					case "0":
						// check to see if robot already requested to join
						existsFlag = false;
						foreach (IPAddress ip in unitIPs)
						{
							if(ip.ToString() == anyIP.Address.ToString())
							{
								existsFlag = true;
							}
						}
						if(existsFlag == false)
						{
							// make new unit for this IP address
							Debug.Log("Creating new unit");
							Unit newU = new Unit();
							newU.Position = new Vector3(0.0f,0.0f,0.0f);
							newU.PositionOffset = new Vector3(100.0f,100.0f,100.0f);
							newU.Orientation = new Vector3(0.0f,0.0f,0.0f);
							newU.UnitType = Convert.ToInt32(parsed[1]);
							newU.IPAddress = anyIP.Address.ToString();
							newU.Port = anyIP.Port;
							newU.IsSelected = false;
							string newID = unitMgrScript.AddUnit(newU);

							// send message to ROS node 
						}
						else
						{
							Debug.Log("Unit already exists");
						}
						break;
					// position update from robot
					case "3":
						Debug.Log("Unit update position msg");
						break;
					default:
						Debug.Log("Mgs not in protocol");
						break;
					
				}
				/*if(parsed.Length >= 14)
				{
					x = (float)Convert.ToDouble(parsed[1]);
					y = (float)Convert.ToDouble(parsed[3]);
					z = (float)Convert.ToDouble(parsed[5]);
					roll = (float)Convert.ToDouble(parsed[7]);
					pitch = (float)Convert.ToDouble(parsed[9]);
					yaw = (float)Convert.ToDouble(parsed[11]);
				}*/
				posMutex.ReleaseMutex();

				
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}

	public void OnDisable() 
	{ 
		if ( receiveThread!= null) 
			receiveThread.Abort(); 
		
		client.Close(); 
	} 
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

