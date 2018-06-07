using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {

	public Image overlay;
	public GameObject startBtn;
	public Animator anim;
	public SoundBehaviour sound;

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
			Invoke("SetScrolled", 0.2f);
		}
	}

	public void SetScrolled()
	{
		anim.SetBool("isScrolled", true);

	}

	public void LoadLevel(string levelName)
	{
		if (anim.GetBool("isScrolled"))
			StartCoroutine(FadeOut(levelName));
	}

	public void QuitApplication()
	{
		if (anim.GetBool("isScrolled"))		
			Application.Quit();
	}

	IEnumerator FadeOut(string levelName)
	{
		float time = 0;

		while (time <= 1f)
		{
			overlay.color = Color.Lerp(overlay.color, Color.black, time);
			time += Time.deltaTime;
			yield return null;
		}
		sound.StartingGame();
		SceneManager.LoadScene(levelName);

		yield break;
	}
}
