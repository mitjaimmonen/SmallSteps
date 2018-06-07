using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundBehaviour : MonoBehaviour {

	[FMODUnity.EventRef] public string backgroundMusic;
	FMOD.Studio.EventInstance musicEI;

	GameObject targetCamera; //Attach music to camera if possible

	float timer;
	void Awake()
	{
		if (GameObject.FindGameObjectsWithTag("SoundManager").Length > 1)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this);
			SceneManager.sceneLoaded += LoadScene;

			targetCamera = GameObject.Find("Main Camera");


			musicEI = FMODUnity.RuntimeManager.CreateInstance(backgroundMusic);
			musicEI.setParameterValue("isGame", SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1);
			
			musicEI.start();
			

			if (!targetCamera)
				FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEI, GetComponent<Transform>(), GetComponent<Rigidbody>());
			else
				FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEI, targetCamera.GetComponent<Transform>(), targetCamera.GetComponent<Rigidbody>());
		}
	}
	void OnDisable() {
		SceneManager.sceneLoaded -= LoadScene;
	}

	public void StartingGame()
	{
		musicEI.setParameterValue("isGame", SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1);
	}

	void LoadScene(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Scene Loaded.");
		targetCamera = GameObject.Find("Main Camera");		
		
		musicEI.setParameterValue("isGame", SceneManager.GetActiveScene().buildIndex == 1 ? 1 : 0);

		FMOD.Studio.PLAYBACK_STATE playbackState;
		musicEI.getPlaybackState(out playbackState);
		if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
			musicEI.start();

		if (!targetCamera)
 			FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEI, GetComponent<Transform>(), GetComponent<Rigidbody>());
		else
 			FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEI, targetCamera.GetComponent<Transform>(), targetCamera.GetComponent<Rigidbody>());
	}
		
	void Update()
	{
		timer = Time.time;

		if (targetCamera)
		{
			transform.position = targetCamera.transform.position;
		}
	}
}
