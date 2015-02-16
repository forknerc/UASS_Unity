using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMgr : MonoBehaviour {
	public List<GameObject> myUnits;
	public List<GameObject> allUnits;
	public GameObject QuadCopterPrefab;
	public GameObject PioneerPrefab;

	// Use this for initialization
	void Start () {
		myUnits = new List<GameObject>();
		allUnits = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Assumes robot has requested connection and is waiting for acknowledgement
	//This function is called once the "Add Robot" button is clicked. 
	public string AddUnit(Unit unitStats)
	{
		GameObject newUnit = new GameObject();
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

		// TODO add owner ID in here

		Unit stats = (Unit)newUnit.GetComponent("Unit");
		stats.CopyAttributes(unitStats);

		myUnits.Add(newUnit);
		allUnits.Add(newUnit);

		//Send robot its unique ID 

		// return owner ID for unitConnection manager to send confirmation message
		return stats.ID;
	}



}
