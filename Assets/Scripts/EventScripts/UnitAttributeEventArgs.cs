using System;

public class UnitAttributeEventArgs : EventArgs
{
    public PlayerHFSMStateManager Player { get; set; }
    // public PlayerAttribute PlayerAttribute { get; set; }
    // public PlayerInputHandler InputHandler { get; set; }
}
public class SendUnitAttribute
{
    // public delegate void SendUnitAttributeEventHandler(object source, UnitEventArgs args); // 這兩行等同下面那一航
    // public event SendUnitAttributeEventHandler AttributeDelegated;
    public event EventHandler<UnitAttributeEventArgs> AttributeDelegated;

    public void SendPlayerAttribute(PlayerHFSMStateManager player)
    {
        OnSendPlayerAttribute(player);
    }

    protected virtual void OnSendPlayerAttribute(PlayerHFSMStateManager player)
    {
        if (AttributeDelegated != null)
        {
            AttributeDelegated(this, new UnitAttributeEventArgs() { Player = player });
        }
    }
  
}
