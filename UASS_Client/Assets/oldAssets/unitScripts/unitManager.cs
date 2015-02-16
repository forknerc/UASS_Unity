using UnityEngine;
using System.Collections;

public class unitManager : MonoBehaviour {


	public ArrayList units;

	// Use this for initialization
	void Start () {

		units = new ArrayList();
		//GameObject newObj = GameObject.Find("droneUnit1");
		units.Add(GameObject.Find("droneUnit1"));
		Debug.Log("all units");
		
		foreach(GameObject unit in units) {
			Debug.Log(unit.ToString());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
