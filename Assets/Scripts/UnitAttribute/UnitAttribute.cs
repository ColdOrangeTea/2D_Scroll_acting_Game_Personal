using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New UnitAttribute", menuName = "DataTool/ Create UnitAttribute Asset", order = 1)]
public class UnitAttribute : ScriptableObject
{
    // 資料的存取範圍要用 public
    public int maxHp = 0;
    public int hp = 0;
    public int maxMp = 0;
    public int mp = 0;
    public int attack = 0;
}
