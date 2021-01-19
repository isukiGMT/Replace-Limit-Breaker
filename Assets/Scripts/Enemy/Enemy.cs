using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float EnemyHP;
    public string EnemyName;
    public float Damage;
    public float MaxAngle;
    public float MaxRadius;
    public bool Active;
    private float heightMultiplayer = 0.5f;
    protected GameObject Player;
    public float Speed;
    public float Distance;
    private NavMeshAgent nav;
    public float LastTimeAttack;
    public float AttackSpeed;
    public bool CanAttack;
    public float Stamina;
    public PlayerSet.Player PlayerScript;
    public float StaminaMin;
    public bool Attacking;
    protected string CurrentStates;
    protected Animator anim;
    public bool GetHit = false;
    // Start is called before the first frame update
    protected virtual void Introduction()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerSet.Player>();
        nav = GetComponent<NavMeshAgent>();
    }
    protected virtual void Working()
    {
        if (Active == false && EnemyHP > 0) inFov(transform, Player.transform, MaxAngle, MaxRadius);
        Distance = Vector3.Distance(Player.transform.position,this.transform.position);
        if (CanAttack == false && Time.time - LastTimeAttack >= AttackSpeed && Stamina >= StaminaMin) CanAttack = true;
        if (EnemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void inFov(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Vector3 directionBetween = (target.transform.position - checkingObject.transform.position).normalized;
        directionBetween.y *= 0;
        RaycastHit hit;
        if (Physics.Raycast(checkingObject.position + Vector3.up * heightMultiplayer, (target.position - checkingObject.position).normalized, out hit, maxRadius))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                float angle = Vector3.Angle(checkingObject.forward + Vector3.up * heightMultiplayer, directionBetween);
                if (angle <= maxAngle)
                {
                    Active = true;
                }
            }
        }

    }
    protected virtual void MoveTowardsPlayer()
    {
        nav.speed = Speed;
        nav.SetDestination(Player.transform.position);
    }
    protected virtual void StopMoving()
    {
        nav.SetDestination(transform.position);
    }
    protected virtual void LookAtPlayer()
    {
        nav.SetDestination(Player.transform.position);
        nav.speed = 0;
    }
    protected void ChangeAnimationStates(string Animation_Name)
    {
        if (CurrentStates == Animation_Name) return;
        else
        {
            CurrentStates = Animation_Name;
            var Animation = Animator.StringToHash(Animation_Name);
            anim.Play(Animation);
            anim.CrossFade(Animation_Name, 0.1f, -1);
        }
    }
    public void EndHit()
    {
        GetHit = false;
        ChangeAnimationStates("Idle");
    }
    public void TakeDamage(float Value)
    {
        if (GetHit == false)
        {
            EnemyHP -= Value;
            GetHit = true;
        }
    }
}
