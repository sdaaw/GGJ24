using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public readonly string[] INTRO_DIALOGUES = new string[]
    {
        "bald people are allowed to only speak with a supervisor",
        "nothing else, thank you"
    };


    public readonly string[] MOCK_DIALOGUES = new string[]
    {
        "I think my mama would have made a better joke.. HAHAHA HA H@! %A A?# H!&#@ HAH$A",
        "Mamaless behaviour.. Disappointing. *sighs*",
        ""
    };


    [SerializeField]
    private GameObject _dialoguePanelObject;

    private DialogueBox _dialogueBox;

    private int _idx;

    public bool introSceneRunning;

    private List<Joke> _currentBadJokes = new List<Joke>();
    private List<Joke> _currentMediumJokes = new List<Joke>();
    private List<Joke> _currentBestJokes = new List<Joke>();

    private List<Joke> _thisRoundJokes = new List<Joke>();


    [SerializeField]
    private List<Button> _jokeButtons = new List<Button>();

    public static DialogueManager instance;

    void Start()
    {
        if(instance == null) instance = this;

        _dialogueBox = _dialoguePanelObject.GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (introSceneRunning) IntroDialogueScene();

        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.GeneratingJokes)
        {
            MakeJokes();
        }
        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.ChoosingJoke)
        {
        }
    }

    private void MakeJokes()
    {
        for(int i = 0; i < 3; i++)
        {
            _currentBadJokes.Add(new Joke(Joke.JokeGrade.Bad));
            _currentMediumJokes.Add(new Joke(Joke.JokeGrade.Medium));
            _currentBestJokes.Add(new Joke(Joke.JokeGrade.Best));
        }
        OrganizeJokesForRound();
        SetButtons();
    }

    private void SetButtons()
    {
        for(int i = 0; i < _jokeButtons.Count; i++)
        {
            _jokeButtons[i].GetComponent<JokeButton>().ButtonJoke = _thisRoundJokes[i];
        }
    }

    public void ChooseJoke(Joke joke)
    {
        GameManager.instance.potentialReward += joke.moneyReward;

    }

    public void OrganizeJokesForRound()
    {
        for (int i = 0; i < 3; i++)
        {
            _thisRoundJokes.Add(_currentBadJokes[Random.Range(0, _currentBadJokes.Count)]);
            _thisRoundJokes.Add(_currentMediumJokes[Random.Range(0, _currentBadJokes.Count)]);
            _thisRoundJokes.Add(_currentBestJokes[Random.Range(0, _currentBadJokes.Count)]);
        }

        _thisRoundJokes = Ext.Shuffle(_thisRoundJokes);
        GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.ChoosingJoke;
    }

    public void IntroDialogueScene()
    {
        _dialoguePanelObject.SetActive(introSceneRunning);
        if (_dialogueBox.state == DialogueBox.DialogueBoxState.Done)
        {
            if (_idx == INTRO_DIALOGUES.Length)
            {
                introSceneRunning = false;
                return;
            }
            _dialogueBox.DisplayText(INTRO_DIALOGUES[_idx], 0.05f);
            _idx++;
        }
    }
}

public class Ext : MonoBehaviour
{
    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }
}
