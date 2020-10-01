using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivePlatform : MonoBehaviourPunCallbacks
{
    // Active Block Generation Parameters:

    public int cooldownAB = 10;
    bool putActiveBlock1 = true;
    bool putActiveBlock2 = true;
    bool putActiveBlock3 = true;
    bool putActiveBlock4 = true;

    public int cooldownSB = 20;
    bool putSpecialBlock = true;

    bool power0used = false;
    bool power1used = false;


    // Collection of Blocks

    public Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    int totalNumBlocks;

    // Current Records

    ActivatedBlock ABscript1 = null;
    ActivatedBlock ABscript2 = null;
    ActivatedBlock ABscript3 = null;
    ActivatedBlock ABscript4 = null;

    public int curNum1 = -1;
    public int prevNum1 = -1;

    public int curNum2 = -1;
    public int prevNum2 = -1;

    public int curNum3 = -1;
    public int prevNum3 = -1;

    public int curNum4 = -1;
    public int prevNum4 = -1;

    SpecialBlock power0Script = null;
    SpecialBlock power1Script = null;

    int power0Num = -1;
    int power1Num = -1;

    int powerChoice = -1;

    // Player:

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    private List<GameObject> players = new List<GameObject>();

    private PhotonView PV;


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        totalNumBlocks = transform.childCount;

        for (int i = 0; i < totalNumBlocks; i++)
        {
            blocks.Add(i, transform.GetChild(i).gameObject);
        }

        player1 = GameObject.FindWithTag("Player1");

        player2 = GameObject.FindWithTag("Player2");

        player3 = GameObject.FindWithTag("Player3");

        player4 = GameObject.FindWithTag("Player4");

        PV.RPC("findPlayers", RpcTarget.All);
    }

    [PunRPC]
    void findPlayers()
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                player1 = GameObject.FindWithTag("Player1");
                break;

            case 2:
                player2 = GameObject.FindWithTag("Player2");
                break;

            case 3:
                player3 = GameObject.FindWithTag("Player3");
                break;

            case 4:
                player4 = GameObject.FindWithTag("Player4");
                break;
            
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (blocks.ContainsKey(prevNum1))
        {
            if (blocks[prevNum1] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(prevNum1);
            }
        }

        if (blocks.ContainsKey(prevNum2))
        {
            if (blocks[prevNum2] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(prevNum2);
            }
        }

        if (blocks.ContainsKey(prevNum3))
        {
            if (blocks[prevNum3] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(prevNum3);
            }
        }

        if (blocks.ContainsKey(prevNum4))
        {
            if (blocks[prevNum4] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(prevNum4);
            }
        }

        if (player1 != null && putActiveBlock1 && blocks.Count > totalNumBlocks / 4 && player1.GetComponent<Movement>().moveable)
        {
            if (blocks.ContainsKey(prevNum1))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 1);
            }

            curNum1 = -1;
            int temp;

            while (curNum1 == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp)
                    && temp != prevNum1
                    && temp != prevNum2 && temp != curNum2
                    && temp != prevNum3 && temp != curNum3
                    && temp != prevNum4 && temp != curNum4
                    && temp != power1Num && temp != power0Num)
                {
                    curNum1 = temp;

                    PV.RPC("ActivateBlock", RpcTarget.All, curNum1, 1);
                }
            }
        }

        if (player2 != null && putActiveBlock2 && blocks.Count > totalNumBlocks / 4 && player2.GetComponent<Movement>().moveable)
        {
            if (blocks.ContainsKey(prevNum2))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 2);
            }

            curNum2 = -1;
            int temp;

            while (curNum2 == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp)
                    && temp != prevNum2
                    && temp != prevNum1 && temp != curNum1
                    && temp != prevNum3 && temp != curNum3
                    && temp != prevNum4 && temp != curNum4
                    && temp != power1Num && temp != power0Num)
                {
                    curNum2 = temp;

                    PV.RPC("ActivateBlock", RpcTarget.All, curNum2, 2);
                }
            }
        }

        if (player3 != null && putActiveBlock3 && blocks.Count > totalNumBlocks / 4 && player3.GetComponent<Movement>().moveable)
        {
            if (blocks.ContainsKey(prevNum3))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 3);
            }

            curNum3 = -1;
            int temp;

            while (curNum3 == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp)
                    && temp != prevNum3
                    && temp != prevNum1 && temp != curNum1
                    && temp != prevNum2 && temp != curNum2
                    && temp != prevNum4 && temp != curNum4
                    && temp != power1Num && temp != power0Num)
                {
                    curNum3 = temp;

                    PV.RPC("ActivateBlock", RpcTarget.All, curNum3, 3);
                }
            }
        }

        if (player4 != null && putActiveBlock4 && blocks.Count > totalNumBlocks / 4 && player4.GetComponent<Movement>().moveable)
        {
            if (blocks.ContainsKey(prevNum4))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 4);
            }

            curNum4 = -1;
            int temp;

            while (curNum4 == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp)
                    && temp != prevNum4
                    && temp != prevNum1 && temp != curNum1
                    && temp != prevNum2 && temp != curNum2
                    && temp != prevNum3 && temp != curNum3
                    && temp != power1Num && temp != power0Num)
                {
                    curNum4 = temp;

                    PV.RPC("ActivateBlock", RpcTarget.All, curNum4, 4);
                }
            }
        }

        if (putSpecialBlock && blocks.Count > totalNumBlocks / 4 && !(power0used && power1used) &&
            ((player1 != null && player1.GetComponent<Movement>().moveable) ||
            (player2 != null && player2.GetComponent<Movement>().moveable) ||
            (player3 != null && player3.GetComponent<Movement>().moveable) ||
            (player4 != null && player4.GetComponent<Movement>().moveable)))
        {

            int newSpecNum = -1;

            int temp;

            while (newSpecNum == -1)
            {
                temp = Random.Range(0, totalNumBlocks);

                if (blocks.ContainsKey(temp)
                    && temp != prevNum1 && temp != curNum1
                    && temp != prevNum2 && temp != curNum2
                    && temp != prevNum3 && temp != curNum3
                    && temp != prevNum4 && temp != curNum4
                    && temp != power1Num && temp != power0Num)
                {
                    newSpecNum = temp;

                    int powerChoice = -1;

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

                    PV.RPC("SpecialBlock", RpcTarget.All, newSpecNum, powerChoice);
                }
            }


        }
    }

    [PunRPC]
    void ActivateBlock(int blockIndex, int playerNum)
    {
        GameObject block = blocks[blockIndex];

        // Activate Block

        switch (playerNum)
        {
            case 1:
                ABscript1 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript1.playerIndex = playerNum;
                ABscript1.colorIndex = player1.GetComponent<Movement>().colorIndex;
                ABscript1.enabled = true;

                ABscript1.blockActivated = true;
                break;

            case 2:
                ABscript2 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript2.playerIndex = playerNum;
                ABscript2.colorIndex = player2.GetComponent<Movement>().colorIndex;
                ABscript2.enabled = true;

                ABscript2.blockActivated = true;
                break;

            case 3:
                ABscript3 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript3.playerIndex = playerNum;
                ABscript3.colorIndex = player3.GetComponent<Movement>().colorIndex;
                ABscript3.enabled = true;

                ABscript3.blockActivated = true;
                break;

            case 4:
                ABscript4 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript4.playerIndex = playerNum;
                ABscript4.colorIndex = player4.GetComponent<Movement>().colorIndex;
                ABscript4.enabled = true;

                ABscript4.blockActivated = true;
                break;
        }

        // Cooldown

        switch (playerNum)
        {
            case 1:
                putActiveBlock1 = false;
                break;

            case 2:
                putActiveBlock2 = false;
                break;

            case 3:
                putActiveBlock3 = false;
                break;

            case 4:
                putActiveBlock4 = false;
                break;
        }

        StartCoroutine(CooldownAB(playerNum));

    }

    [PunRPC]
    void SpecialBlock(int blockIndex, int powerChoice)
    {
        GameObject block = blocks[blockIndex];

        if (powerChoice == 0)
        {
            power0Script = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();
            power0Script.choice = powerChoice;
            power0Script.blockActivated = true;
            power0Script.enabled = true;

            power0Num = blockIndex;
            power0used = true;
        }
        else if (powerChoice == 1)
        {
            power1Script = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();
            power1Script.choice = powerChoice;
            power1Script.blockActivated = true;
            power1Script.enabled = true;

            power1Num = blockIndex;
            power1used = true;
        }

        if (powerChoice != -1)
        {
            // Cooldown

            putSpecialBlock = false;
            StartCoroutine(CooldownSB(true));
        }
    }

    [PunRPC]

    void DeactivateBlock(int playerNum)
    {
        ActivatedBlock curABScript = null;

        switch (playerNum)
        {
            case 1:
                curABScript = ABscript1;
                break;
            case 2:
                curABScript = ABscript2;
                break;
            case 3:
                curABScript = ABscript3;
                break;
            case 4:
                curABScript = ABscript4;
                break;
        }

        curABScript.blockActivated = false;
        curABScript.playerIndex = 0;
        curABScript.enabled = false;
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
            }
        }
    }

    IEnumerator CooldownAB(int playerNum)
    {
        int counter = cooldownAB;

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;

            if (counter < 1)
            {
                switch (playerNum)
                {
                    case 1:
                        prevNum1 = curNum1;
                        putActiveBlock1 = true;
                        break;

                    case 2:
                        prevNum2 = curNum2;
                        putActiveBlock2 = true;
                        break;

                    case 3:
                        prevNum3 = curNum3;
                        putActiveBlock3 = true;
                        break;

                    case 4:
                        prevNum4 = curNum4;
                        putActiveBlock4 = true;
                        break;
                }
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
