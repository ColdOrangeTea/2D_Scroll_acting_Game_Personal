using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;

public class EnemyStateManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private StateMachine fsm;
    private Animator animator;
    private Text stateDisplayText;
    public Transform playerPos;//chage to private
    [Header("Adjustment")]
    public float chaseSpeed = 10f;

    public Vector2 jumpDir=new Vector2(0.1f,1f);
    public float jumpForce = 5f;//can use newrton's law to adjust
    public float heightGap=5f;
    float ShootCooldown = 5f;

    [Header("Checksbox")]
    public Transform pivotPoint;
    public LayerMask playerLayer;
    public Vector2 combatPointOffset;
    public float combatRadius = 1f;

    public Vector2 rangedPointoffset;
    public float rangedRadius = 3f;
    public LayerMask platformLayer;
    public Vector2 platformCheckPointoffset;
    public Vector2 platformChecksize= new Vector2(0.5f,1);

    [Header("Status")]
    public bool isFacingRight=true;
    public bool inCombatRange;
    public bool inNonShootRange;
    bool canShoot = true;

    public bool onHeadHavePlatform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateDisplayText = GetComponentInChildren<Text>();
        fsm = new StateMachine();
        fsm.AddState("Chase", onEnter: state => animator.Play("Walk"),
        onLogic: state =>
        {
            facingPlayer();
            rb.velocity= new Vector2(chaseSpeed*(isFacingRight?-1:1),rb.velocity.y);
        });
        fsm.AddState("Jump", onEnter: state =>{rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);animator.Play("LongJump");},onExit:state=>rb.velocity=new Vector2(0,0),canExit: state => !AnimatorIsPlaying("LongJump"), needsExitTime: true);
        fsm.AddTransition("Chase","Jump",t=>playerPos.position.y-transform.position.y>heightGap&&onHeadHavePlatform);

        fsm.AddState("Melee", onEnter: state => animator.Play("Attack1"), canExit: state => !AnimatorIsPlaying("Attack1"), needsExitTime: true);
        fsm.AddTransition("Chase", "Melee", t => inCombatRange);

        fsm.AddState("Ranged", onEnter: state => { animator.Play("Attack1"); SetUnAbleToShoot(); }, canExit: state => !AnimatorIsPlaying("Attack1"), needsExitTime: true);
        fsm.AddTransitionFromAny("Ranged", t => !inNonShootRange && canShoot);
        fsm.AddTransitionFromAny("Chase");
        fsm.SetStartState("Chase");
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
        stateDisplayText.text = fsm.GetActiveHierarchyPath();
        inCombatRange = Physics2D.OverlapCircle((Vector2)pivotPoint.position + combatPointOffset, combatRadius, playerLayer);
        inNonShootRange = Physics2D.OverlapCircle((Vector2)pivotPoint.position + rangedPointoffset, rangedRadius, playerLayer);
        onHeadHavePlatform=Physics2D.OverlapBox((Vector2)pivotPoint.position+platformCheckPointoffset,platformChecksize,0,platformLayer);    
    }
    public void facingPlayer()
	{
		if(playerPos.position.x<transform.position.x!=isFacingRight)
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

    private void SetUnAbleToShoot()
    {
        canShoot = false;
        CancelInvoke("SetAbletoShoot");
        Invoke("SetAbletoShoot", ShootCooldown);

    }
    private void SetAbletoShoot()
    {
        canShoot = true;
    }
    private void SetIgnoreCollision()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerPos.GetComponent<Collider2D>());
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
     private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + combatPointOffset, combatRadius);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + rangedPointoffset, rangedRadius);
        Gizmos.color=Color.cyan;
        Gizmos.DrawWireCube((Vector2)pivotPoint.position+platformCheckPointoffset,platformChecksize);

    }
}
