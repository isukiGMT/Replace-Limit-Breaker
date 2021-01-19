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
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Working();
        float Distance = Vector3.Distance(Player.transform.position,transform.position);
        if (Active == true && GetHit == false)
        { 
                if (Distance <= 30 && Distance > 3)
                {
                    MoveTowardsPlayer();
                    ChangeAnimationStates("Run");
                }
                else if (Distance <= 3)
                {
                    if (CanAttack == true)
                    {
                        anim.SetFloat("RandomShit",0);
                        ChangeAnimationStates("Attack");
                    }
                    if (Attacking == false) LookAtPlayer();
                    else StopMoving();
                }
        }
        else if (Active == false)
        {
            ChangeAnimationStates(Idle);
        }
        else if (GetHit == true)
        {
            ChangeAnimationStates("Hit");
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
    private IEnumerator LoadAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(1);
        CanAttack = true;
    }
    public void EndAttack()
    {
        ChangeAnimationStates("Idle");
    }
 }
