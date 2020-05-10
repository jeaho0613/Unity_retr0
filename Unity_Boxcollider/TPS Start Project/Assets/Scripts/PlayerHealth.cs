using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;
    private AudioSource playerAudioPlayer;

    public AudioClip deathClip;
    public AudioClip hitClip;


    private void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // base (LivingEntity의 OnEnable을 먼저 실행
        UpdateUI();
    }
    
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth); // base (LivingEntity의 RestoreHealth을 먼저 실행
        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }
    
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, // 이팩트 생성 지점
                                             damageMessage.hitNormal, // 이팩트 회전 값
                                             transform, // 위치
                                             EffectManager.EffectType.Flesh); // 생성 이팩트 타입

        playerAudioPlayer.PlayOneShot(hitClip);

        UpdateUI();
        
        return true;
    }
    
    public override void Die()
    {
        base.Die(); // base (LivingEntity의 Die을 먼저 실행

        playerAudioPlayer.PlayOneShot(deathClip);
        animator.SetTrigger("Die");

        UpdateUI();
    }
}