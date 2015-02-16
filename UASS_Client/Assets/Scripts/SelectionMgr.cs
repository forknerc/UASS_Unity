using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionMgr : MonoBehaviour {
	List<GameObject> selectedUnits;
	
	// Use this for initialization
	void Start () {
		selectedUnits = new List<GameObject>();
	}

	void AddUnitToSelectedList(GameObject unit)
	{
		selectedUnits.Add(unit);
	}

	void RemoveUnitFromSelectedList(GameObject unit)
	{
		selectedUnits.Remove(unit);
	}

	void RemoveAllUnits()
	{

	}

	// Update is called once per frame
	void Update () {
	
	}
}
