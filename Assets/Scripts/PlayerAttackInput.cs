using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
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

    public int CurrentHealth;
    public GameObject playerHealth;


    public Text myText;


    private void Hit()
    {
        enemyControl = GameObject.Find("Enemy").GetComponent<EnemyController>() as EnemyController;
        enemyControl.GetShot(damage, enemyControl);
        playerAnimation.Attack_1();
    }

    // Start is called before the first frame update
    // Update is called once per frame
    public override void OnActionReceived(float[] vectorAction)
    {   //when you press keys J- defend and K-attack
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
                    if (hit.transform.tag == "Enemy"){
                    AddReward(0.033f);
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
        myText = GameObject.Find("PlayerScore").GetComponentInChildren<Text>();
        myText.text = "Player Health :  " + 100;
    }

    public override void OnEpisodeBegin()
    
    {
        OnEnvironmentReset?.Invoke();
        transform.position = StartingPosition;
    }

    public void GetShot(int damage, PlayerAttackInput player)
    {

        ApplyDamage(damage, player);
    }
    
    private void ApplyDamage(int damage, PlayerAttackInput player)
    {
        myText = GameObject.Find("PlayerScore").GetComponentInChildren<Text>();
        myText.text = "Player Health :  " + CurrentHealth.ToString();
        CurrentHealth = CurrentHealth - damage;

        AddReward(-0.033f);
        if (CurrentHealth <= 0)
        {
            Die(player);
        }
    }
    
    private void Die(PlayerAttackInput player)
    {
        AddReward(-0.033f);
        SetEnemiesActive();
        EndEpisode();
    }

    private void SetEnemiesActive()
    {
        gameObject.SetActive(false);
        StartingPosition = transform.position;
        gameObject.SetActive(true);
        CurrentHealth = startingHealth;
        //CurrentHealth = startingHealth;
    }


}
