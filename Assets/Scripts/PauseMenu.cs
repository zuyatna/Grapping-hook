﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject pauseMenuUi;
    public GameObject menuUi;
    public GameObject settingMenu;
    public GameObject resolutionMenu;
    public GameObject loadingScreen;

    public Slider slider;

    private int _sceneIndex;

    private bool _isMainMenu = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isMainMenu)
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;
    }

    private void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0;
        GameIsPause = true;
    }

    public void ResetScene()
    {
        GameManager.Manager.isLoadScene = false;
        
        Time.timeScale = 1;
        GameIsPause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        _isMainMenu = false;
        
        StartCoroutine(LoadAsynchronously(sceneIndex));
        Time.timeScale = 1;
        GameIsPause = false;
    }

    public void LoadToMainMenu()
    {
        _isMainMenu = true;
        StartCoroutine(LoadAsynchronously(0));
    }
    
    public void LoadFromData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        _sceneIndex = data.saveSceneIndex;
        
        Debug.Log("scene: " + _sceneIndex);
        
        StartCoroutine(LoadAsynchronously(_sceneIndex));
        Time.timeScale = 1;
        GameIsPause = false;
    }
    
    public void LoadSlot(string slot)
    {
        SaveSystem.LoadSlot = "/hook" + slot + ".fun";

        GameManager.Manager.isLoadScene = true;
        GameManager.Manager.slot = slot;
        
        _isMainMenu = false;
        
        LoadFromData();
    }

    public void ActivePauseMenu()
    {
        if (_isMainMenu)
        {
            menuUi.SetActive(true);
            settingMenu.SetActive(false);
            resolutionMenu.SetActive(false); 
        }
        else
        {
            pauseMenuUi.SetActive(true);
            settingMenu.SetActive(false);
            resolutionMenu.SetActive(false);   
        }
    }

    public void ActiveSettingMenu()
    {
        pauseMenuUi.SetActive(false);
        settingMenu.SetActive(true);
        resolutionMenu.SetActive(false);
    }
    
    public void ActiveResolutionMenu()
    {
        pauseMenuUi.SetActive(false);
        settingMenu.SetActive(false);
        resolutionMenu.SetActive(true);
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
        
        loadingScreen.SetActive(false);
    }
}
