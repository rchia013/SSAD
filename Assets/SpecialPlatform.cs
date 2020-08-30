using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlatform : MonoBehaviour
{
    // Active Block Generation Parameters:

    //public int cooldownSB = 20;
    //bool putSpecialBlock = true;

    //bool power1used = false;
    //bool power2used = false;


    //// Collection of Blocks

    //Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    //int totalNumBlocks;

    //// Current Records

    //ActivatedBlock ABscript = null;

    //int curNum = -1;
    //int prevNum = -1;

    //SpecialBlock SBscript = null;

    //int specNum = -1;

    //// Player:

    //private GameObject player;



    //// Start is called before the first frame update
    //void Start()
    //{
    //    totalNumBlocks = transform.childCount;

    //    for (int i = 0; i < totalNumBlocks; i++)
    //    {
    //        blocks.Add(i, transform.GetChild(i).gameObject);
    //    }

    //    player = GameObject.FindWithTag("Player");

    //}

    ////Update is called once per frame
    //void Update()
    //{
    //    if (blocks.ContainsKey(prevNum))
    //    {
    //        if (blocks[prevNum] == null)
    //        {
    //            // Remove destroyed blocks

    //            blocks.Remove(prevNum);
    //            print("Deleted");
    //        }
    //    }

    //    if (putActiveBlock && blocks.Count > totalNumBlocks / 4 && player.GetComponent<Movement>().moveable)
    //    {
    //        if (blocks.ContainsKey(prevNum))
    //        {
    //            ABscript.blockActivated = false;
    //            ABscript.enabled = false;
    //        }

    //        curNum = -1;
    //        int temp;

    //        while (curNum == -1)
    //        {
    //            temp = Random.Range(0, totalNumBlocks);

    //            if (blocks.ContainsKey(temp) && temp != prevNum)
    //            {
    //                curNum = temp;

    //                ActivateBlock(blocks[curNum]);
    //            }
    //        }
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (putSpecialBlock && blocks.Count > totalNumBlocks && !(power1used && power2used))
    //    {
    //        specNum = -1;

    //        int temp;

    //        while (specNum == -1)
    //        {
    //            temp = Random.Range(0, totalNumBlocks);

    //            if (blocks.ContainsKey(temp) && temp != prevNum && temp != curNum)
    //            {
    //                specNum = temp;

    //                SpecialBlock(blocks[specNum]);

    //                print("Chosen SB");
    //            }
    //        }


    //    }
    //}

    //void ActivateBlock(GameObject block)
    //{
    //    // Activate Block

    //    ABscript = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
    //    ABscript.enabled = true;

    //    ABscript.blockActivated = true;

    //    // Cooldown

    //    putActiveBlock = false;

    //    StartCoroutine("CooldownAB");

    //}

    //void SpecialBlock(GameObject block)
    //{
    //    SBscript = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();

    //    int choice;

    //    if (!power1used && !power2used)
    //    {
    //        choice = Random.Range(0, 2);
    //    }
    //    else
    //    {
    //        if (power1used)
    //            choice = 2;
    //        else
    //            choice = 1;
    //    }

    //    if (choice != -1)
    //    {
    //        SBscript.choice = choice;
    //        SBscript.blockActivated = true;
    //        SBscript.enabled = true;

    //        // Cooldown

    //        putSpecialBlock = false;

    //        print("SB Active");

    //        StartCoroutine("CooldownSB");
    //    }
    //}

    //IEnumerator CooldownAB()
    //{
    //    int counter = cooldownAB;

    //    while (counter > 0)
    //    {
    //        yield return new WaitForSeconds(1);
    //        counter--;

    //        print(counter);

    //        if (counter < 1)
    //        {
    //            prevNum = curNum;
    //            putActiveBlock = true;
    //        }
    //    }
    //}

    //IEnumerator CooldownSB()
    //{
    //    int counter = cooldownSB;

    //    while (counter > 0)
    //    {
    //        yield return new WaitForSeconds(1);
    //        counter--;

    //        print(counter);

    //        if (counter < 1)
    //        {
    //            putSpecialBlock = true;
    //        }
    //    }
    }