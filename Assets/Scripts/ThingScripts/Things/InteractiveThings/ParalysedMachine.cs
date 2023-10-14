using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysedMachine : InteractiveThing
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void TurnOff()
    {
        base.TurnOff();
        Animation.Effect.gameObject.SetActive(false);
        Animation.Sprite.gameObject.SetActive(true);

    }
    protected override void TurnOn()
    {
        base.TurnOn();
        // Take effect
        Animation.Effect.gameObject.SetActive(true);
        Animation.Sprite.gameObject.SetActive(false);

    }

}
