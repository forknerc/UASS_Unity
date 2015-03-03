﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EnableScript : MonoBehaviour {
	public GameObject NetworkObj;
	public NetworkMgr networkMgr;

	public string IpAddress;
	public int Port;
	public InputField IpInputField, PortInputField;
	private HostData[] hostList;

	
	public void IPTextChange()
	{
		IpInputField.text = Regex.Replace(IpInputField.text, "[^0-9.]", "");
		IpAddress = IpInputField.text;
		networkMgr.UserIpAddress = IpInputField.text;
	}
	
	public void PortTextChange()
	{
		PortInputField.text = Regex.Replace(PortInputField.text, "[^0-9]", "");
		// attempt to parse the value using the TryParse functionality of the integer type
		int.TryParse(PortInputField.text, out Port);
		int.TryParse(PortInputField.text, out networkMgr.UserPort);
	}

	void OnGUI()
	{
		GUI.TextField(new Rect(0, 0, 200, 25), MasterServer.ipAddress + "  " + MasterServer.port);
		
		if (!Network.isClient && !Network.isServer)
		{
			if (networkMgr.hostList != null)
			{
				for (int i = 0; i < networkMgr.hostList.Length; i++)
				{
					if (GUI.Button(new Rect(280, 60 + (60 * i), 110, 30), networkMgr.hostList[i].gameName))
					{
						networkMgr.JoinServer(networkMgr.hostList[i]);
						gameObject.SetActive(false);
					}
				}
			}
		}
	}

	public void DisableStartMenu()
	{
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		networkMgr = NetworkObj.GetComponent<NetworkMgr>();
		hostList = networkMgr.hostList;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
