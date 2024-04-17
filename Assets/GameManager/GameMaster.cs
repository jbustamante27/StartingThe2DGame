using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using System.Linq;
using System.ComponentModel;
using System;
using System.Numerics;

public class GameMaster : NetworkComponent
{
    public bool GameStarted = false;
    public override void HandleMessage(string flag, string value)
    {
        if (IsClient && flag == "GAMESTART")
        {
            NPM[] Players = GameObject.FindObjectsOfType<NPM>();
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].gameObject.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
            }
        }
    }

    public override void NetworkedStart()
    {

    }

    public override IEnumerator SlowUpdate()
    {
        while (IsServer && !GameStarted)
        {
            //NPM == NetworkPlayerManager every one for every player is stored here
            NPM[] Players = GameObject.FindObjectsOfType<NPM>();

            if (Players.Length > 1)
            {
                int RdyPlayers = 0;
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Players[i].IsReady)
                    {
                        RdyPlayers++;
                    }
                }

                if (RdyPlayers == Players.Length)
                {
                    GameStarted = true;
                    SendUpdate("GAMESTART", "");

                    for (int i = 0; i < Players.Length; i++)
                    {

                        GameObject MePlayer = MyCore.NetCreateObject(Players[i].CharSelected, Players[i].Owner, GameObject.Find("P" + (i + 1).ToString() + "Start").transform.position);
                        MePlayer.GetComponent<PlayerCharacter>().ColorSelected = Players[i].ColorSelected;
                        MePlayer.GetComponent<PlayerCharacter>().PName = Players[i].PName;

                    }
                }


            }

            yield return new WaitForSeconds(.1f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
