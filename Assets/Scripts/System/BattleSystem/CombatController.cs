using UnityEngine;

public class CombatController : MonoBehaviour
{

    private void OnEnable()
    {
        //BattleManager.TakeDamage += DoDamage;
        //BattleManager.HealDamage += HealDamage;
        //BattleManager.Guard += GuardDamage;
    }

    private void OnDisable()
    {
        //BattleManager.TakeDamage -= DoDamage;
        //BattleManager.HealDamage -= HealDamage;
        //BattleManager.Guard -= GuardDamage;
    }

    public void DoDamage(Creature attacker, Creature attacked, int damage) => attacked.TakeDamage(damage);
    public void HealDamage(Creature healer, Creature healed) => healed.Heal(healer.Intelligence);
    public void GuardDamage(Creature guard) => guard.Guard();
}