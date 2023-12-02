using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;

public class MachineJellyFishHFSMStateManager : MonoBehaviour
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
    // public Transform TentaclesCollider;
    // [Space(10)]

    // [Header("Adjustment")]
    // #region StretchOut
    // bool goDown = true;
    // bool goBack = false;
    // public float StretchOutSpeed = 2f;
    // public float yOriginPosition;
    // [Range(0.1f, 5)]
    // public float yLengthStretchOut;
    // public float stretchOutTimer;
    // public float stretchOutSec;
    // #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        // yOriginPosition = TentaclesCollider.position.y;
        // stretchOutTimer = stretchOutSec;

        fsm = new StateMachine();
        fsm.AddState("stretchOut", onEnter: state => { },
        onLogic: state =>
        {
            // if (!goBack && goDown)
            // {
            //     float distance = Mathf.Abs((yLengthStretchOut + TentaclesCollider.position.y) - yOriginPosition);
            //     Debug.Log("dis: " + distance + " 往下TentaclesCollider.position.y " + TentaclesCollider.position.y + " " + yLengthStretchOut + " " + yOriginPosition);

            //     if (distance <= 0.01f)
            //     {
            //         Debug.Log("開始倒數");

            //         // TentaclesCollider.transform.position = new Vector2(TentaclesCollider.transform.position.x, yOriginPosition - yLengthstretchOut);
            //         stretchOutTimer -= Time.deltaTime;
            //         if (stretchOutTimer <= 0)
            //         {
            //             Debug.Log("倒數完畢");
            //             goDown = false;
            //             goBack = true;
            //             stretchOutTimer = stretchOutSec;
            //         }
            //     }
            //     else
            //     {
            //         // 往下
            //         TentaclesCollider.transform.position = Vector2.MoveTowards(TentaclesCollider.position, new Vector2(TentaclesCollider.position.x, yOriginPosition - yLengthStretchOut), Time.deltaTime * StretchOutSpeed);
            //     }
            // }
            // else if (!goDown && goBack)
            // {
            //     float distance = Mathf.Abs(TentaclesCollider.position.y - yOriginPosition);
            //     //Debug.Log("dis: " + distance + " 往下TentaclesCollider.position.y " + TentaclesCollider.position.y + " " + yOriginPosition);

            //     if (distance <= 0.01f)
            //     {
            //         // Debug.Log("開始倒數");
            //         stretchOutTimer -= Time.deltaTime;
            //         if (stretchOutTimer <= 0)
            //         {
            //             //  Debug.Log("倒數完畢");
            //             goBack = false;
            //             goDown = true;
            //             stretchOutTimer = stretchOutSec;
            //         }
            //     }
            //     else
            //     {
            //         // 往回
            //         TentaclesCollider.transform.position = Vector2.MoveTowards(TentaclesCollider.position, new Vector2(TentaclesCollider.position.x, yOriginPosition), Time.deltaTime * StretchOutSpeed);
            //     }
            // }
        }
        );

        fsm.SetStartState("stretchOut");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
    }
}
