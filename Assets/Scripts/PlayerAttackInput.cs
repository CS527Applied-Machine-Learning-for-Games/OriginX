using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.Mathematics;


public class PlayerAttackInput : Agent
{
    public int score = 0;

    public int damage = 100;

    public EnemyManager enemyManager;

    private CharacterAnimations playerAnimation;

    private EnemyController enemyControl;

    private Vector3 StartingPosition;

    private EnvironmentParameters EnvironmentParameters;


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
            if(UnityEngine.Random.Range(0,2)>0)
            {
                AddReward(0.033f);
                enemyControl.GetShot(damage, this);
                playerAnimation.Attack_1();
            }
            else
            {
                playerAnimation.Attack_2();
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
       playerAnimation = GetComponent<CharacterAnimations>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = StartingPosition;
    }

}
