using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class networkMgr_old : MonoBehaviour {

	private const string typeName = "UASS_Server";
	private const string gameName = "ECSL_Lab";
	private string OperatingSystem = "N/A";
	private string path;
	private Process ServerProcess = null;
	private HostData[] hostList;
	private string DefaultIP;

	private void StartUnityServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		MasterServer.updateRate = 2;
	}

	private void StartUserServer()
	{
		MasterServer.ipAddress = "127.0.0.1";

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

		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName,"UserGame");
	}


	public GameObject playerPrefab1;
	public GameObject pioneerPrefab;

	
	void OnServerInitialized()
	{
		//SpawnPlayer();
	}

	void OnGUI()
	{
		GUI.TextField(new Rect(0, 0, 200, 25), MasterServer.ipAddress + "  " + MasterServer.port);

		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 200, 100), "Start Unity Server"))
				StartUnityServer();
			if (GUI.Button(new Rect(100, 200, 200, 100), "Start User Server"))
				StartUserServer();
			if (GUI.Button(new Rect(100, 300, 200, 75), "Refresh Hosts"))
				RefreshHostList();
			if (GUI.Button(new Rect(100, 350, 200, 75), "Refresh LANs"))
				RefreshLANList();



			GUI.TextField(new Rect(100, 000, 200, 25), OperatingSystem);
			GUI.TextField(new Rect(100, 50, 200, 25), path);

			
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
		MasterServer.RequestHostList(typeName);
	}

	private void RefreshLANList()
	{
		//hostList = null;
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;

		MasterServer.RequestHostList(typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		Network.Instantiate(pioneerPrefab, new Vector3(-11f, 1f, -14f), Quaternion.identity, 0);
	}

	// Use this for initialization
	void Start () {	
		//MasterServer.RequestHostList(typeName);
		Network.sendRate = 100;
		path = Application.dataPath;
		Application.runInBackground = true;
		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			OperatingSystem = "Windows";	
	}
	
	// Update is called once per frame
	void Update () {
		if (MasterServer.PollHostList ().Length != 0) {
						HostData[] hostData = MasterServer.PollHostList ();
						for (int i = 0; i < hostData.Length; i++)
								UnityEngine.Debug.Log ("Game name: " + hostData [i].gameName);
						MasterServer.ClearHostList ();
				} 
	}

	void OnDestroy()
	{

		if(ServerProcess != null)
			ServerProcess.Kill ();
	}
}
