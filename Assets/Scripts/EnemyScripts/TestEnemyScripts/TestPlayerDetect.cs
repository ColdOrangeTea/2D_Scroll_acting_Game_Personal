using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerDetect : MonoBehaviour
{
    #region --COMPONENTS--
    public Rigidbody2D RB { get; private set; }
    public Collider2D MyselfCollider { get; private set; }

    #endregion

    [SerializeField] private Vector2 player_check_offset;
    [SerializeField] private Transform player_checkpoint;
    [SerializeField] private Vector2 player_check_size;
    [SerializeField] private LayerMask attackable_layer;
    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        CheckIfSawPlayer();
    }
    #region PLAYERCHECK METHOD
    public bool CheckIfSawPlayer()
    {
        if (Physics2D.OverlapBox((Vector2)player_checkpoint.position, player_check_size, 0, attackable_layer))
        {
            if (Physics2D.OverlapBox(player_checkpoint.position, player_check_size, 0, attackable_layer) == MyselfCollider)
            {
                return false;
            }
            else
            {
                if (Physics2D.OverlapBox(player_checkpoint.position, player_check_size, 0, attackable_layer).CompareTag("Player"))
                {
                    Debug.Log("玩家察覺:箱子測試 ");
                    return true;
                }
                else // 不是自己 也不是玩家
                    return false;
            }
        }
        else // 沒東西
            return false;
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (player_checkpoint)
        {
            Gizmos.DrawWireCube((Vector2)player_checkpoint.position + player_check_offset, player_check_size);
            Gizmos.DrawWireCube((Vector2)player_checkpoint.position + player_check_offset, player_check_size / 2);
        }
    }
}
