using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UASS.unitInfoStruct;

public class Unit : MonoBehaviour{ 
	public GameObject SelectionCircle;


	public string id;
	public string ID
	{
		get{return id;}
		set{id = value;}
	}

	public Vector3 Position;
	public Vector3 Orientation;
	public Vector3 OrientationOffset;
	public Vector3 PositionOffset;

	public int UnitType;
	public string Owner;
	
	public string IPAddress;
	public int Port;

	public bool IsAerial = false;

	public bool isSelected;
	public bool IsSelected
	{
		get{return isSelected;}
		set
		{
			isSelected = value;
			if(SelectionCircle != null)
				SelectionCircle.SetActive(value);
		}
	}


	public void CopyAttributes(newRobotInfo U)
	{
		IsSelected=U.IsSelected;
		Owner=U.Owner;
		UnitType=U.UnitType;
		Position=U.Position;
		Orientation=U.Orientation;
		IPAddress=U.IPAddress;
		Port=U.Port;
		PositionOffset = U.PositionOffeset;
		OrientationOffset = U.OrientationOffset;
	}

	void Start()
	{
		SelectionCircle = transform.FindChild("selectionCircle").gameObject;
		IsSelected = false;
	}

}