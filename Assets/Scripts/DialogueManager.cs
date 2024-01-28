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

    public readonly string[] BAD_JOKE_REACTIONS = new string[]
    {
        "I think my mama would have made a better joke.. HAHAHA HA H@! %A A?# H!&#@ HAH$A",
        "Mamaless behaviour.. Disappointing. *sighs*"
    };
    public readonly string[] MEDIUM_JOKE_REACTIONS = new string[]
    {
        "That was alright.."
    };
    public readonly string[] BEST_JOKE_REACTIONS = new string[]
    {
        "Peak humour!"
    };

    public readonly string[] PREPARE_DIALOGUES = new string[]
    {
        "That was.. something.. how about we start the show?",
        "GET READY"
    };


    public GameObject dialoguePanelObject;

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

    private Joke _chosenJoke;

    private int jokesChosen = 0;

    public int JokeCycleCount = 3;

    private bool transitionDelayStarted;

    void Start()
    {
        if(instance == null) instance = this;

        _dialogueBox = dialoguePanelObject.GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.Intro)
        {
            IntroDialogueScene();
        }

        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.GeneratingJokes)
        {
            MakeJokes();
        }
        if (GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.ChoosingJoke)
        {
            if (_chosenJoke == null) return;
            HandleJokeChoosing();
        }
        if(GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.BattlePrepare)
        {
            BattleBeginDialogueScene();
        }
    }

    private void HandleJokeChoosing()
    {
        if (_chosenJoke.Grade == Joke.JokeGrade.Bad)
        {
            _dialogueBox.DisplayText(BAD_JOKE_REACTIONS[Random.Range(0, BAD_JOKE_REACTIONS.Length)], 0.01f);
        }
        if (_chosenJoke.Grade == Joke.JokeGrade.Medium)
        {
            _dialogueBox.DisplayText(MEDIUM_JOKE_REACTIONS[Random.Range(0, MEDIUM_JOKE_REACTIONS.Length)], 0.01f);
        }
        if (_chosenJoke.Grade == Joke.JokeGrade.Best)
        {
            _dialogueBox.DisplayText(BEST_JOKE_REACTIONS[Random.Range(0, BEST_JOKE_REACTIONS.Length)], 0.01f);
        }
        jokesChosen++;
        if (jokesChosen == JokeCycleCount)
        {
            jokesChosen = 0;
            GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.BattlePrepare;
        }
        else
        {
            GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.GeneratingJokes;
        }

    }

    private void MakeJokes()
    {
        _chosenJoke = null;
        _currentBadJokes = new();
        _currentMediumJokes = new();
        _currentBestJokes = new();
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
        GameManager.instance.MoneyReward += joke.moneyReward;
        _chosenJoke = joke;
    }

    public void OrganizeJokesForRound()
    {
        _thisRoundJokes = new();
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
        dialoguePanelObject.SetActive(introSceneRunning);
        if (_dialogueBox.state == DialogueBox.DialogueBoxState.Done)
        {
            if (_idx == INTRO_DIALOGUES.Length)
            {
                introSceneRunning = false;
                _idx = 0;
                GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.GeneratingJokes;
                return;
            }
            _dialogueBox.DisplayText(INTRO_DIALOGUES[_idx], 0.01f);
            _idx++;
        }
    }

    public void BattleBeginDialogueScene()
    {
        if (_dialogueBox.state == DialogueBox.DialogueBoxState.Done)
        {
            if (_idx == INTRO_DIALOGUES.Length)
            {
                _idx = 0;
                GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
                return;
            }
            _dialogueBox.DisplayText(PREPARE_DIALOGUES[_idx], 0.02f);
            _idx++;
        }
    }

    IEnumerator TransitionDelay()
    {
        transitionDelayStarted = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.BattlePrepare;
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
