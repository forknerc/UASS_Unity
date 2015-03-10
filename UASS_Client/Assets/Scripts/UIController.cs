using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	
	public GameObject pauseMenu;
	public GameObject Manager;
	private InputMgr inputMgr;
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
		if(pauseMenu.activeSelf)
			temp = true;
		inputMgr.SetMenuActive(temp);
	}

	void Start()
	{
		inputMgr = Manager.GetComponent<InputMgr>();
	}
	
}