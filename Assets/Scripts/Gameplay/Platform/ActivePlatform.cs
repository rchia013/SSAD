using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// This script determines the activation of all player blocks and special blocks throughout the entire gameplay.
/// It is assigned to the group of active blocks on each arena.
/// It processes all logic required for the random assignment and reshuffling of blocks in the game;
/// and activates the chosen blocks for their required function.
/// </summary>

public class ActivePlatform : MonoBehaviourPunCallbacks
{
    // Active Block Generation Parameters:
    public int cooldownAB = 8;
    bool putActiveBlock1 = true;
    bool putActiveBlock2 = true;
    bool putActiveBlock3 = true;
    bool putActiveBlock4 = true;

    // Special Block Generation Parameters:
    public int cooldownSB = 10;
    bool putSpecialBlock = true;

    // Powerup Tracking:
    bool power0used = false;
    bool power1used = false;
    bool power2used = false;

    // Collection of Blocks
    public Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    int totalNumBlocks;

    // Current State and Record of active/special scripts and blocks
    ActivatedBlock ABscript1 = null;
    public int curNum1 = -1;
    public int prevNum1 = -1;

    ActivatedBlock ABscript2 = null;
    public int curNum2 = -1;
    public int prevNum2 = -1;

    ActivatedBlock ABscript3 = null;
    public int curNum3 = -1;
    public int prevNum3 = -1;

    ActivatedBlock ABscript4 = null;
    public int curNum4 = -1;
    public int prevNum4 = -1;

    SpecialBlock power0Script = null;
    int power0Num = -1;

    SpecialBlock power1Script = null;
    int power1Num = -1;

    SpecialBlock power2Script = null;
    int power2Num = -1;

    int powerChoice = -1;

    // Information on ALL existing players in game:
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    private List<GameObject> players = new List<GameObject>();


    // Photon View
    private PhotonView PV;

