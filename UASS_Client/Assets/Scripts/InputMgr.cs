using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputMgr : MonoBehaviour {
	public float distance = 5.0f;
	Ray ray;
	Vector3 point;
	private Vector3 pos;
	private SelectionMgr selectionMgr;
	private CommandMgr commandMgr;
	public bool MenuActive;


	public Vector3 RayCastPoint
	{
		get
		{
			if(pos == null)
				return new Vector3(0,0,0);
			return pos;
		}
	}


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
			//Debug.Log(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1).ToString());
			//EventSystem eventsystem;

			GameObject test = UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject;
			//	Debug.Log(UnityEngine.EventSystems.EventSystem.current);
			Debug.Log (IsPointerOverUIObject());
			//Debug.Log (test.tag.ToString());

			// raycast that point
			Ray rayL;
			RaycastHit hitL;
			rayL = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// if the ray hits something
			if(Physics.Raycast(rayL,out hitL,Mathf.Infinity))
			{
				Debug.Log(hitL.collider.tag);

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
				else if(Input.GetKey(KeyCode.LeftControl) == false && MenuActive == false)
				{
					selectionMgr.RemoveAllUnits();
				}
				pos = hitL.point;




			}


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

	/// <summary>
	/// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
	/// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
	/// </summary>
	private bool IsPointerOverUIObject() {
		// Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
		// the ray cast appears to require only eventData.position.
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		return results.Count > 0;
	}
	
	public void SetMenuActive(bool val)
	{
		MenuActive = val;
	}


}
