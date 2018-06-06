using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {

	public GameObject startBtn;
	public Animator anim;

	void Awake()
	{
		if (!anim)
			anim = GetComponentInChildren<Animator>();
	}
	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(startBtn);
		}
		if (Input.GetButton("Submit"))
		{
			SetScrolled();
		}
	}

	public void SetScrolled()
	{
		anim.SetBool("isScrolled", true);

	}

	public void LoadLevel(string levelName)
	{
		if (anim.GetBool("isScrolled"))
			SceneManager.LoadScene(levelName);
	}

	public void QuitApplication()
	{
		if (anim.GetBool("isScrolled"))		
			Application.Quit();
	}
}
