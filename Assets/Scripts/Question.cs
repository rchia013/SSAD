using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{

    public int ID
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

    public string Description
    {
        get;
        private set;
    }

    public Dictionary<string, bool> Options
    {
        get;
        private set;
    }

    //int ID;
    //int Difficulty;
    //int Cateogory;
    //string Description public object MyProperty { get; set; }

    // Start is called before the first frame update

    public Question(int id, int diff, int cat, string desc, Dictionary<string, bool> options)
    {
        this.ID = id;
        this.Difficulty = diff;
        this.Category = cat;
        this.Description = desc;
        this.Options = options;
    }
}
