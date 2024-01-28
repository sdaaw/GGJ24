using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public enum GameState
    {
        None,
        InMenu,
        InPlay,
        Paused,
        GeneratingJokes,
        ChoosingJoke
    }

    [SerializeField]
    private GameObject _mainMenuCanvasParent;

    [SerializeField]
    private GameObject _pausedCanvasParent;

    [SerializeField]
    private GameObject _jokeChoosingPanel;

    [SerializeField]
    private GameObject _arenaHud;

    public GameState CurrentState { get; set; }
    private GameState _previousState { get; set; }

    public GameState startingScene;

    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _previousState = GameState.None;
        CurrentState = startingScene;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStates();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CurrentState = (CurrentState == GameState.Paused) ? GameState.InPlay : GameState.Paused;
        }
    }

    public void CheckStates()
    {
        if (_previousState == CurrentState) return;
        _previousState = CurrentState;
        switch (CurrentState)
        {
            case GameState.InMenu:
            {
                break;
            }
            case GameState.InPlay:
            {
                Cursor.lockState = CursorLockMode.Locked;
                break;
            }
            case GameState.Paused:
            {
                Cursor.lockState = CursorLockMode.None;
                break;
            }
            case GameState.ChoosingJoke:
            {
                Cursor.lockState = CursorLockMode.None;
                break;
            }
        }
        _pausedCanvasParent.SetActive(CurrentState == GameState.Paused);
        _mainMenuCanvasParent.SetActive(CurrentState == GameState.InMenu);
        _jokeChoosingPanel.SetActive(CurrentState == GameState.ChoosingJoke);
    }

    //this is for menu button XD
    public void SetToPlay()
    {
        CurrentState = GameState.InPlay;
    }
}
