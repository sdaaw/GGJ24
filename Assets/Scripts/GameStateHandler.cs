using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        Intro,
        GeneratingJokes,
        ChoosingJoke,
        BattlePrepare,
        WaveDefeated,
        PlayerDeath
    }

    [SerializeField]
    private GameObject _mainMenuCanvasParent;

    [SerializeField]
    private GameObject _pausedCanvasParent;

    [SerializeField]
    private GameObject _jokeChoosingPanel;

    [SerializeField]
    private GameObject _inPlayPanel;

    [SerializeField]
    private TMP_Text _playerInfo;

    [SerializeField]
    private GameObject _deathPanel;

    public GameState CurrentState;
    private GameState _previousState { get; set; }

    public GameState startingScene;

    [SerializeField]
    private AudioSource _audioSource;

    private bool _quizbgmPlayed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        _previousState = GameState.None;
        CurrentState = startingScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInfo != null && CurrentState == GameState.InPlay)
        {
            _playerInfo.text = $"HP: { GameManager.instance.player.GetComponent<Entity>().CurrentHealth }\n" +
                        $"Power: { GameManager.instance.MoneyReward }";
        }

        CheckStates();
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            CurrentState = (CurrentState == GameState.Paused) ? GameState.InPlay : GameState.Paused;
        }*/

        /*if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentState = (CurrentState == GameState.BattlePrepare) ? GameState.InPlay : GameState.BattlePrepare;
            FindFirstObjectByType<FPSController>().ResetCameraPosition();
        }*/
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
                if (_previousState == GameState.Paused) return;
                GameManager.instance.player.GetComponent<FPSController>().cameraObject.transform.position = GameManager.instance.player.transform.position;
                GameManager.instance.player.GetComponent<FPSController>().freezeControls = false;
                GameManager.instance.player.GetComponent<FPSController>().viewmodelObject.SetActive(true);
                GameManager.instance.SpawnNextWave();

                _audioSource.Play();

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
                _audioSource.Pause();
                if (!_quizbgmPlayed) SoundManager.PlayASource("quizbgm");
                _quizbgmPlayed = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            }
            case GameState.BattlePrepare:
            {
                _quizbgmPlayed = false;
                GameManager.instance.SpawnPlayer();
                GameManager.instance.player.GetComponent<FPSController>().viewmodelObject.SetActive(false);
                break;
            }
            case GameState.Intro:
            {
                DialogueManager.instance.introSceneRunning = true;
                break;
            }
            case GameState.WaveDefeated:
            {
                // Debug.Log(GameManager.instance.enemyController.nextWaveIndex);
                if (GameManager.instance.waveIndex >= GameManager.instance.waveAmount)
                {
                    Application.LoadLevel("EndScene");
                }

                GameManager.instance.WaveStarted = false;
                GameManager.instance.enemyController.currentWave = null;
                GameManager.instance.player.GetComponent<FPSController>().viewmodelObject.SetActive(false);
                GameManager.instance.DespawnPlayer();
                CurrentState = GameState.GeneratingJokes;
                break;
            }
            case GameState.PlayerDeath:
            {
                break;
            }
        }
        _pausedCanvasParent.SetActive(CurrentState == GameState.Paused);
        _mainMenuCanvasParent.SetActive(CurrentState == GameState.InMenu);
        _jokeChoosingPanel.SetActive(CurrentState == GameState.ChoosingJoke);
        _inPlayPanel.SetActive(CurrentState == GameState.InPlay);
        _deathPanel.SetActive(CurrentState == GameState.PlayerDeath);

    }

    //this is for menu button XD
    public void SetToPlay()
    {
        CurrentState = GameState.InPlay;
    }
}
