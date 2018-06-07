using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
public class PauseMenuHandler : MonoBehaviour
{

    public GameObject Container;
    public Button resumeBtn;
    public Button mainMenuBtn;
    InputManager input;
    bool isPaused;
    bool pause;
    PlayerMachine player;
    void Awake()
    {
        var tags = GameObject.FindGameObjectsWithTag("Player");
        foreach (var tag in tags)
        {
            player = tag.GetComponent<PlayerMachine>();
            if (player)
                break;
        }
        input = GameObject.Find("InputManager").GetComponent<InputManager>();
        input.pauseMenuHandler = this;

        Container.SetActive(false);
    }

    void Update()
    {
        if (player.CurrentHealth < 0)
        {
            Debug.Log("Dead");
            Container.SetActive(true);
            isPaused = Container.activeSelf;
            resumeBtn.gameObject.SetActive(false);
            if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject == resumeBtn.gameObject)
                EventSystem.current.SetSelectedGameObject(mainMenuBtn.gameObject);
        }
        else
        {
            resumeBtn.gameObject.SetActive(true);
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(resumeBtn.gameObject);
        }
    }

    public void HandleInput(GamePadState state, GamePadState prevState)
    {
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {
            if (!pause)
            {
                pause = true;
                Container.SetActive(!Container.activeSelf);
                isPaused = Container.activeSelf;                
                
                Time.timeScale = 0;
              
            }
            else
            {
                ResumeGame();
            }

          
        }
    }

    public void ChangeScene(string name)
    {
        Time.timeScale = 1;        
        SceneManager.LoadScene(name);
    }
    public void ResumeGame()
    {
        pause = false;
        Container.SetActive(!gameObject.activeSelf);
        isPaused = Container.activeSelf;
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
