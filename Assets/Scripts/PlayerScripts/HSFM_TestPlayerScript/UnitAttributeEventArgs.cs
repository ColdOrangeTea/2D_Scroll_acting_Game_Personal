using System;

public class UnitAttributeEventArgs : EventArgs
{
    public TestPlayerController Player { get; set; }
    public TestEnemyManager Enemy { get; set; }
    // public PlayerAttribute PlayerAttribute { get; set; }
    // public PlayerInputHandler InputHandler { get; set; }
}
public class SendUnitAttribute
{
    // public delegate void SendUnitAttributeEventHandler(object source, UnitEventArgs args); // 這兩行等同下面那一航
    // public event SendUnitAttributeEventHandler AttributeDelegated;
    public event EventHandler<UnitAttributeEventArgs> AttributeDelegated;

    public void SendEnemyAttribute(TestEnemyManager enemy)
    {
        OnSendEnemyAttribute(enemy);
    }

    protected virtual void OnSendEnemyAttribute(TestEnemyManager enemy)
    {
        if (AttributeDelegated != null)
        {
            AttributeDelegated(this, new UnitAttributeEventArgs() { Enemy = enemy });
        }
    }

    public void SendPlayerAttribute(TestPlayerController player)
    {
        OnSendPlayerAttribute(player);
    }

    protected virtual void OnSendPlayerAttribute(TestPlayerController player)
    {
        if (AttributeDelegated != null)
        {
            AttributeDelegated(this, new UnitAttributeEventArgs() { Player = player });
        }
    }
    // public void SendPlayerAttribute(PlayerAttribute playerAttribute, PlayerInputHandler inputHandler)
    // {
    //     OnSendPlayerAttribute(playerAttribute, inputHandler);
    // }

    // protected virtual void OnSendPlayerAttribute(PlayerAttribute playerAttribute, PlayerInputHandler inputHandler)
    // {
    //     if (AttributeDelegated != null)
    //     {
    //         AttributeDelegated(this, new UnitAttributeEventArgs() { PlayerAttribute = playerAttribute, InputHandler = inputHandler });
    //     }
    // }
}
