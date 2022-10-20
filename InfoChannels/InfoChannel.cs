using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.AI;

[System.Serializable]
public class InfoChannel
{
    public Guid id;
    public string name;

    public bool locked = false;

    private GameObject channelInGameObject;

    #region Init, De-init
    public void InitializeChannel(GameObject argPrefab, int argPositionX, int argPositionY, string argName)
    {
        //assign ID and name
        id = Guid.NewGuid();
        name = argName;

        //create world canvas
        channelInGameObject = GameObject.Instantiate(argPrefab, InfoChannelManager.instance.channelRoot);
        channelInGameObject.transform.position = new Vector3(argPositionX, argPositionY, channelInGameObject.transform.position.z);

        //add to master channel list
        InfoChannelManager.instance.channels.Add(this);

        Ascalon.Log("InfoChannel created with ID " + id, LogMode.InfoVerbose);
    }

    public void InitializeChannel(int argPositionX, int argPositionY, string argName)
    {
        InitializeChannel(InfoChannelManager.instance.defaultChannelPrefab, argPositionX, argPositionY, argName);
    }

    public void DestroyChannel()
    {
        InfoChannelManager.instance.channels.Remove(this);
        GameObject.Destroy(channelInGameObject);
        id = Guid.Empty;
    }
    #endregion




    #region Parameter manipulation
    public void SetTMPParameters()
    {
        TextMeshProUGUI tmpText = channelInGameObject.GetComponent<TextMeshProUGUI>();

        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmpText.enableWordWrapping = false;
    }

    public TextMeshProUGUI GetTMP()
    {
        return channelInGameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetPosition(int argPositionX, int argPositionY)
    {
        channelInGameObject.transform.position = new Vector3(argPositionX, argPositionY, channelInGameObject.transform.position.z);
    }
    #endregion

    #region Set/Clear text
    public void SetText(string argText)
    {
        //do nothing if the text is locked
        if (locked)
        {
            return;
        }

        channelInGameObject.GetComponent<TextMeshProUGUI>().text = argText;
    }

    public void Clear()
    {
        //do nothing if the text is locked
        if (locked)
        {
            return;
        }

        channelInGameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
    #endregion
}
