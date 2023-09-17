using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeText : MonoBehaviour
{
    // 手動放物件
    public Text hpText;
    public Text attackText;

    void Start()
    {

    }

    public void UpdateHP(int hp, int maxHp, int attack)
    {
        hpText.text = "HP: " + hp.ToString() + " / " + maxHp.ToString();
        attackText.text = "Attack: " + attack.ToString();
    }
}
