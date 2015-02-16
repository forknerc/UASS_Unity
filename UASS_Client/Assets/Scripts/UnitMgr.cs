using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitType;

public class UnitMgr : MonoBehaviour {
	public List<GameObject> myEntities;
	public List<GameObject> allEntities;
	public GameObject QuadCopterPrefab;
	public GameObject PioneerPrefab;

	// Use this for initialization
	void Start () {
		myEntities = new List<GameObject>();
		allEntities = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Assumes robot has requested connection and is waiting for acknowledgement
	//This function is called once the "Add Robot" button is clicked. 
	void AddEntity(Unit unitToAdd)
	{
		GameObject NewObject = (GameObject)Network.Instantiate(PioneerPrefab, new Vector3(0f, 4f, 0f), Quaternion.identity, 0);
		Debug.Log (NewObject.networkView.viewID);
		//Based on type, create pioneer or quad instance
		//Network.instantiate(EntityToAdd)
		//MyEntities.Add(EntityToAdd);

		//Messages sent over network to robot requesting connection
		//SendAcknowledgement(EntityToAdd)
		//SetPosition(EntityToAdd)
		//SetOrientation(EntityToAdd)
	}



}
