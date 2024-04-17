using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;
public class PlayerCharacter : NetworkComponent
{
    public Text PlayerName;
    public Material[] MColor;
    public int ColorSelected = -1;
    public string PName = "<Default>";
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "COLOR_CHOICE" && IsClient)
        {
            ColorSelected = int.Parse(value);
            gameObject.GetComponent<SpriteRenderer>().color = MColor[ColorSelected].color;
        }

        if (flag == "PNAME" && IsClient)
        {
            //Parsing is deriving a value from a string
            PName = value;
            PlayerName.text = PName;
        }

    }

    public override void NetworkedStart()
    {

    }

    public override IEnumerator SlowUpdate()
    {
        while (IsConnected)
        {

            if (IsServer)
            {
                if (IsDirty)
                {
                    SendUpdate("COLOR_CHOICE", ColorSelected.ToString());

                    SendUpdate("PNAME", PName.ToString());


                    IsDirty = false;
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
