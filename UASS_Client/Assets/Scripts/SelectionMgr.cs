using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SelectionMgr : MonoBehaviour {
	public List<GameObject> selectedUnits;
	
	// Use this for initialization
	void Start () {
		selectedUnits = new List<GameObject>();
	}

	public void AddUnitToSelectedList(GameObject unit)
	{
		selectedUnits.Add(unit);
		Unit stats = (Unit)unit.GetComponent("Unit");
		stats.IsSelected = true;
	}

	public void AddUnitsToSelectedList(List<GameObject> units)
	{
		foreach(GameObject unit in units)
		{
			selectedUnits.Add(unit);
			Unit stats = (Unit)unit.GetComponent("Unit");
			stats.IsSelected = true;
		}
	}

	public void RemoveUnitFromSelectedList(GameObject unit)
	{
		selectedUnits.Remove(unit);
		Unit stats = (Unit)unit.GetComponent("Unit");
		stats.IsSelected = false;
	}

	public void RemoveAllUnits()
	{
		foreach(GameObject unit in selectedUnits)
		{
			Unit stats = (Unit)unit.GetComponent("Unit");
			stats.IsSelected = false;
		}
		selectedUnits.Clear ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
