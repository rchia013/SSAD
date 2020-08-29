using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlatform : MonoBehaviour
{
    // Active Block Generation Parameters:

    public int cooldown = 10;
    bool putActiveBlock = true;


    // Collection of Blocks

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    int totalNumBlocks;

    // Current Records

    ActivatedBlock script = null;

    int curNum = -1;
    int prevNum = -1;

    int curPowerBlock = -1;

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

    //Update is called once per frame
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
                script.blockActivated = false;
                script.enabled = false;
            }

            curNum = -1;
            int temp;

            while (curNum == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp) && temp != prevNum)
                {
                    curNum = temp;

                    ActivateBlock(blocks[curNum]);
                }
            }
        }
    }

    void ActivateBlock(GameObject block)
    {
        // Activate Block

        script = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
        script.enabled = true;

        script.blockActivated = true;

        // Cooldown

        putActiveBlock = false;

        StartCoroutine("Cooldown");

    }

    IEnumerator Cooldown()
    {
        int counter = cooldown;

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
}
