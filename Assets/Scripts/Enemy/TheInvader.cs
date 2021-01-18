using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TheInvader : Enemy
{
    private string Idle = "Idle", Run = "Run";
    private enum AllStates {Wait, Attack}
    AllStates States;
    float DecideTime = 1;
    float LastTimeDecide;
    bool MustDecide = true;
    public int A;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Introduction();
    }

    // Update is called once per frame
    void Update()
    {
        Working();
        if (Active == true && GetHit == false)
        {
            if (MustDecide == true)
            {
               A = Random.Range(1, 4);
            }
            else
            {
                if (Time.time - LastTimeDecide >= DecideTime)
                {
                    MustDecide = true;
                }
            }
            if (A == 1 || A == 2 || A == 3)
            {
                MustDecide = false;
                if (Distance <= 20 && Distance >= 3)
                {
                    MoveTowardsPlayer();
                    ChangeAnimationStates(Run);
                }
                else if (Distance <= 3)
                {
                    StopMoving();
                    Debug.Log("Attack");
                    LastTimeDecide = Time.time;
                    A = 5;
                }
            }
            else if (A == 4)
            {
                A = 5;
                MustDecide = false;
                ChangeAnimationStates(Idle);
                LastTimeDecide = Time.time;
            }
        }
        else
        {
            ChangeAnimationStates(Idle);
        }
        if (GetHit == true)
        {
            ChangeAnimationStates("Hit");
            GetHit = false;
        }
    } 
      public void StartAttack (int gabeo)
    {
        if (gabeo == 0) Attacking = false;
        else if (gabeo == 1)
        {
            Attacking = true;
        }     
    }
 }
