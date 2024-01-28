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

    void Start()
    {
        StateHandler = GetComponent<GameStateHandler>();


        if (instance == null) { instance = this; }

        if(StateHandler.startingScene == GameStateHandler.GameState.InPlay)
        {
            SpawnPlayer();
        }

    }

    public void StartGame()
    {
        StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        player = Instantiate(_playerPrefab, spawnPosTransform.transform.position, spawnPosTransform.transform.rotation);
        player.GetComponent<FPSController>().cameraObject = Camera.main.gameObject;
        Camera.main.transform.parent = player.transform;
        Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
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
}
