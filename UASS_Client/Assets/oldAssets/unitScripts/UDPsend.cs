/*
 
    -----------------------
    UDP-Send
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
    // > gesendetes unter
    // 127.0.0.1 : 8050 empfangen
   
    // nc -lu 127.0.0.1 8050
 
        // todo: shutdown thread at the end
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPsend : MonoBehaviour
{
	private static int localPort;
	
	// prefs
	private string IP;  // define in init
	public int port;  // define in init
	
	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;
	
	// gui
	//string strMessage="";
	
	// start from unity3d
	public void Start()
	{
		init();
	}
	
	// init
	public void init()
	{
		//
		//print("UDPSend.init()");
		
		// define send location
		IP="134.197.41.35";
		port=8052;
		
		// ----------------------------
		// Senden
		// ----------------------------
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		client = new UdpClient();
		
		// status
		//print("Sending to "+IP+" : "+port);
		//print("Testing: nc -lu "+IP+" : "+port);
		
	}
	
	// sendData
	public void sendString(string message)
	{
		try
		{
			byte[] data = Encoding.UTF8.GetBytes(message);
			
			client.Send(data, data.Length, remoteEndPoint);
		}
		catch (Exception err)
		{
			print(err.ToString());
		}
	}
}
