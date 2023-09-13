using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataText : MonoBehaviour
{
    public PlayerData playerData;
    public Text hp;
    public Text attack;

    void Start()
    {

    }

    void Update()
    {
        hp.text = "HP: " + playerData.playerData.hp + " / " + playerData.playerData.maxHp.ToString();
        attack.text = "Attack: " + playerData.playerData.attack;
    }
}
