using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Bloody Strike")]
public class BloodyStrike : Skill
{
    public float Damage;
    PlayerSet.Player player;
    public override void Use()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSet.Player>();
        Damage = player.BaseDamage + player.BaseDamage * DamageBonus;
        Debug.Log("Use "+SkillName+" Deal "+ Damage);
        LastTimeUse = Time.time;
    }
}
