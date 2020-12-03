using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum EnemyState
{
    CHASE,
    ATTACK
}

public class EnemyController : Agent
{


    private CharacterAnimations enemy_Anim;
    private NavMeshAgent navAgent;
    private Transform playerTarget;
    public float move_Speed = 3.5f;
    public float attack_Distance = 1f;
    public float chase_Player_After_Attack_Distance = 1f;
    private float wait_Before_Attack_Time = 3f;
    private float attack_Timer;
    public GameObject enemyHealth;

    public Text myTextenemy;

    private EnemyState enemy_State;
    public int damage = 1;

    public int startingHealth = 100;

    public PlayerAttackInput Agent;

    public PlayerAttackInput playerControl;

    // public event Action OnEnvironmentReset;



    public int CurrentHealth;
    private Vector3 StartPosition;

    // Start is called before the first frame update
    void Awake()
    {
        enemy_Anim = GetComponent<CharacterAnimations>();
        navAgent = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;

    }


    void Start()
    {   
        StartPosition = transform.position;
        CurrentHealth = startingHealth;
        enemy_State = EnemyState.CHASE;
        attack_Timer = wait_Before_Attack_Time;
        navAgent = GetComponent<NavMeshAgent>();
        myTextenemy = GameObject.Find("EnemyScore").GetComponentInChildren<Text>();
        myTextenemy.text = "Enemy Health :  " + 100;
        SetEnemiesActive();
        // Agent.OnEnvironmentReset += Respawn;
    }


    // Update is called once per frame
    void Update()
    {
        if(enemy_State == EnemyState.CHASE)
        {
            ChasePlayer();
        }
        if(enemy_State == EnemyState.ATTACK)
        {
            AttackPlayer();
        }
    }

    public void GetShot(int damage, EnemyController shooter)
    {

        ApplyDamage(damage, shooter);
    }
    
    private void ApplyDamage(int damage, EnemyController shooter)
    {
        myTextenemy = GameObject.Find("EnemyScore").GetComponentInChildren<Text>();
        myTextenemy.text = "Enemy Health :  " + CurrentHealth.ToString();
        CurrentHealth = CurrentHealth - damage; 
        playerControl.AddReward(0.033f);
        if (CurrentHealth <= 0)
        {
            Die(shooter);
        }
    }
    
    private void Die(EnemyController shooter)
    {
        //AddReward(0.033f);
        playerControl.AddReward(0.033f);
        //shooter.RegisterKill();
        SetEnemiesActive();
        playerControl.EndEpisode(); 
    }

    private void SetEnemiesActive()
    {
        gameObject.SetActive(false);
        CurrentHealth = startingHealth;
        StartPosition = transform.position;
        gameObject.SetActive(true);
        //CurrentHealth = startingHealth;
    }
    public void Respawn()
    {
        StartPosition = transform.position;
        CurrentHealth = startingHealth;
        enemy_State = EnemyState.CHASE;
        attack_Timer = wait_Before_Attack_Time;
        
    }

    void ChasePlayer()
    {
        navAgent.SetDestination(playerTarget.position);
        navAgent.speed = move_Speed;
        if(navAgent.velocity.sqrMagnitude == 0)
        {
            enemy_Anim.Walk(false);
        }
        else
        {
            enemy_Anim.Walk(true);
        }
        if(Vector3.Distance(transform.position,playerTarget.position) <= attack_Distance)
        {
            enemy_State = EnemyState.ATTACK;
        }
    }

    private void Hit()
    {
        playerControl = GameObject.Find("Warrior").GetComponent<PlayerAttackInput>() as PlayerAttackInput;
        playerControl.GetShot(damage, playerControl);
        //playerAnimation.Attack_1();
    }

    void AttackPlayer()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        enemy_Anim.Walk(false);
        attack_Timer += Time.deltaTime;
        if(attack_Timer > wait_Before_Attack_Time)
        {

                enemy_Anim.Attack_1();
                Hit();

                attack_Timer = 0f;
        }
        if(Vector3.Distance(transform.position,playerTarget.position) > attack_Distance + chase_Player_After_Attack_Distance)
        {
            navAgent.isStopped = false;
            enemy_State = EnemyState.CHASE;
        }
    } 

}
