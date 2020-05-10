using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 총의 상태
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }
    public State state { get; private set; } // 외부에서 덮어 씌우기 불가능

    private PlayerShooter gunHolder; // 총의 주인
    private LineRenderer bulletLineRenderer; // 총알 괘적

    private AudioSource gunAudioPlayer; // 총 발사 소리
    public AudioClip shotClip; // 소스
    public AudioClip reloadClip; // 소스

    public ParticleSystem muzzleFlashEffect; // 파티클
    public ParticleSystem shellEjectEffect; // 파티클

    public Transform fireTransform; // 발사 위치
    public Transform leftHandMount; // 왼손 위치

    public float damage = 25; // 총의 대미지
    public float fireDistance = 100f; // 총알의 발사 거리

    public int ammoRemain = 100; // 총 남은 탄약
    public int magAmmo; // 현재 탄창량
    public int magCapacity = 30; // 탄창 용량

    public float timeBetFire = 0.12f; // 발사 사이 간격
    public float reloadTime = 1.8f; // 재장전 시간

    [Range(0f, 10f)] public float maxSpread = 3f; // 탄착군의 범위
    [Range(1f, 10f)] public float stability = 1f; // 반동의 증가량
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f; // 
    private float currentSpread; // 현재 탄 퍼짐 정도
    private float currentSpreadVelocity; // 

    private float lastFireTime; // 가장 최근 발사 시점

    private LayerMask excludeTarget; // 총알의 대미지를 받지 않는 오브젝트

    // 컴포넌트 초기화
    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; // 라인 렌더러 수
        bulletLineRenderer.enabled = false;


    }

    // 총 초기화
    public void Setup(PlayerShooter gunHolder)
    {
        this.gunHolder = gunHolder;
        excludeTarget = gunHolder.excludeTarget;
    }

    // 총에 활성화 마다 
    private void OnEnable()
    {
        magAmmo = magCapacity;
        currentSpread = 0f;
        lastFireTime = 0f;
        state = State.Ready;
    }

    // 총에 비활성화 마다
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // aimTarget(조준 대상) Shot을 감싸는 역할
    // - 탄 퍼짐 구현
    public bool Fire(Vector3 aimTarget)
    {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            var fireDirection = aimTarget - fireTransform.position;

            var xError = Utility.GedRandomNormalDistribution(0f, currentSpread);
            var yError = Utility.GedRandomNormalDistribution(0f, currentSpread);

            fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
            fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

            currentSpread += 1f / stability;

            lastFireTime = Time.time;
            Shot(fireTransform.position, fireDirection);

            return true;
        }

        return false;
    }

    // 실제 총 발사
    private void Shot(Vector3 startPoint, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 hitPosition;

        if (Physics.Raycast(startPoint, direction, out hit, fireDistance, ~excludeTarget))
        {
            var target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damager = gunHolder.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;

                target.ApplyDamage(damageMessage);
            }
            else
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);
            }
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = startPoint + direction * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0) state = State.Empty;
    }

    // 이팩트
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAudioPlayer.PlayOneShot(shotClip);

        bulletLineRenderer.enabled = true;
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    // ReloadRoutine을 감싸는 역할
    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());

        return true;
    }

    // 제장전
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        var ammoToFile = Mathf.Clamp(magCapacity - magAmmo, 0, ammoRemain);

        magAmmo += ammoToFile;
        ammoRemain -= ammoToFile;

        state = State.Ready;
    }

    private void Update()
    {
        currentSpread = Mathf.Clamp(currentSpread, 0f, maxSpread);
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);
    }
}