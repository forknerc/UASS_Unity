using UnityEngine;
using System.Collections;

public class selectionManager : MonoBehaviour {

	public ArrayList selectedUnits;

	// Use this for initialization
	void Start () {

		selectedUnits = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
		foreach(GameObject unit in selectedUnits)
		{
			if(unit.name == "droneUnit1")
			{
				unit.GetComponent<droneUnit>().isSelected = true;
			}
			//else if(unit.name == "pioneer3at")
			//{
			//	unit.GetComponent
			//}
		}

	}

	public void addUnitToSelected(GameObject unit){

		selectedUnits.Add(unit);
	}

	public void deselectAll() {

		foreach(GameObject unit in selectedUnits)
		{
			unit.GetComponent<droneUnit>().isSelected = false;
		}

		selectedUnits.Clear();
	}

	public bool unitsSelected() {

		int count = selectedUnits.Count;
		if(count > 0)
			{
				return true;
			}
		return false;
	}
}
