using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialChannelActivity : MonoBehaviour
{
    void Start()
    {
        InfoChannelManager.CreateChannelIfDoesntExist(Screen.width / 2, Screen.height / 3, "default");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InfoChannelManager.SetChannelText("default", "Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InfoChannelManager.ClearChannel("default");
        }
    }
}
