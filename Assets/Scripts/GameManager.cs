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

    private int _sphereSpawnCount;


    void Start()
    {
        StateHandler = GetComponent<GameStateHandler>();


        if (instance == null) { instance = this; }

    }

    public void StartGame()
    {
        StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
        player = Instantiate(_playerPrefab, spawnPosTransform.transform.position, Quaternion.identity);
        player.GetComponent<FPSController>().cameraObject = Camera.main.gameObject;
        Camera.main.transform.parent = player.transform;
        Camera.main.transform.localPosition = new(0, 0.5f, 0);
        SpawnSphereEntities(_sphereSpawnCount);
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
        EntitiesInWorld = new();
        StateHandler.CurrentState = GameStateHandler.GameState.InMenu;

        Camera.main.transform.parent = null;

        Camera.main.transform.position = Vector3.zero;

        Destroy(player);
        player = null;
    }

    public void SpawnSphereEntities(int count)
    {
        Vector3 pos = new();
        for(int i = 0; i < count; i++) 
        {
            pos = new Vector3(Random.Range(-20, 20), Random.Range(3, 10), Random.Range(-20, 20));
            GameObject s = Instantiate(sphere, pos, Quaternion.identity);
            EntitiesInWorld.Add(s);
        }
    }
}
