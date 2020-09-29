using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Option
{
    public string one;
    public string two;
    public string three;
    public string four;
}
public class Question
{

    public int ID;
    public int Difficulty;
    public int Category;
    public string Description;
    public Option Options;
    public string Correct;

    //public int id
    //{
    //    get;
    //    private set;
    //}
    //public int difficulty
    //{
    //    get;
    //    private set;
    //}
    //public int category
    //{
    //    get;
    //    private set;
    //}

    //public string description
    //{
    //    get;
    //    private set;
    //}

    //public Dictionary<string, bool> options
    //{
    //    get;
    //    private set;
    //}


}
