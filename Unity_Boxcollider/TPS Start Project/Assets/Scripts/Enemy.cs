using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : LivingEntity
{
    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }

    private State state;

    private NavMeshAgent agent;
    private Animator animator;

    public Transform attackRoot;
    public Transform eyeTransform; // 시야의 기준 점

    private AudioSource audioPlayer; // 소리 오디오 소스
    public AudioClip hitClip; // 소스 클립
    public AudioClip deathClip; // 소스 클립

    private Renderer skinRenderer; // 좀비 피부색

    public float runSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float damage = 30f; // 공격력
    public float attackRadius = 2f; // 공격 범위
    private float attackDistance; // 공격 거리

    public float fieldOfView = 50f; // 시야각
    public float viewDistance = 10f; // 거리
    public float patrolSpeed = 3f;

    public LivingEntity targetEntity;
    public LayerMask whatIsTarget; // 적 감지 타켓


    private RaycastHit[] hits = new RaycastHit[10];
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>(); // 공격 대상 리스트

    private bool hasTarget => targetEntity != null && !targetEntity.dead;


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
        }

        if (eyeTransform != null)
        {
            var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            var leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }
    }

#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        skinRenderer = GetComponentInChildren<Renderer>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }

    // 적의 스팩을 구성
    public void Setup(float health, float damage, float runSpeed, float patrolSpeed, Color skinColor)
    {
        this.startingHealth = health;
        this.health = health;

        this.damage = damage;
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;

        skinRenderer.material.color = skinColor;

        agent.speed = patrolSpeed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking)
        {
            // 좀비와 플레이어 사이의 거리
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);

            // 그 거리가 attackDistance보다 작으면
            if (distance <= attackDistance)
            {
                BeginAttack(); // 공격 시작
            }
        }

        // desiredVelocity : 실제 이동 속도
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;

        }

        if(state == State.Attacking)
        {
            var dircetion = transform.forward;
            var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

            var size = Physics.SphereCastNonAlloc(attackRoot.position, attackRadius, dircetion, hits, deltaDistance, whatIsTarget);

            for (var i = 0; i < size; i++)
            {
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();
                if(attackTargetEntity !=null && !lastAttackedTargets.Contains(attackTargetEntity))
                {
                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damager = gameObject;
                    if(hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    message.hitNormal = hits[i].normal;

                    attackTargetEntity.ApplyDamage(message);
                    lastAttackedTargets.Add(attackTargetEntity);
                    break;
                }
            }
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                if (state == State.Patrol)
                {
                    state = State.Tracking;
                    agent.speed = runSpeed;
                }
                agent.SetDestination(targetEntity.transform.position); // 찾을 목표 값
            }
            else
            {
                if (targetEntity != null) targetEntity = null;

                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }

                if (agent.remainingDistance <= 1f)
                {
                    var patrolTargetPosition
                        = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolTargetPosition);
                }

                // 가상의 구를 그린다.
                // - 구 안에있는 모든 콜라이더를 가져온다. 단, whatIsTarget 레이어만
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);

                // 가져온 콜라이더를 검사한다.
                foreach (var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform))
                    {
                        continue;
                    }

                    // LivingEntity (생명체) 인지 검사
                    var livingEntity = collider.GetComponent<LivingEntity>();

                    // 생명체이고 죽지 않았다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // targetEntity에 콜라이더를 넘겨줌
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if(targetEntity == null)
        {
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        audioPlayer.PlayOneShot(hitClip);

        return true;
    }

    // 스크립트에서 실행
    public void BeginAttack()
    {
        state = State.AttackBegin;

        agent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    // 애니메이션 event에서 실행
    public void EnableAttack()
    {
        state = State.Attacking;

        lastAttackedTargets.Clear();
    }

    // 애니메이션 event에서 실행
    public void DisableAttack()
    {
        if(hasTarget)
        {
            state = State.Tracking;
        }
        else
        {
            state = State.Patrol;
        }
        agent.isStopped = false;
    }

    private bool IsTargetOnSight(Transform target)
    {
        var dircetion = target.position - eyeTransform.position;
        dircetion.y = eyeTransform.forward.y;

        if (Vector3.Angle(dircetion, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        dircetion = target.position - eyeTransform.position;

        RaycastHit hit;

        if (Physics.Raycast(eyeTransform.position, dircetion, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        GetComponent<Collider>().enabled = false;

        agent.enabled = false;

        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        audioPlayer.PlayOneShot(deathClip);
    }
}