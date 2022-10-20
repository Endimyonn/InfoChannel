using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;

public class InfoChannelManager : MonoBehaviour
{
    public static InfoChannelManager instance;

    [SerializeField] public List<InfoChannel> channels;

    public Transform channelRoot;

    public GameObject defaultChannelPrefab;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    #region Channel Creation/Destruction
    public static Guid CreateChannel(GameObject argPrefab, int argPositionX, int argPositionY, string argName)
    {
        InfoChannel newChannel = new InfoChannel();
        newChannel.InitializeChannel(argPrefab, argPositionX, argPositionY, argName);
        return newChannel.id;
    }

    public static Guid CreateChannel(int argPositionX, int argPositionY, string argName)
    {
        InfoChannel newChannel = new InfoChannel();
        newChannel.InitializeChannel(argPositionX, argPositionY, argName);
        return newChannel.id;
    }

    public static Guid CreateChannelIfDoesntExist(GameObject argPrefab, int argPositionX, int argPositionY, string argName)
    {
        if (ChannelExists(argName))
        {
            return Guid.Empty;
        }
        InfoChannel newChannel = new InfoChannel();
        newChannel.InitializeChannel(argPrefab, argPositionX, argPositionY, argName);
        return newChannel.id;
    }

    public static Guid CreateChannelIfDoesntExist(int argPositionX, int argPositionY, string argName)
    {
        if (ChannelExists(argName))
        {
            return Guid.Empty;
        }
        InfoChannel newChannel = new InfoChannel();
        newChannel.InitializeChannel(argPositionX, argPositionY, argName);
        return newChannel.id;
    }

    public static void DestroyChannel(Guid argChannelID)
    {
        instance.channels.Find(x => x.id == argChannelID).DestroyChannel();
    }

    public static void DestroyChannel(string argChannelName)
    {
        instance.channels.Find(x => x.name == argChannelName).DestroyChannel();
    }
    #endregion


    #region Channel text manipulation
    public static void SetChannelText(Guid argChannelID, string argText)
    {
        instance.channels.Find(x => x.id == argChannelID).SetText(argText);
    }

    public static void SetChannelText(string argChannelName, string argText)
    {
        instance.channels.Find(x => x.name == argChannelName).SetText(argText);
    }

    public static void ClearChannel(Guid argChannelID)
    {
        instance.channels.Find(x => x.id == argChannelID).Clear();
    }

    public static void ClearChannel(string argChannelName)
    {
        instance.channels.Find(x => x.name == argChannelName).Clear();
    }

    public static void SetChannelTextTemporary(Guid argChannelID, string argText, float argTime)
    {
        InfoChannel timedChannel = instance.channels.Find(x => x.id == argChannelID);
        instance.StartCoroutine(instance.TemporaryText(timedChannel, argText, argTime));
    }

    public static void SetChannelTextTemporary(string argChannelName, string argText, float argTime)
    {
        InfoChannel timedChannel = instance.channels.Find(x => x.name == argChannelName);
        instance.StartCoroutine(instance.TemporaryText(timedChannel, argText, argTime));
    }

    public IEnumerator TemporaryText(InfoChannel argChannel, string argText, float argTime)
    {
        //do nothing if the text is locked
        if (argChannel.locked)
        {
            yield return null;
        }

        //set text, lock, wait, clear text, unlock
        argChannel.SetText(argText);
        argChannel.locked = true;
        yield return new WaitForSeconds(argTime);
        argChannel.locked = false;
        argChannel.Clear();
    }
    #endregion


    public static void SetChannelPosition(Guid argChannelID, int argPositionX, int argPositionY)
    {
        instance.channels.Find(x => x.id == argChannelID).SetPosition(argPositionX, argPositionY);
    }

    public static void SetChannelPosition(string argChannelName, int argPositionX, int argPositionY)
    {
        instance.channels.Find(x => x.name == argChannelName).SetPosition(argPositionX, argPositionY);
    }

    //does a channel exist?
    public static bool ChannelExists(string argChannelName)
    {
        return (instance.channels.Find(x => x.name == argChannelName) != null);
    }

    #region Console debug stuff
    [ConCommand("hud_temphint", "Notif testing")]
    static void cmd_hud_temphint(string argText, float argTime)
    {
        InfoChannelManager.CreateChannelIfDoesntExist(Screen.width / 2, Screen.height / 3, "default");
        InfoChannelManager.SetChannelTextTemporary("default", argText, argTime);
    }
    #endregion
}
