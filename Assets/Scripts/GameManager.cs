using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _playerPrefab;

    [HideInInspector]
    public GameObject player;

    [SerializeField]
    private GameObject spawnPosTransform;


    [SerializeField]
    private GameObject sphere;

    public static GameManager instance;

    public List<GameObject> EntitiesInWorld = new List<GameObject>();

    public GameStateHandler StateHandler;

    [SerializeField]
    public Transform cameraCinematicPosition;

    [SerializeField]
    public Transform cameraCinematicLookAtPosition;


    public int MoneyReward;

    public EnemyController enemyController;

    public int waveIndex;

    public bool WaveStarted;

    void Start()
    {
        StateHandler = GetComponent<GameStateHandler>();
        enemyController = GetComponent<EnemyController>();  

        if (instance == null) { instance = this; }
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPosTransform.transform.position;
    }

    public void StartGame()
    {
        StateHandler.CurrentState = GameStateHandler.GameState.Intro;
    }

    private void Update()
    {
        if(enemyController.currentWave != null && enemyController.currentWave.enemiesRemaining == 0 && WaveStarted)
        {
            StateHandler.CurrentState = GameStateHandler.GameState.WaveDefeated;
        }
    }

    public void SpawnPlayer()
    {
        player = Instantiate(_playerPrefab, spawnPosTransform.transform.position, spawnPosTransform.transform.rotation);
        player.GetComponent<FPSController>().cameraObject = Camera.main.gameObject;
        Camera.main.transform.parent = player.transform;
        Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    public void DespawnPlayer()
    {
        player.GetComponent<FPSController>().cameraObject.transform.SetParent(null);
        Destroy(player.GetComponent<FPSController>().viewmodelObject);
        Destroy(player);
    }

    public void QuitGameToMenu()
    {
        if(EntitiesInWorld.Count > 0) 
        { 
            for(int i = 0; i <  EntitiesInWorld.Count; i++) 
            {
                Destroy(EntitiesInWorld[i]);
            }
        }
        EntitiesInWorld = new List<GameObject>();
        StateHandler.CurrentState = GameStateHandler.GameState.InMenu;

        Camera.main.transform.parent = null;

        Camera.main.transform.position = Vector3.zero;
        Destroy(player);
        player = null;
    }

    public void SpawnNextWave()
    {
        enemyController.SpawnWave(enemyController.enemyWaves[waveIndex]);
        enemyController.currentWave = enemyController.enemyWaves[waveIndex];
        WaveStarted = true;
        waveIndex++;
    }

    public void LoadLeven(string lvlName)
    {
        Application.LoadLevel(lvlName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
