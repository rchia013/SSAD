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

    public int SessionID
    {
        get;
        set;
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


    //int ID;
    //int Difficulty;
    //int Cateogory;
    //string Description public object MyProperty { get; set; }

    // Start is called before the first frame update
    public Record(string datetime, int sessID, int diff, int cat, string playerName, int points, Dictionary<string, int> resp)
    {
        this.DateTime = datetime;
        this.SessionID = sessID;
        this.Difficulty = diff;
        this.Category = cat;

        this.playerName = playerName;
        this.Points = points;
        this.Responses = resp;
    }
}
