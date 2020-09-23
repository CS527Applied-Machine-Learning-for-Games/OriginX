using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackInput : MonoBehaviour
{
    private CharacterAnimations playerAnimation;
    // Start is called before the first frame update
    void Awake()
    {
        playerAnimation = GetComponent<CharacterAnimations>();
    }

    // Update is called once per frame
    void Update()
    {   //when you press keys J- defend and K-attack
        if(Input.GetKeyDown(KeyCode.J))
        {
            playerAnimation.UnFreezeAnimation();
            playerAnimation.Defend(true);
        }
        if(Input.GetKeyUp(KeyCode.J))
        {
            playerAnimation.Defend(false);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            if(Random.Range(0,2)>0)
            {
                playerAnimation.Attack_1();
            }
            else
            {
                playerAnimation.Attack_2();
            }
        }
    }
}
