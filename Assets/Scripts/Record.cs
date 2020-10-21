using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public string DateTime
    {
        get;
        private set;
    }

    public int Difficulty
    {
        get;
        private set;
    }

    public int Category
    {
        get;
        private set;
    }

    public string playerName
    {
        get;
        private set;
    }

    public int Points
    {
        get;
        private set;
    }

    public Dictionary<string, int> Responses
    {
        get;
        private set;
    }


    // Start is called before the first frame update
    public Record(string datetime, int diff, int cat, string playerName, int points, Dictionary<string, int> resp)
    {
        this.DateTime = datetime;
        this.Difficulty = diff;
        this.Category = cat;

        this.playerName = playerName;
        this.Points = points;
        this.Responses = resp;
    }
}
