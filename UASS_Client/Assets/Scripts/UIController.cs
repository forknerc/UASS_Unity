using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	
	public GameObject pauseMenu;
	public GameObject Manager;
	public KeyCode toggleKey = KeyCode.BackQuote;

	public GameObject[] List; 

	void Update()
	{
		bool temp = false;
		foreach(GameObject panel in List)
		{
			if(panel.activeSelf)
				temp = true;
		}

		// Enable pause menu
		if (Input.GetKeyDown(toggleKey))
		{
			if(temp == false)
				pauseMenu.SetActive(!pauseMenu.activeSelf);
		}
	}

	void Start()
	{
	}
	
}