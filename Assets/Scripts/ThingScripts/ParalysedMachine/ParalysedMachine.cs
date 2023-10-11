using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysedMachine : Thing
{
    [SerializeField] private bool is_activated;
    [SerializeField] private float start_time;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (CheckIfEffectDurationOver())
        {
            is_activated = false;
            TurnOff();
            Debug.Log("關閉");
        }
    }
    private void TurnOff()
    {
        Animation.Effect.gameObject.SetActive(false);
        Animation.Sprite.gameObject.SetActive(true);

    }
    private void TurnOn()
    {
        // Take effect
        Animation.Effect.gameObject.SetActive(true);
        Animation.Sprite.gameObject.SetActive(false);

    }
    public override void TakeDamage()
    {
        if (!Attribute.CanBeDamaged) // 是互動物件
        {
            if (CheckIfCanActivated())
            {
                if (!is_activated)
                {
                    start_time = Time.time;
                    is_activated = true;
                    TurnOn();
                    Debug.Log("開啟");
                }
            }
            else
            {
                Debug.Log("冷卻中...");
            }
        }
        else // 是箱子之類的物件
        {
            DestroyThing();
        }
    }
    private bool CheckIfCanActivated() => (Time.time > start_time + Attribute.ActivatedCoolDown) || (start_time == 0);
    private bool CheckIfEffectDurationOver() => is_activated && Time.time >= start_time + Attribute.ActivatedDuration;
}
