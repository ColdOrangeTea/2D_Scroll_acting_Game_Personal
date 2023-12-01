using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;
public class RabbitCanonerSta : MonoBehaviour
{
    private Rigidbody2D rb;
    private UnityHFSM.StateMachine fsm;
    private Animator animator;
    private Text stateDisplayText;
    public Transform playerPos;//chage to private

    public Transform firePoint;
    public Transform Canon1;
    public Transform Canon2;
    public Transform Canon3;
    public GameObject axePrefab;


    [Header("Adjustment")]
    public float chaseSpeed = 10f;

    public float force = 10f;
    float betweenWaves = 3f;
    


    [Header("Checksbox")]
    public Transform pivotPoint;
    public LayerMask playerLayer;
    public Vector2 shootRangePointOffset;
    public float rangedRadius = 1f;


    [Header("Status")]
    public bool isFacingRight = true;
    public bool inShootRange;
    public bool isThrowing;
    public bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateDisplayText = GetComponentInChildren<Text>();
        fsm = new UnityHFSM.StateMachine();
        fsm.AddState("Patrol", onEnter: state => animator.Play("Walk"),
        onLogic: state =>
        {
            //facingPlayer();
        });
        fsm.AddState("Throw", new CoState(this, ThreeWaveAxeThrow, loop: false,canExit:state=>!isThrowing,needsExitTime: true));
        fsm.AddState("Wait", onEnter:state=>WaitForAttack(),canExit:state=>!isWaiting,needsExitTime:true);
        fsm.AddTransition("Patrol","Throw",t=>inShootRange==true);
        fsm.AddTransition("Throw","Wait");
        fsm.AddTransition("Wait","Patrol");


        fsm.SetStartState("Patrol");
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
        stateDisplayText.text = fsm.GetActiveHierarchyPath();
        inShootRange = Physics2D.OverlapCircle((Vector2)pivotPoint.position + shootRangePointOffset, rangedRadius, playerLayer);
    }
    public void facingPlayer()
    {
        if (playerPos.position.x < transform.position.x != isFacingRight)
            Turn();
    }
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    bool AnyAnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnyAnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }


    private IEnumerator ThreeWaveAxeThrow()
    {
        int AttackRound = 3;
        isThrowing=true;

        while (AttackRound > 0)
        {

            GameObject Axe1 = Instantiate(axePrefab, firePoint.position, Canon1.rotation);
            GameObject Axe2 = Instantiate(axePrefab, firePoint.position, Canon2.rotation);
            GameObject Axe3 = Instantiate(axePrefab, firePoint.position, Canon3.rotation);

            Rigidbody2D axe1 = Axe1.GetComponent<Rigidbody2D>();
            Rigidbody2D axe2 = Axe2.GetComponent<Rigidbody2D>();
            Rigidbody2D axe3 = Axe3.GetComponent<Rigidbody2D>();
            axe1.AddForce(Axe1.transform.right * force, ForceMode2D.Impulse);
            axe2.AddForce(Axe2.transform.right * force, ForceMode2D.Impulse);
            axe3.AddForce(Axe3.transform.right * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
            AttackRound--;
        }
        isThrowing=false;
        yield break;
    }
    private void WaitForAttack()
    {
        isWaiting=true;
        Invoke("UnWait",betweenWaves);
    }
    private void UnWait()
    {
        isWaiting=false;
    }
    
     private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + shootRangePointOffset, rangedRadius);
  
    }
}
