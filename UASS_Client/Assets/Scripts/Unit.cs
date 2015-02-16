using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Unit : MonoBehaviour{
	private string id;
	public string ID
	{
		get{return id;}
		set{id = value;}
	}

	public Vector3 Position;
	public Vector3 Orientation;
	public int UnitType;
	public string Owner;
	
	public string IPAddress;
	public int Port;


	private bool isSelected;
	public bool IsSelected
	{
		get{return isSelected;}
		set
		{
			isSelected = value;
		}
	}


	public void CopyAttributes(Unit U)
	{
		IsSelected=U.IsSelected;
		Owner=U.Owner;
		UnitType=U.UnitType;
		Position=U.Position;
		Orientation=U.Orientation;
		IPAddress=U.IPAddress;
		Port=U.Port;
	}

}