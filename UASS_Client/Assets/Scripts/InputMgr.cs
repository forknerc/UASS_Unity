using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputModes
{
	public const int RegularMode = 0;
	public const int SetPositionMode = 1;
	public const int MenuMode = 2;
}



public class InputMgr : MonoBehaviour {
	public int InputMode = 0;
	public float distance = 5.0f;
	Ray ray;
	Vector3 point;
	public Vector3 pos;
	private SelectionMgr selectionMgr;
	private CommandMgr commandMgr;
	public bool MenuActive;

	public GameObject UpdatePositionPanel;
	public GameObject MenuPanel;

	public KeyCode LaunchShortcut;
	public KeyCode LandShortcut;



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

		LaunchShortcut = KeyCode.G;
		LandShortcut = KeyCode.H;

	}

	void CheckMode()
	{
		if(UpdatePositionPanel.activeSelf)
		{
			InputMode = InputModes.SetPositionMode;
		}
		else if(MenuPanel.activeSelf)
		{
			InputMode = InputModes.MenuMode;
		}
		else
		{
			InputMode = InputModes.RegularMode;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		//Check game to see which input mode we should be in (Regular, UpdatePosition, etc.)
		CheckMode();
		switch(InputMode)
		{
		case InputModes.MenuMode:
			break;
		case InputModes.SetPositionMode:
			SetPositionModeUpdate();
			break;
		case InputModes.RegularMode:
			RegularModeUpdate();
			break;
		default:
			break;
		}
	}

	void SetPositionModeUpdate()
	{
		//Create a raycast.
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		point = ray.origin + (ray.direction * distance);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit)) {
			// Create a particle if hit
			pos=hit.point;
		}

		if(Input.GetMouseButtonDown(0))
		{
			// if the ray hits something
			if(Physics.Raycast(ray,out hit,Mathf.Infinity))
			{
				if(hit.collider.tag == "Unit")
				{
					GameObject unit = hit.transform.gameObject;
					selectionMgr.RemoveAllUnits();
					selectionMgr.AddUnitToSelectedList(unit);
				}
			}	
		}
	}

	void RegularModeUpdate()
	{
		if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
			//User is interacting with GUI, not the world. 
			//Don't create a raycast
		}
		else
		{
			//Check Keypresses
			CheckKeyPresses();

			if(Input.GetMouseButtonDown(0))
			{
				//Create a raycast.
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				point = ray.origin + (ray.direction * distance);
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast (ray, out hit)) {
					// Create a particle if hit
					pos=hit.point;
				}

				// if the ray hits something
				if(Physics.Raycast(ray,out hit,Mathf.Infinity))
				{
					//Debug.Log(hitL.collider.tag);
					
					if(hit.collider.tag == "Unit")
					{
						GameObject unit = hit.transform.gameObject;
						
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
					else if(Input.GetKey(KeyCode.LeftControl))
					{
						selectionMgr.RemoveAllUnits();
					}
					else
					{
						selectionMgr.RemoveAllUnits();
					}
					pos = hit.point;
				}	
			}
			
			// right click
			if(Input.GetMouseButtonDown(1))
			{	
				//Create a raycast.
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				point = ray.origin + (ray.direction * distance);
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast (ray, out hit)) {
					// Create a particle if hit
					pos=hit.point;
				}

				// if a unit is selected
				if(selectionMgr.selectedUnits.Count>0)
				{
					// find target position
					pos = hit.point;
					commandMgr.GoTo(selectionMgr.selectedUnits, pos);
				}
				else
				{
					Debug.Log("no units selected to move");
				}		
			}
		}
	}


	public void CheckKeyPresses()
	{
		if(Input.anyKeyDown)
		{
			Debug.Log (Input.inputString);
		}
		if (Input.GetKeyDown(LaunchShortcut))
		{
			commandMgr.Launch(selectionMgr.selectedUnits);
		}
		else if(Input.GetKeyDown(LandShortcut))
		{
			commandMgr.Land(selectionMgr.selectedUnits);
		}

	}
}
