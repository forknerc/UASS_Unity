using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UASS.unitInfoStruct;

public class UnitMgr : MonoBehaviour {
	public List<GameObject> myUnits;
	public List<GameObject> allUnits;
	public GameObject QuadCopterPrefab;
	public GameObject PioneerPrefab;


	// Use this for initialization
	void Start () 
	{
		myUnits = new List<GameObject>();
		allUnits = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () 
	{		

	}

	public GameObject FindUnit(string ID)
	{
		foreach(GameObject unit in myUnits)
		{
			Unit stats = (Unit)unit.GetComponent("Unit");
			if(stats.ID == ID)
			{
				return unit;
			}
		}
		return null;
	}



	//Assumes robot has requested connection and is waiting for acknowledgement
	//This function is called once the "Add Robot" button is clicked. 
	public string AddUnit(newRobotInfo unitStats)
	{
		GameObject newUnit = null;
		switch(unitStats.UnitType)
		{
		case 0:
			newUnit = (GameObject)Network.Instantiate(QuadCopterPrefab, new Vector3(0, 0, 0), Quaternion.identity, 0);
			break;
		case 1:
			newUnit = (GameObject)Network.Instantiate(PioneerPrefab, new Vector3(0, 0, 0), Quaternion.identity, 0);
			break;
		case 3:
			break;
		default:
			break;
		}

		if(newUnit != null)
		{
			Unit stats = newUnit.GetComponent<Unit>();
			stats.CopyAttributes(unitStats);
			stats.ID = (newUnit.GetComponent<NetworkView>().viewID.ToString().Split())[1];
			stats.Owner = newUnit.GetComponent<NetworkView>().viewID.owner.guid.ToString();

			Debug.Log("Owner ID: " + stats.Owner + " " + stats.ID);
	
			myUnits.Add(newUnit);
			allUnits.Add(newUnit);

			// return owner ID for unitConnection manager to send confirmation message
			return stats.ID;
		}
		return "no";
	}



}

namespace UASS.unitInfoStruct
{
	public struct newRobotInfo
	{
		public string id;
		
		public Vector3 Position;
		public Vector3 PositionOffeset;
		public Vector3 Orientation;
		public Vector3 OrientationOffset;
		public int UnitType;
		public string Owner;
		public string IPAddress;
		public int Port;
		public bool IsSelected;
	}
}
