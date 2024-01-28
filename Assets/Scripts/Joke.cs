using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joke
{
    // Start is called before the first frame update

    private readonly string[] BEST_JOKE_OPTIONS = new string[]
    {
        "Yo mama is like a freezer— everyone puts their meat in her.",
        "Yo mama is so ugly, she threw a boomerang and it refused to come back.",
        "Yo mama's encounter with a giant squid ended up it telling her to go fuck herself before leaving.",
        "Yo mama so fat that when she goes camping, bears hide their food.",
        "Yo momma so ugly. The whole world faked the pandemic so she would wear a mask.",
        "Yo mamma so fat, she gives memory foam PTSD.",
        "Yo mama so ugly, her birth certificate was an apology letter.",
        "Yo mama so slow it took her 9 months to make a joke.",
        "Yo mama is like a door knob— everyone gets a turn."
    };

    private readonly string[] MEDIUM_JOKE_OPTIONS = new string[]
    {
        "Yo mama is like a subway— everybody rides her for cheap.",
        "Yo mama is like a bat— sleeps during the day but sucks all night.",
        "Yo mama is great at multitasking, but she'll never be able to juggle two balls at once – only one ball (the other is too big).",
        "Yo momma so fat, when she sends me a selfie My phone weighs more!",
        "Yo mama so ugly, her portraits hang themselves.",
        "Yo mama so fat that her only friend on Facebook is McDonald's.",
        "Yo mama is like a bike— everyone rides her.",
        "Yo mama so ugly she gives Freddy Krueger nightmares.",
        "Yo mama is so stupid when the judge said, 'Order! Order!' she said, 'Fries and coke please.'"
    };

    private readonly string[] BAD_JOKE_OPTIONS = new string[]
    {
        "Yo mama's so stupid, she went to the dentist to get a Bluetooth.",
        "Yo mama’s so ugly, when she looks in the mirror, her reflection ducks.",
        "Yo mama's so dumb, when the doctor told her she had coronavirus, she bought a new laptop.",
        "Your mama's so fat, on a scale from one to ten, she's a 747.",
        "Yo mama's so poor, Nigerian princes wire her money.",
        "Yo mama's so fat, her blood type is Ragu.",
        "Yo mama's so fat, when God said, 'Let there be light,' he asked her to move out of the way.",
        "Yo mama's so old, she was a waitress at the Last Supper.",
        "Yo mama's so fat, a vampire sucked her blood and got diabetes."
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
