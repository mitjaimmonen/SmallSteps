using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {

	[FMODUnity.EventRef] public string starGameSound, selectionSound, quitGameSound;
	public Image overlay;
	public GameObject startBtn;
	public Animator anim;
	public SoundBehaviour sound;
	GameObject chosenItem;
	Transform cameraTrans;

	void Awake()
	{
		if (!anim)
			anim = GetComponentInChildren<Animator>();
        
		Time.timeScale = 1;
		cameraTrans = GameObject.Find("Main Camera").transform;
	}
	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(startBtn);
			chosenItem = startBtn;
		}
		if (Input.GetButton("Submit"))
		{
			Invoke("SetScrolled", 0.2f);
		}
		if (EventSystem.current.currentSelectedGameObject != chosenItem)
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				FMODUnity.RuntimeManager.PlayOneShot(selectionSound, cameraTrans.position);
				chosenItem = EventSystem.current.currentSelectedGameObject;

			}
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
		{
			FMODUnity.RuntimeManager.PlayOneShot(quitGameSound, cameraTrans.position);
			StartCoroutine(FadeOut(null));
		}
	}

	IEnumerator FadeOut(string levelName)
	{
		float time = 0;
		sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundBehaviour>();

		if (levelName != null)
		{
			sound.StartingGame();
			FMODUnity.RuntimeManager.PlayOneShot(starGameSound, cameraTrans.position);
		}

		while (time <= 1f)
		{
			overlay.color = Color.Lerp(overlay.color, Color.black, time);
			time += Time.deltaTime;
			yield return null;
		}
		if (levelName != null)
			SceneManager.LoadScene(levelName);
		else
			Application.Quit();

		yield break;
	}
}
