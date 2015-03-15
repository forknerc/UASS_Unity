using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpdatePositionScript : MonoBehaviour {
	public Text Current_X, Current_Y, Current_Z, Current_Yaw;
	public InputField Actual_X, Actual_Y, Actual_Z, Actual_Yaw;
	public GameObject Manager;
	private SelectionMgr selectionMgr;
	private InputMgr inputMgr;
	private CommandMgr commandMgr;
	public KeyCode toggleKey = KeyCode.T;

	private Vector3 DesiredPosition, CurrentPosition;
	private float DesiredYaw, CurrentYaw;

	// Use this for initialization
	void Start () {
		selectionMgr = Manager.GetComponent<SelectionMgr>();
		inputMgr = Manager.GetComponent<InputMgr>();
		commandMgr = Manager.GetComponent<CommandMgr>();
	}
	
	// Update is called once per frame
	void Update () {
		if(selectionMgr.selectedUnits.Count > 0)
		{
			GameObject unit = selectionMgr.selectedUnits[0];
			CurrentPosition = new Vector3((float)Math.Round(unit.transform.position.x, 2), (float)Math.Round(unit.transform.position.y, 2), (float)Math.Round(unit.transform.position.z, 2));
			CurrentYaw = Math.Round(unit.transform.rotation.y, 2);

			Current_X.text = CurrentPosition.x.ToString();
			Current_Y.text = CurrentPosition.y.ToString();
			Current_Z.text = CurrentPosition.z.ToString();
			Current_Yaw.text = Math.Round(unit.transform.rotation.y, 2).ToString();
		}
		else
		{
			Current_X.text = "0";
			Current_Y.text = "0";
			Current_Z.text = "0";
			Current_Yaw.text = "0";

		}

		if (Input.GetKeyDown(toggleKey)) {
			Actual_X.text = Math.Round(inputMgr.RayCastPoint.x, 2).ToString();
			Actual_Y.text = Math.Round(inputMgr.RayCastPoint.y, 2).ToString();
			Actual_Z.text = Math.Round(inputMgr.RayCastPoint.z, 2).ToString();
		}

	}

	public void UpdateUnitPosAndOri()
	{
		if(Actual_X.text == "")
			Actual_X.text = "0";
		if(Actual_Y.text == "")
			Actual_Y.text = "0";
		if(Actual_Z.text == "")
			Actual_Z.text = "0";
		if(Actual_Yaw.text == "")
			Actual_Yaw.text = "0";

		DesiredPosition = new Vector3(float.Parse(Actual_X.text), float.Parse(Actual_Y.text), float.Parse(Actual_Z.text));
		DesiredYaw = float.Parse (Actual_Yaw.text);

		Vector3 PositionOffset = DesiredPosition - CurrentPosition;
		float YawOffset = DesiredYaw - CurrentYaw;
		if(selectionMgr.selectedUnits.Count > 0)
			commandMgr.SetOffset(selectionMgr.selectedUnits[0], PositionOffset, YawOffset);
	}



}
