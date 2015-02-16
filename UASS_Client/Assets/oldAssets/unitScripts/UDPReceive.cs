/*
 
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051
 
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {
	
	// receiving Thread
	Thread receiveThread;

	// mutex lock
	Mutex posMutex;
	
	// udpclient object
	UdpClient client;
	
	// public
	// public string IP = "127.0.0.1"; default local
	public int port; // define > init
	
	// infos
	public string lastReceivedUDPPacket="";
	//public string allReceivedUDPPackets=""; // clean up this from time to time!
	
	public float x = 0.0f,y  = 0.0f, z = 0.0f;
	public float roll = 0.0f, pitch = 0.0f, yaw = 0.0f;

	public float posScaling;

	
	// start from shell
	/*private static void Main()
	{
		UDPReceive receiveObj=new UDPReceive();
		receiveObj.init();
		
		string text="";
		do
		{
			text = Console.ReadLine();
		}
		while(!text.Equals("exit"));
	}*/
	// start from unity3d
	public void Start()
	{
		posMutex = new Mutex();
		init();
	}
	
	// OnGUI
	void OnGUI()
	{
		Rect rectObj=new Rect(40,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj,
		        "Recieving on 127.0.0.1 : "+port+" \n"
		        + "\nLast Packet: \n"+ lastReceivedUDPPacket
		        ,style);
	}
	
	// init
	private void init()
	{
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		//print("UDPSend.init()");
		
		// define port
		port = 8051;
		
		// status
		//print("Sending to 127.0.0.1 : "+port);
		//print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
		
		
		// ----------------------------
		// Abhören
		// ----------------------------
		// Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
		// Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
		
	}
	
	// receive thread
	private  void ReceiveData()
	{
		
		client = new UdpClient(port);
		while (true)
		{
			
			try
			{
				// Bytes empfangen.
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				
				// Bytes mit der UTF8-Kodierung in das Textformat kodieren.
				string text = Encoding.UTF8.GetString(data);
				
				// Den abgerufenen Text anzeigen.
				print(">> " + text);
				//Debug.Log(text);
				
				// latest UDPpacket
				lastReceivedUDPPacket=text;
				
				// split message into position vector
				posMutex.WaitOne();
				string[] parsed = lastReceivedUDPPacket.Split(' ');
				if(parsed.Length >= 14)
					{
						x = (float)Convert.ToDouble(parsed[1]);
						y = (float)Convert.ToDouble(parsed[3]);
						z = (float)Convert.ToDouble(parsed[5]);
						roll = (float)Convert.ToDouble(parsed[7]);
						pitch = (float)Convert.ToDouble(parsed[9]);
						yaw = (float)Convert.ToDouble(parsed[11]);
					}
				posMutex.ReleaseMutex();
				
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}
	
	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket()
	{
		//allReceivedUDPPackets="";
		return lastReceivedUDPPacket;
	}

	public void OnDisable() 
	{ 
		if ( receiveThread!= null) 
			receiveThread.Abort(); 
		
		client.Close(); 
	} 

	public Vector3 getPos()
	{
		posMutex.WaitOne();
		Vector3 pos;
		pos.x = x * posScaling;
		pos.y = y * posScaling;
		pos.z = z * posScaling;
		posMutex.ReleaseMutex();

		return pos;
	}

	public Vector3 getOri()
	{
		posMutex.WaitOne();
		Vector3 ori;
		ori.x = -pitch;
		ori.y = yaw;
		ori.z = -roll;
		posMutex.ReleaseMutex();
		
		return ori;
	}

}