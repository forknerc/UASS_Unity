using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine.UI; 


public class NetworkMgr : MonoBehaviour {
//First section deals with hosting/joining rooms.
//Seconds section deals with accepting robot connection requests.
	//public GameObject tempFab;

	//Server Related Variables
	private const string typeName = "UASS_Server";
	private const string gameName = "ECSL_Lab";
	//private string OperatingSystem = "N/A";
	private string path;
	private Process ServerProcess = null;
	public HostData[] hostList;
	private string portString = "";

	public string UserIpAddress;
	public int UserPort = 23467;

	public void StartUnityServer()
	{
		Network.InitializeServer(8, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		MasterServer.updateRate = 2;
	}

	public void StartUserServer()
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
	
	public void RefreshUnityList()
	{	
		Network.Disconnect ();
		MasterServer.ipAddress = "67.225.180.24";
		MasterServer.port = 23466;
		MasterServer.RequestHostList(typeName);
	}

	public void RefreshUserList()
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

	public void JoinServer(HostData hostData)
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
