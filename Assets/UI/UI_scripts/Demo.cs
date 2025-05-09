
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public void AddHealth()
    {
        HealthSystem.Instance.AddHealth();
    }
    public void Heal(float hp)
    {
        HealthSystem.Instance.Heal(hp);
    }
    public void Hurt(float dmg)
    {
        HealthSystem.Instance.TakeDamage(dmg);
    }
    public void Button3()
    {
        HealthSystem.Instance.UseMana(10f);
    }
    public void Button4()
    {
        HealthSystem.Instance.RestoreMana(10f);
    }
    public void Button5()
    {
        HealthSystem.Instance.SetMaxHealth(10f); 
    }
    public void Button6()
    {
        HealthSystem.Instance.SetMaxMana(10f);
    }
}
