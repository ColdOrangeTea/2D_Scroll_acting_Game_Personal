using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeEventArgs : EventArgs
{
    //¥iÂX¥R
}
public class SendFading
{
    public static event EventHandler<SceneFadeEventArgs> SendSceneFadOut;
    public static event EventHandler<SceneFadeEventArgs> SendSceneFadIn;

    public void SendSceneFadeOut()
    {
        OnSendSceneFadeOut();
    }

    public void OnSendSceneFadeOut()
    {
        if (SendSceneFadOut != null)
            SendSceneFadOut(this, new SceneFadeEventArgs());
    }

    public void SendSceneFadeIn()
    {
        OnSendSceneFadeIn();
    }

    public void OnSendSceneFadeIn()
    {
        if (SendSceneFadIn != null)
            SendSceneFadIn(this, new SceneFadeEventArgs());
    }
}
