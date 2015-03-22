using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI; 

public class NetworkMgr : MonoBehaviour {
	//Server Related Variables
	private const string typeName = "UASS_Server";
	private const string gameName = "ECSL_Lab";
	//private string OperatingSystem = "N/A";
	private string path;
	private Process ServerProcess = null;
	public HostData[] hostList;
	private string portString = "";


	private string UnityMSAddress = "67.225.180.24";
	private int UnityMSPort;

	public string UserIpAddress = "127.0.0.1";
	public int UserPort = 23467;
	
	public void StartUnityServer()
	{
		Network.InitializeServer(8, 0, !Network.HavePublicAddress());
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

		Network.InitializeServer(8, 0, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName,"UserGame");
	}
	
	public void RefreshUnityList()
	{	
		if(TestConnection(UnityMSAddress))
		{
			if(MasterServer.ipAddress != UnityMSAddress)
			{
				Network.Disconnect ();
				MasterServer.ClearHostList();
				MasterServer.ipAddress = UnityMSAddress;
				MasterServer.port = UnityMSPort;
			}
			MasterServer.RequestHostList(typeName);
		}
		else
		{
			MasterServer.ClearHostList();
			hostList = null;
		}
	}

	public void RefreshUserList()
	{
		if(TestConnection(UserIpAddress))
		{
			if(MasterServer.ipAddress != UserIpAddress)
			{
				Network.Disconnect ();
				MasterServer.ClearHostList();
				MasterServer.ipAddress = UserIpAddress;
				MasterServer.port = UserPort;
			}
			MasterServer.RequestHostList(typeName);
		}
		else
		{
			MasterServer.ClearHostList();
			hostList = null;
		}


	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		UnityEngine.Debug.Log("MasterServerReached");
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void InitializeServerSettings()
	{
		UserIpAddress = Network.player.ipAddress;
		UserPort = 23467;
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
		Network.Disconnect();
	}


	// Use this for initialization
	void Start () {	
		InitializeServerSettings ();
	}
	
	// Update is called once per frame
	void Update () {
			hostList = MasterServer.PollHostList();
	}
	
	public HostData[] getHostList
	{
		get{return this.hostList;}
		set{this.hostList = value;}
	}


	public bool TestConnection(string IP)
	{
		bool thereIsConnection = true;

		float maxTime = 2.0F;
		

		Ping testPing = new Ping( IP );
		
		float timeTaken = 0.0F;
		
		while ( !testPing.isDone && thereIsConnection)
		{

			StartCoroutine(WaitXSeconds(timeTaken, 1));
			
			if ( timeTaken > maxTime )
			{
				// if time has exceeded the max
				// time, break out and return false
				thereIsConnection = false;
			}
			
		}
		if(testPing.time == 0)
			thereIsConnection = false;
		else if ( timeTaken <= maxTime) 
			thereIsConnection = true;
		UnityEngine.Debug.Log (timeTaken.ToString());
		return thereIsConnection;
	}
	

	IEnumerator WaitXSeconds(float timeTaken, int x)
	{

		yield return new WaitForSeconds(x);
		timeTaken += x;
	
	}


}
