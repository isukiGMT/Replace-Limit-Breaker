using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSet
{
    public class PlayerStats : MonoBehaviour
    {
        public void AdmitStats(PlayerSet.Player Player)
        {
            Player.MaximumHP = 100 + 10 * Player.BonusHP;
            Player.MaximumEP = 30 + 5 * Player.BonusEP;
            Player.BaseDamage = 10 + 5 * Player.DamageBonus;
            Player.BaseSpeed = 1 + 0.1f * Player.SpeedBonus;
            Player.BaseCriticalChance = 1 * Player.CriticalChanceBonus;
            Player.MaxStamina = 90 + Player.Endurance;
            Player.Stamina = Mathf.Clamp(Player.Stamina, 0, Player.MaxStamina);
            if (Player.CurrentHP == 0)
            {
                Player.CanMove = false;
            }
            if (Player.Stamina == Player.MaxStamina)
            {
                Player.CanChargeStamina = false;
            }
            if (Player._Block == false && Time.time - Player.LastTimeLoseStamina >= 1f)
                if (Player.Stamina + Player.StaminaRegenerateSpeed * Time.deltaTime < Player.MaxStamina) Player.Stamina += Player.StaminaRegenerateSpeed * Time.deltaTime; else Player.Stamina = Player.MaxStamina;
        }
        public void DefaultStats(Player Player)
        {
            Player.CurrentHP = Player.MaximumHP;
            Player.CurrentEP = Player.MaximumEP;
            Player.Stamina = Player.MaxStamina;
        }
    }
}
