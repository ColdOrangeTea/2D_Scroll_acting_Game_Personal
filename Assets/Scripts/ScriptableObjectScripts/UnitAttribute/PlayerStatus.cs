using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New PlayerStatus", menuName = "SO/DataTool/ Create PlayerStatus Asset", order = 4)]
public class PlayerStatus : ScriptableObject
{
    public int MaxHp = 20;
    public int Hp = 20;
    // public int maxMp = 0;
    // public int mp = 0;
    public int Damage = 3;
}
