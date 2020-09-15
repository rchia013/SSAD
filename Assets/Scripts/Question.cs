using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    void Start()
    {
        Options = new Dictionary<string, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
