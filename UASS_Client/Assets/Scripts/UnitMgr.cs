﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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

	//Assumes robot has requested connection and is waiting for acknowledgement
	//This function is called once the "Add Robot" button is clicked. 
	public string AddUnit(Unit unitStats)
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
			Unit stats = (Unit)newUnit.GetComponent("Unit");
			stats.CopyAttributes(unitStats);
			stats.Owner = newUnit.networkView.viewID.ToString();
	
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
		private string id;
		public string ID
		{
			get{return id;}
			set{id = value;}
		}
		
		public Vector3 Position;
		public Vector3 Orientation;
		public int UnitType;
		public string Owner;
		
		public string IPAddress;
		public int Port;
	}
}
