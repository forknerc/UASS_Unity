using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Net;
using System.Collections.Generic;

/*Command message format
MSGType	ID	CmdType	CmdSpecificData 
CmdSpesificData:
CmdType 0:	desired XYZ, desired orientation
CmdType 1:	none
CmdType 2:	none (can only be performed on a drone)
CmdType 3:	none (can only be performed on a drone)

GOTO Command:
		2 ID 0 X Y Z dX dY dZ
*/



public class CommandMgr : MonoBehaviour {
	UdpClient client = new UdpClient();


	private string PosAndOriToString(Vector3 DesiredPos, Vector3 DesiredOri)
	{
		return DesiredPos.x + " " + DesiredPos.y + " " + DesiredPos + " " + DesiredOri.x + " " + DesiredOri.y + " " + DesiredOri;
	}

	private string PosToString(Vector3 DesiredPos)
	{
		return DesiredPos.x + " " + DesiredPos.y + " " + DesiredPos;
	}

	public void SendGoToCommand(GameObject Unit, Vector3 DesiredPos, Vector3 DesiredOri)
	{
		Unit stats = (Unit)Unit.GetComponent("Unit");
		string cmdMsg = 2 + " " + stats.ID + " " + 0 + " " + PosAndOriToString(DesiredPos, DesiredOri);
		//Send to robot with stats.ipAddress and stats.port
		Debug.Log ("Sending command: " + cmdMsg);
		SendMessage (stats.IPAddress, stats.Port, cmdMsg);
	}

	public void SendGoToCommand(List<GameObject> Units, Vector3 DesiredPos, Vector3 DesiredOri)
	{
		foreach(GameObject unit in Units)
		{
			Unit stats = (Unit)unit.GetComponent("Unit");
			string cmdMsg = 2 + " " + stats.ID + " " + 0 + " " + PosAndOriToString(DesiredPos, DesiredOri);
			//Send to robot with stats.ipAddress and stats.port
			Debug.Log ("Sending command: " + cmdMsg);
			SendMessage (stats.IPAddress, stats.Port, cmdMsg);
		}
		
	}

	public void Move(GameObject Unit, Vector3 DesiredPos)
	{
		Unit stats = (Unit)Unit.GetComponent("Unit");
		string cmdMsg = 2 + " " + stats.ID + " " + 0 + " " + PosToString(DesiredPos);
		//Send to robot with stats.ipAddress and stats.port
		Debug.Log ("Sending command: " + cmdMsg);
		SendMessage (stats.IPAddress, stats.Port, cmdMsg);
	}
	
	public void Move(List<GameObject> Units, Vector3 DesiredPos)
	{
		foreach(GameObject unit in Units)
		{
			Unit stats = (Unit)unit.GetComponent("Unit");
			string cmdMsg = 2 + " " + stats.ID + " " + 0 + " " + PosToString(DesiredPos);
			//Send to robot with stats.ipAddress and stats.port
			Debug.Log ("Sending command: " + cmdMsg);
			SendMessage (stats.IPAddress, stats.Port, cmdMsg);
		}
		
	}


	void SendMessage(string ipAddress, int port, string msg)
	{
		byte[] data = Encoding.UTF8.GetBytes(msg);
		IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
		client.Send(data, data.Length, remoteEndPoint);
	}

	// Use this for initialization
	void Start () {
		SendMessage ("127.0.0.1", 8051, "2 0 1 1 1");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
