using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New UnitData", menuName = "DataTool/ Create UnitData Asset", order = 1)]
public class UnitData : ScriptableObject
{
    // 資料的存取範圍要用 public
    public int maxHp = 0;
    public int hp = 0;
    public int maxMp = 0;
    public int mp = 0;
    public int attack = 0;
}

// [System.Serializable]
// public class UnitInfo
// {
//     // 資料的存取範圍要用 public
//     public int maxHp = 0;
//     public int hp = 0;
//     public int maxMp = 0;
//     public int mp = 0;
//     public int attack = 0;
// }


// // [CreateAssetMenu]

// // fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
// [CreateAssetMenu(fileName = "New UnitData", menuName = "DataTool/ Create UnitData Asset", order = 1)]
// public class UnitData : ScriptableObject
// {
//     public UnitInfo unitData;

// }
