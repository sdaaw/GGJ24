using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joke
{
    // Start is called before the first frame update

    private readonly string[] BEST_JOKE_OPTIONS = new string[]
    {
        "is like a freezer— everyone puts their meat in her.",
        "is so ugly, she threw a boomerang and it refused to come back.",
        "so fat that when she goes camping, bears hide their food.",
        "so ugly. The whole world faked the pandemic so she would wear a mask.",
        "so fat, she gives memory foam PTSD.",
        "so ugly, her birth certificate was an apology letter.",
        "so slow it took her 9 months to make a joke.",
        "is like a door knob— everyone gets a turn."
    };

    private readonly string[] MEDIUM_JOKE_OPTIONS = new string[]
    {
        "is like a subway— everybody rides her for cheap.",
        "is like a bat— sleeps during the day but sucks all night.",
        "is great at multitasking, but she'll never be able to juggle two balls at once – only one ball (the other is too big).",
        "so fat, when she sends me a selfie My phone weighs more!",
        "so ugly, her portraits hang themselves.",
        "so fat that her only friend on Facebook is McDonald's.",
        "is like a bike— everyone rides her.",
        "so ugly she gives Freddy Krueger nightmares.",
        "is so stupid when the judge said, 'Order! Order!' she said, 'Fries and coke please.'"
    };

    private readonly string[] BAD_JOKE_OPTIONS = new string[]
    {
        "so stupid, she went to the dentist to get a Bluetooth.",
        "so ugly, when she looks in the mirror, her reflection ducks.",
        "so dumb, when the doctor told her she had coronavirus, she bought a new laptop.",
        "so fat, on a scale from one to ten, she's a 747.",
        "so fat, her blood type is Ragu.",
        "so fat, when God said, 'Let there be light,' he asked her to move out of the way.",
        "so old, she was a waitress at the Last Supper.",
        "so fat, a vampire sucked her blood and got diabetes."
    };

    public enum JokeGrade
    {
        Bad,
        Medium,
        Best,
    }
    public JokeGrade Grade { get; set; }

    public string[] Options { get; private set; }

    public string JokeText;

    public int moneyReward;

    public Joke(JokeGrade grade)
    {
        Grade = grade;
        JokeText = GetJoke(grade);
    }

    public string GetJoke(JokeGrade grade)
    {
        switch(grade)
        {
            case JokeGrade.Bad:
            {
                moneyReward = 3;
                return BAD_JOKE_OPTIONS[Random.Range(0, BAD_JOKE_OPTIONS.Length)];
            }
            case JokeGrade.Medium: 
            {
                moneyReward = 8;
                return MEDIUM_JOKE_OPTIONS[Random.Range(0, MEDIUM_JOKE_OPTIONS.Length)];
            }
            case JokeGrade.Best:
            {
                moneyReward = 15;
                return BEST_JOKE_OPTIONS[Random.Range(0, BEST_JOKE_OPTIONS.Length)];
            }
        }
        return null;
    }
}
