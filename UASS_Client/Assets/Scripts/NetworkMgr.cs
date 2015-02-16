using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

public class NetworkMgr : MonoBehaviour {
//First section deals with hosting/joining rooms.
//Seconds section deals with accepting robot connection requests.


	//Server Related Variables
	private const string typeName = "UASS_Server";
	private const string gameName = "ECSL_Lab";
	//private string OperatingSystem = "N/A";
	private string path;
	private Process ServerProcess = null;
	private HostData[] hostList;

	private string UserIpAddress;
	private int UserPort = 23467;

	private void StartUnityServer()
	{
		Network.InitializeServer(8, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		MasterServer.updateRate = 2;
	}

	private void StartUserServer()
	{
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23467;
		//Run Server Executable depending on OS
		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			path += "/../";
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			path += "/../MasterServer/VisualStudio/Debug/MasterServer";
		}

		ServerProcess = new Process();
		ServerProcess.StartInfo.FileName = path;
		ServerProcess.Start();		

		Network.InitializeServer(8, 25001, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName,"UserGame");
	}


	private string portString = "";
	void OnGUI()
	{
		GUI.TextField(new Rect(0, 0, 200, 25), MasterServer.ipAddress + "  " + MasterServer.port);




		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 200, 50), "Start Unity Server"))
				StartUnityServer();
			if (GUI.Button(new Rect(100, 150, 200, 50), "Start User Server"))
				StartUserServer();
			if (GUI.Button(new Rect(100, 200, 200, 50), "Refresh Hosts"))
				RefreshHostList();
			if (GUI.Button(new Rect(100, 250, 200, 50), "Refresh LANs"))
				RefreshLANList();

			//Allows user to enter in port of user host.
			UserIpAddress = GUI.TextField(new Rect(100, 300, 120, 20), UserIpAddress);
			
			//Allows user to enter in port of user host.
			portString= GUI.TextField(new Rect(230, 300, 60, 20), UserPort.ToString(), 6);
			portString= Regex.Replace(portString, "[^0-9]", "");
			UserPort = int.Parse(portString);


			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}


	
	private void RefreshHostList()
	{	
		Network.Disconnect ();
		MasterServer.ipAddress = "67.225.180.24";
		MasterServer.port = 23466;
		MasterServer.RequestHostList(typeName);
	}

	private void RefreshLANList()
	{
		Network.Disconnect();
		MasterServer.ipAddress = UserIpAddress;
		MasterServer.port = UserPort;
		MasterServer.RequestHostList(typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if(msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void InitializeServerSettings()
	{
		UserIpAddress = Network.player.ipAddress;
		hostList = new HostData[0];
		Network.sendRate = 100;
		path = Application.dataPath;
		Application.runInBackground = true;
		//		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		//			OperatingSystem = "Windows";	
	}

	void OnDestroy()
	{
		if(ServerProcess != null)
			ServerProcess.Kill ();
	}

	// Use this for initialization
	void Start () {	
		InitializeServerSettings ();
	}
	
	// Update is called once per frame
	void Update () {

	}






















}
