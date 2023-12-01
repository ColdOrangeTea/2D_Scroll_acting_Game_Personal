using System.Collections;
using UnityEngine;
using UnityHFSM;

public class MachineBatHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private StateMachine fsm;
    private Animator animator;
    #endregion

    [Header("Checksbox")]
    public Transform pivotPoint;
    [Space(10)]

    [Header("Adjustment")]
    #region DROP
    public Vector2 groundCheckOffset;
    public float xGroundCheck, yGroundCheck;
    public float DropSpeed = 2f;
    public float dropSec;
    #endregion

    #region --LAYERS--
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private LayerMask thingLayer;
    enum NumOfLayer
    {
        AttackableUnit = 7,
        Thing = 8,
        Attack = 9
    }
    #endregion

    #region TAG NAME
    public const string PLAYER = "Player";
    public const string B_THING = "B_Thing";
    public const string P_THING = "P_Thing";
    #endregion
    private IEnumerator DropCoroutine;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        DropCoroutine = Drop();
        fsm = new StateMachine();
        fsm.AddState("drop", onEnter: state => { rb.velocity = new Vector2(0, -1 * DropSpeed); StartCoroutine(DropCoroutine); });

        fsm.SetStartState("drop");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
    }
    IEnumerator Drop()
    {
        // 無窮迴圈
        while (true)
        {
            // Debug.Log("掉落");
            rb.velocity = new Vector2(0, -1 * DropSpeed);
            yield return new WaitForEndOfFrame();
            if (GroundCheck())
            {// 停止指定秒數
                StartCoroutine(DestroySelf(dropSec));
                break;
            }
        }
    }
    IEnumerator DestroySelf(float sec)
    {
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(sec);

        // 刪除物件
        // Debug.Log("刪除");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public bool GroundCheck()
    {
        if (Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset, Vector2.down, yGroundCheck, groundLayer) &&
        Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, 0), Vector2.down, yGroundCheck, groundLayer) &&
        Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(+xGroundCheck, 0), Vector2.down, yGroundCheck, groundLayer))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset, (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(0, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, -yGroundCheck));

    }
}
