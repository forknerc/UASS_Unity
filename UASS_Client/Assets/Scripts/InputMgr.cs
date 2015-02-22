using UnityEngine;
using System.Collections;

public class InputMgr : MonoBehaviour {
	public float distance = 5.0f;
	Ray ray;
	Vector3 point;
	private Vector3 pos;
	public SelectionMgr selectionMgr;
	public CommandMgr commandMgr;

	// Use this for initialization
	void Start () {
		selectionMgr = GetComponent<SelectionMgr>();
		commandMgr = GetComponent<CommandMgr>();
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		point = ray.origin + (ray.direction * distance);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit)) {
			// Create a particle if hit
			pos=hit.point;
		}

		if(Input.GetMouseButtonDown(0))
		{
			// raycast that point
			Ray rayL;
			RaycastHit hitL;
			rayL = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// if the ray hits something
			if(Physics.Raycast(rayL,out hitL,Mathf.Infinity))
			{
				Debug.Log(hitL.transform.gameObject.tag);

				if(hitL.collider.tag == "Unit")
				{
					GameObject unit = hitL.transform.gameObject;
			
					if ( Input.GetKey(KeyCode.LeftControl) == false )
					{
						selectionMgr.RemoveAllUnits();
						selectionMgr.AddUnitToSelectedList(unit);
					}
					else
					{
						if(selectionMgr.selectedUnits.Contains(unit))
							selectionMgr.RemoveUnitFromSelectedList(unit);
						else
							selectionMgr.AddUnitToSelectedList(unit);
					}
				}
				pos = hitL.point;
			}
			else if ( Input.GetKey(KeyCode.LeftControl) == false )
				selectionMgr.RemoveAllUnits();

		}
		
		// right click
		if(Input.GetMouseButtonDown(1))
		{
			// create raycast from mouse point
			Ray rayR;
			RaycastHit hitR;
			rayR = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(rayR,out hitR,Mathf.Infinity);
			
			// if a unit is selected
			if(selectionMgr.selectedUnits.Count>0)
			{
				// find target position
				pos = hitR.point;
				
				// if in designated movement range
				//if(pos.x < 1.8 && pos.x > -1.8 && pos.z < 1.0 && pos.z > -1.0)
				//{
				commandMgr.GoTo(selectionMgr.selectedUnits, pos);
				//}
				//else 
				//{
					// click out of movement range
				//	Debug.Log("move out of bounds");
				//}
			}
			else
			{
				Debug.Log("no units selected to move");
			}		
		}




	}

	void OnGUI()
	{	
		GUI.TextField(new Rect(200, 000, 200, 25), pos.ToString());
	}

}
