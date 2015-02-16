using UnityEngine;
using System.Collections;

public class input : MonoBehaviour {

	private Vector3 pos;

	public GameObject sMgr;
	public selectionManager sMgrS;

	public GameObject nMgr;
	public UDPsend sendUDP;

	// Use this for initialization
	void Start () {
	
		pos.x = 0;
		pos.y = 0;
		pos.z = 0;

		sMgr = GameObject.Find("selectionMgr");
		sMgrS = sMgr.GetComponent<selectionManager>();

		nMgr = GameObject.Find("networkControl");
		sendUDP = nMgr.GetComponent<UDPsend>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
			{
				Ray rayL;
				RaycastHit hitL;
				rayL = Camera.main.ScreenPointToRay(Input.mousePosition);
		
				if(Physics.Raycast(rayL,out hitL,Mathf.Infinity))
					{
						if(hitL.collider.name == "droneUnit1")
							{
								Debug.Log("hit drone");
								sMgrS.addUnitToSelected(GameObject.Find("droneUnit1"));
								
							}
						else if(hitL.collider.name == "pioneer3at")
							{
						Debug.Log("hit pioneer");
						//sMgrS.addUnitToSelected(GameObject.Find("pioneer3at"));
						
							}
						else 
							{
								Debug.Log("hit ground");
								sMgrS.deselectAll();
							}
						pos = hitL.point;
					}
			}
		if(Input.GetMouseButtonDown(1))
			{
				Ray rayR;
				RaycastHit hitR;
				rayR = Camera.main.ScreenPointToRay(Input.mousePosition);
				Physics.Raycast(rayR,out hitR,Mathf.Infinity);
				if(sMgrS.unitsSelected())
					{
						pos = hitR.point;
						if(pos.x < 1.8 && pos.x > -1.8 && pos.z < 1.0 && pos.z > -1.0)
							{
								foreach(GameObject unit in sMgrS.selectedUnits)
									{
										unit.GetComponent<droneUnit>().sendCommand(pos);
									}
							}
						else 
							{
								Debug.Log("move out of bounds");
							}
					}
				else
					{
						Debug.Log("no units selected to move");
					}		
			}
		}
}
