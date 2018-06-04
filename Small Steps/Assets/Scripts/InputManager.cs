using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure; // Required in C#

public class InputManager : MonoBehaviour {
		public PlayerInputController playerInput;
        bool playerIndexSet = false;
        PlayerIndex playerIndex;
        GamePadState state;
        GamePadState prevState;
        [HideInInspector]public PauseMenuHandler pauseMenuHandler;
	// Use this for initialization

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("InputManager").Length > 1)
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this);
    }
	void Start () {
		if (!playerInput)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
                playerInput = go.GetComponent<PlayerInputController>();
        }
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
        if (!playerInput)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
                playerInput = go.GetComponent<PlayerInputController>();
        }
	}

    public void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
        playerInput.HandleInput(state, prevState);
        

    }

}
