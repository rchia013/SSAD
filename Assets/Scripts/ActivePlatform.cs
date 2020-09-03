using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlatform : MonoBehaviour
{
    // Active Block Generation Parameters:

    public int cooldownAB = 10;
    bool putActiveBlock = true;

    public int cooldownSB = 20;
    bool putSpecialBlock = true;

    bool power0used = false;
    bool power1used = false;


    // Collection of Blocks

    public Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    int totalNumBlocks;

    // Current Records

    ActivatedBlock ABscript = null;

    public int curNum = -1;
    public int prevNum = -1;

    SpecialBlock power0Script = null;
    SpecialBlock power1Script = null;

    int power0Num = -1;
    int power1Num = -1;

    int powerChoice = -1;

    // Player:

    private GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        totalNumBlocks = transform.childCount;

        for (int i = 0; i < totalNumBlocks; i++)
        {
            blocks.Add(i, transform.GetChild(i).gameObject);
        }

        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (blocks.ContainsKey(prevNum))
        {
            if (blocks[prevNum] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(prevNum);
                print("Deleted");
            }
        }

        if (putActiveBlock && blocks.Count > totalNumBlocks / 4 && player.GetComponent<Movement>().moveable)
        {
            if (blocks.ContainsKey(prevNum))
            {
                ABscript.blockActivated = false;
                ABscript.enabled = false;
            }

            curNum = -1;
            int temp;

            while (curNum == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp) && temp != prevNum && temp != power1Num && temp != power0Num)
                {
                    curNum = temp;

                    ActivateBlock(blocks[curNum]);
                }
            }
        }

        if (putSpecialBlock && blocks.Count > totalNumBlocks / 4 && !(power0used && power1used))
        {

            int newSpecNum = -1;

            int temp;

            while (newSpecNum == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp) && temp != prevNum && temp != curNum)
                {
                    newSpecNum = temp;

                    SpecialBlock(blocks[newSpecNum], newSpecNum);

                    print("Chosen SB");
                }
            }


        }
    }


    void ActivateBlock(GameObject block)
    {
        // Activate Block

        ABscript = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
        ABscript.enabled = true;

        ABscript.blockActivated = true;

        // Cooldown

        putActiveBlock = false;

        StartCoroutine("CooldownAB");

    }

    void SpecialBlock(GameObject block, int index)
    {
        if (!power0used && !power1used)
        {
            powerChoice = Random.Range(0, 2);
        }
        else
        {
            if (power0used)
                powerChoice = 1;

            else if (power1used)
                powerChoice = 0;
        }

        if (powerChoice == 0)
        {
            power0Script = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();
            power0Script.choice = powerChoice;
            power0Script.blockActivated = true;
            power0Script.enabled = true;

            power0Num = index;
            power0used = true;
        }
        else if (powerChoice == 1)
        {
            power1Script = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();
            power1Script.choice = powerChoice;
            power1Script.blockActivated = true;
            power1Script.enabled = true;

            power1Num = index;
            power1used = true;
        }


        if (powerChoice != -1)
        {
            // Cooldown

            putSpecialBlock = false;

            print("SB Active");

            StartCoroutine(CooldownSB(true));
        }
    }

    private void FixedUpdate()
    {
        if (blocks.ContainsKey(power0Num))
        {
            if (blocks[power0Num] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(power0Num);
                StartCoroutine(CooldownSB(false));
                power0used = false;

                print("Power 0 Available");
            }
        }

        if (blocks.ContainsKey(power1Num))
        {
            if (blocks[power1Num] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(power1Num);
                StartCoroutine(CooldownSB(false));
                power1used = false;

                print("Power 1 Available");
            }
        }
    }

    IEnumerator CooldownAB()
    {
        int counter = cooldownAB;

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            print(counter);

            if (counter < 1)
            {
                prevNum = curNum;
                putActiveBlock = true;
            }
        }
    }

    IEnumerator CooldownSB(bool initial, int powerNum = -1)
    {
        int counter = 0;
        if (initial)
        {
            counter = cooldownSB;

            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;

                print(counter);

                if (counter < 1)
                {
                    putSpecialBlock = true;
                }
            }
        }
        else
        {
            counter = 5;

            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;

                print(counter);

                if (counter < 1)
                {
                    if (powerNum == 0)
                    {
                        power0used = false;
                        power0Script = null;
                        power0Num = -1;
                    }

                    if (powerNum == 1)
                    {
                        power1used = false;
                        power1Script = null;
                        power1Num = -1;
                    }
                }
            }
        }

        
    }
}
