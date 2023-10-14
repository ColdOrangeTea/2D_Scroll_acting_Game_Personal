using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveThing : Thing
{
    #region TIMER parameter
    [SerializeField] protected bool isActivated;
    [SerializeField] protected float coolDownStartTime;
    [SerializeField] protected float effectStartTime;
    #endregion

    #region UNITY CALLBACK FUNCTIONS
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (IsRequiredEffectDuration())
        {
            if (CheckIfEffectDurationOver())
            {
                TurnOff();
                Debug.Log("生效時間到 關閉");
            }
        }

    }
    #endregion


    protected virtual void TurnOff()
    {
        isActivated = false;
    }
    protected virtual void TurnOn()
    {
        // Take effect
        isActivated = true;
        if (IsRequiredEffectDuration())
        {
            effectStartTime = Time.time;
        }
    }

    public override void TakeDamage()
    {
        if (IsRequiredCoolDown())
        {
            if (CheckIfCoolDownOver())
            {
                coolDownStartTime = Time.time;
                if (!CheckIfIsActivated())
                {
                    TurnOn();
                    Debug.Log("開啟" + CheckIfIsActivated());
                }
                else
                {
                    TurnOff();
                    Debug.Log("關閉" + CheckIfIsActivated());
                }
            }
            else
            {
                Debug.Log("冷卻中...");
            }
        }
        else
        {
            if (!CheckIfIsActivated())
            {
                TurnOn();
                Debug.Log("沒冷卻 開啟");
            }
            else
            {
                TurnOff();
                Debug.Log("沒冷卻 關閉");
            }
        }
    }
    #region CHECK METHODS
    public virtual bool IsRequiredCoolDown() => Attribute.ActivatedCoolDown > 0;
    public virtual bool IsRequiredEffectDuration() => Attribute.EffectedDuration > 0;
    public virtual bool CheckIfCoolDownOver() => (Time.time >= coolDownStartTime + Attribute.ActivatedCoolDown) || coolDownStartTime == 0;
    public virtual bool CheckIfEffectDurationOver() => Time.time >= effectStartTime + Attribute.EffectedDuration;
    public virtual bool CheckIfIsActivated() => isActivated == true;
    #endregion

}
