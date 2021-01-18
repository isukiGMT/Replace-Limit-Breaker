using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Skill : ScriptableObject
{
    public string SkillName;
    public int Cooldown;
    public float DamageBonus;
    public bool CanUse = true;
    public float LastTimeUse = 0;
    public void SetLast()
    {
        LastTimeUse = Time.time;
    }
    public void Default()
    {
        LastTimeUse = 0;
        CanUse = true;
    }
    public abstract void Use();
}
