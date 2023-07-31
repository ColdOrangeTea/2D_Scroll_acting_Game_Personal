using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    // 以Hollow Knight的遊戲功能當參考藍本
    // 目前基本狀態: 走路(左右移動)、跳躍、攻擊(包含攻擊時的反作用力，參考Hollow Knight)、二段跳

    public bool walking;
    public bool jumping;

    // 反作用力，水平方向(X)攻擊、垂直方向(Y)攻擊皆可造成
    public bool recoilingX;
    public bool recoilingY;

    // 未來計畫新增的:移動視角、施展技能、與NPC對話、開啟商店

    // 玩家移動視角
    public bool looking;

    // 技能，狀態分成施展中/ 施展出來
    public bool casting;
    public bool castReleased;

    // 可互動的物件，靠近會提示玩家可以互動，互動可能是:對話、打開開關、使用特定道具
    public bool interact;
    public bool interacting;

    // 可互動的NPC，靠近會提示玩家可以互動，互動可能是:對話、打開商店
    public bool atNPC;
    public bool usingNPC;

    // 存檔，狀態分成:站在存檔物件範圍裡、在存檔物件附近
    public bool onBench;
    public bool atBench;

    //public bool 
}
