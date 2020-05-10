using System;
using UnityEngine;

// 게임속 생명체들이 가질 공통 된 기능
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    
    public event Action OnDeath;
    
    private const float minTimeBetDamaged = 0.1f; // 너무 짧은 주기의 대미지를 무시할 시간
    private float lastDamagedTime;

    protected bool IsInvulnerabe
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }
    
    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    // 데미지 로직
    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        if (IsInvulnerabe || damageMessage.damager == gameObject || dead) return false;

        lastDamagedTime = Time.time;
        health -= damageMessage.amount;
        
        if (health <= 0) Die();

        return true;
    }
    
    // 체력회복 
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;
        
        health += newHealth;
    }
    
    // 사망 로직
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();
        
        dead = true;
    }
}