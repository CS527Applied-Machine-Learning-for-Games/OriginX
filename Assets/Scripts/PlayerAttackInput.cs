using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.Mathematics;


public class PlayerAttackInput : Agent
{
    public int score = 0;

    public int damage = 1;

    public int startingHealth = 100;

    public Transform shootingPoint;

    public EnemyController enemyControl;

    private CharacterAnimations playerAnimation;

    private Vector3 StartingPosition;

    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    public GameObject raycastObject;

    public GameObject attackPoint;

    public int CurrentHealth;


    private void Hit()
    {
        enemyControl = GameObject.Find("Enemy").GetComponent<EnemyController>() as EnemyController;
        enemyControl.GetShot(damage, this);
        playerAnimation.Attack_1();
    }

    // Start is called before the first frame update
    // Update is called once per frame
    public override void OnActionReceived(float[] vectorAction)
    {   //when you press keys J- defend and K-attack
        Debug.LogWarning("onaction");
        if (Mathf.RoundToInt(vectorAction[0]) >= 1){

            playerAnimation.UnFreezeAnimation();
            playerAnimation.Defend(true);

        }
        if (Mathf.RoundToInt(vectorAction[1]) >= 1){

            AddReward(0.03f);
            playerAnimation.Defend(false);
        }
        if (Mathf.RoundToInt(vectorAction[2]) >= 1){
                Vector3 raycastOffset = new Vector3(0, 0.5f, 0);
                if(Physics.Raycast(this.transform.position+raycastOffset,transform.TransformDirection(Vector3.forward), out var hit, 2.0f))
                {
                    Debug.LogWarning("IN out");
                    if (hit.transform.tag == "Enemy"){
                    AddReward(0.033f);
                    Debug.LogWarning("IN hit");
                    Hit();
                    }

                }
        }
    }

    private void FixedUpdate()
    {
         RequestDecision();
    }
   
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        actionsOut[2] = 0;

        actionsOut[0] = Input.GetKeyDown(KeyCode.J) ? 1f : 0f;
        actionsOut[1] = Input.GetKeyUp(KeyCode.J) ? 1f : 0f;
        actionsOut[2] = Input.GetKey(KeyCode.K) ? 1f : 0f;
        //actionsOut[3] = Input.GetAxis("Horizontal");
        //actionsOut[1] = -Input.GetAxis("Vertical");
        //actionsOut[4] = Input.GetAxis("Vertical");
    }

    public void RegisterKill()
    {
        score++;
        AddReward(1.0f);
    }
    
    public override void Initialize()
    {
       StartingPosition = transform.position;
       CurrentHealth = startingHealth;
       playerAnimation = GetComponent<CharacterAnimations>();
    }

    public override void OnEpisodeBegin()
    
    {
        OnEnvironmentReset?.Invoke();
        transform.position = StartingPosition;
        Debug.LogWarning("episode begin!!!!");
    }

    public void GetShot(int damage, EnemyController shooter)
    {

        ApplyDamage(damage, shooter);
    }
    
    private void ApplyDamage(int damage, EnemyController shooter)
    {
        CurrentHealth = CurrentHealth - damage;
        AddReward(-0.033f);
        Debug.LogWarning("playr");
        Debug.LogWarning(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die(shooter);
        }
    }
    
    private void Die(EnemyController shooter)
    {
        AddReward(-0.033f);
        SetEnemiesActive();
        //shooter.EndEpisode(); 
    }

    private void SetEnemiesActive()
    {
        gameObject.SetActive(false);
        StartingPosition = transform.position;
        gameObject.SetActive(true);
        CurrentHealth = startingHealth;
        //CurrentHealth = startingHealth;
    }

    void Activate_AttackPoint()
    {
       attackPoint.SetActive(true);
    }

    void Deactivate_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }
}