    /// <summary>
    /// Start is called before first frame update,
    /// to initialize certain settings and paramters (mainly for the arena)
    /// </summary>
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        totalNumBlocks = transform.childCount;
        for (int i = 0; i < totalNumBlocks; i++)
        {
            blocks.Add(i, transform.GetChild(i).gameObject);
        }
    }
    
    /// <summary>
    /// Update is called every frame, and is the main function used to determine the arena behavior
    /// </summary>

    void Update()
    {
        // Players must be found at every frame, to avoid errors in cases where players leave mid game.
        // If a player ceases to exist, his blocks will no longer appear.
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
        player3 = GameObject.FindWithTag("Player3");
        player4 = GameObject.FindWithTag("Player4");


        // Updates blocks list if the previously assigned block has fallen/been destroyed. If not, it can be deactivated.
        if (blocks.ContainsKey(prevNum1))
        {
            if (blocks[prevNum1] == null)
            {
                blocks.Remove(prevNum1);
            }
        }
        if (blocks.ContainsKey(prevNum2))
        {
            if (blocks[prevNum2] == null)
            {
                blocks.Remove(prevNum2);
            }
        }
        if (blocks.ContainsKey(prevNum3))
        {
            if (blocks[prevNum3] == null)
            {
                blocks.Remove(prevNum3);
            }
        }
        if (blocks.ContainsKey(prevNum4))
        {
            if (blocks[prevNum4] == null)
            {
                blocks.Remove(prevNum4);
            }
        }


        // Conducts randomization of blocks for the local player.
        // Using this, each player in the game will calculate the block on the arena to be assigned to them.
        // After it is calculated, a network event is called to ensure all players see the same block assigned to the particular player.

        // Player 1:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && player1 != null &&
            putActiveBlock1 && blocks.Count > totalNumBlocks / 4 && player1.GetComponent<PlayerController>().moveable)
        {
            // Deactivate previous block if it has not been destroyed
            if (blocks.ContainsKey(prevNum1))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 1);
            }

            // Block randomization and activation
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
                    && temp != power2Num && temp != power1Num && temp != power0Num)
                {
                    PV.RPC("ActivateBlock", RpcTarget.All, temp, 1);
                }
            }
        }

        // Player 2:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && player2 != null && putActiveBlock2 && blocks.Count > totalNumBlocks / 4 && player2.GetComponent<PlayerController>().moveable)
        {
            // Deactivate previous block if it has not been destroyed
            if (blocks.ContainsKey(prevNum2))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 2);
            }

            // Block randomization and activation
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
                    && temp != power2Num && temp != power1Num && temp != power0Num)
                {
                    PV.RPC("ActivateBlock", RpcTarget.All, temp, 2);
                }
            }
        }

        // Player 3:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 3 && player3 != null && putActiveBlock3 && blocks.Count > totalNumBlocks / 4 && player3.GetComponent<PlayerController>().moveable)
        {
            // Deactivate previous block if it has not been destroyed
            if (blocks.ContainsKey(prevNum3))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 3);
            }

            // Block randomization and activation
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
                    && temp != power2Num && temp != power1Num && temp != power0Num)
                {
                    PV.RPC("ActivateBlock", RpcTarget.All, temp, 3);
                }
            }
        }

        // Player 4:
        if (PhotonNetwork.LocalPlayer.ActorNumber == 4 && player4 != null && putActiveBlock4 && blocks.Count > totalNumBlocks / 4 && player4.GetComponent<PlayerController>().moveable)
        {
            // Deactivate previous block if it has not been destroyed
            if (blocks.ContainsKey(prevNum4))
            {
                PV.RPC("DeactivateBlock", RpcTarget.All, 4);
            }

            // Block randomization and activation
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
                    && temp != power2Num && temp != power1Num && temp != power0Num)
                {

                    PV.RPC("ActivateBlock", RpcTarget.All, temp, 4);
                }
            }
        }

        // Special Block randomization and activation (only done by Host (player 1))
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 &&
            putSpecialBlock && blocks.Count > totalNumBlocks / 4 && !(power0used && power1used && power2used) &&
            ((player1 != null && player1.GetComponent<PlayerController>().moveable) ||
            (player2 != null && player2.GetComponent<PlayerController>().moveable) ||
            (player3 != null && player3.GetComponent<PlayerController>().moveable) ||
            (player4 != null && player4.GetComponent<PlayerController>().moveable)))
        {

            // Block randomization and activation
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
                    && temp != power1Num && temp != power0Num && temp != power2Num)
                {
                    newSpecNum = temp;

                    // Determines which powerup to use, depending on which are available
                    int powerChoice = -1;
                    if (!power0used && !power1used && !power2used)
                    {
                        powerChoice = Random.Range(0, 3);
                    }
                    else
                    {
                        if (power0used)
                        {
                            if (!power1used && !power2used)
                                powerChoice = Random.Range(1, 3);

                            else if (power1used)
                                powerChoice = 2;

                            else if (power2used)
                                powerChoice = 1;
                        }

                        else if (power1used)
                        {
                            if (!power0used && !power2used)
                            {
                                powerChoice = Random.Range(0, 2);

                                if (powerChoice == 1)
                                {
                                    powerChoice = 2;
                                }
                            }
                                

                            else if (power0used)
                                powerChoice = 2;

                            else if (power2used)
                                powerChoice = 0;
                        }

                        else if (power2used)
                        {
                            if (!power0used && !power1used)
                                powerChoice = Random.Range(0,2);

                            else if (power0used)
                                powerChoice = 1;

                            else if (power1used)
                                powerChoice = 0;
                        }
                    }

                    PV.RPC("SpecialBlock", RpcTarget.All, newSpecNum, powerChoice);
                }
            }


        }
    }

    /// <summary>
    /// This function is called to trigger a network event where a chosen block is activated for a particular player
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <param name="playerNum"></param>

    [PunRPC]
    void ActivateBlock(int blockIndex, int playerNum)
    {

        GameObject block = blocks[blockIndex];

        // Activate Block based on blockIndex and playerNum

        switch (playerNum)
        {
            case 1:
                curNum1 = blockIndex;

                ABscript1 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript1.playerIndex = playerNum;
                ABscript1.colorIndex = player1.GetComponent<PlayerController>().colorIndex;
                ABscript1.enabled = true;

                ABscript1.blockActivated = true;
                break;

            case 2:
                curNum2 = blockIndex;

                ABscript2 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript2.playerIndex = playerNum;
                ABscript2.colorIndex = player2.GetComponent<PlayerController>().colorIndex;
                ABscript2.enabled = true;

                ABscript2.blockActivated = true;
                break;

            case 3:
                curNum3 = blockIndex;

                ABscript3 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript3.playerIndex = playerNum;
                ABscript3.colorIndex = player3.GetComponent<PlayerController>().colorIndex;
                ABscript3.enabled = true;

                ABscript3.blockActivated = true;
                break;

            case 4:
                curNum4 = blockIndex;

                ABscript4 = block.transform.GetChild(0).gameObject.GetComponent<ActivatedBlock>();
                ABscript4.playerIndex = playerNum;
                ABscript4.colorIndex = player4.GetComponent<PlayerController>().colorIndex;
                ABscript4.enabled = true;

                ABscript4.blockActivated = true;
                break;
        }

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

        // Initiate cooldown to determine when a certain player should be assigned a new block
        StartCoroutine(CooldownAB(playerNum));

    }

    /// <summary>
    /// This function is called to trigger a network event to activate a special block for all players
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <param name="powerChoice"></param>

    [PunRPC]
    void SpecialBlock(int blockIndex, int powerChoice)
    {
        GameObject block = blocks[blockIndex];

        //Activates special block based on block and powerup chosen
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
        else if (powerChoice == 2)
        {
            power2Script = block.transform.GetChild(0).gameObject.GetComponent<SpecialBlock>();
            power2Script.choice = powerChoice;
            power2Script.blockActivated = true;
            power2Script.enabled = true;

            power2Num = blockIndex;
            power2used = true;
        }

        if (powerChoice != -1)
        {
            // Cooldown

            putSpecialBlock = false;
            StartCoroutine(CooldownSB(true));
        }
    }


    /// <summary>
    /// This function is called to trigger a network event to deactivate a particular player's block
    /// </summary>
    /// <param name="playerNum"></param>
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


    /// <summary>
    /// FixedUpdate is called every frame to determine if the special blocks have been used and destroyed.
    /// If so, the powerup can be reassigned to a new special block.
    /// </summary>
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

        if (blocks.ContainsKey(power2Num))
        {
            if (blocks[power2Num] == null)
            {
                // Remove destroyed blocks

                blocks.Remove(power2Num);
                StartCoroutine(CooldownSB(false));
                power2used = false;
            }
        }
    }


    /// <summary>
    /// This function is used to track the time since a block has been activated for a particular user.
    /// If the specified time is up, then a new block should be assigned to the user.
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
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

    /// <summary>
    /// This function is used to track the amount of time since the last special block was assigned.
    /// If the cooldown has passed, then a new special block can be assigned.
    /// </summary>
    /// <param name="initial"></param>
    /// <param name="powerNum"></param>
    /// <returns></returns>
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

                    if (powerNum == 2)
                    {
                        power2used = false;
                        power2Script = null;
                        power2Num = -1;
                    }
                }
            }
        }


    }
}
