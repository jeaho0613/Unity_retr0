using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float lifeTime = 10f;
    public float explosionRadius = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Physics 물리법칙 클레스 OverLapSphere 원의 시작,구의지름,마스크 지정해주면 범위에 콜라이더를 전부 가져와줌
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);

        for(int i=0; i<colliders.Length;i++)
        {
            Rigidbody targetRididbody = colliders[i].GetComponent<Rigidbody>();

            targetRididbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); // rigidbody 내장기능

            Prop targetProp = colliders[i].GetComponentInParent<Prop>();

            float damage = CalulateDamage(colliders[i].transform.position);

            targetProp.TakeDamage(damage);
        }

        explosionParticle.transform.parent = null;

        explosionParticle.Play();
        explosionAudio.Play();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }

    private float CalulateDamage(Vector3 targetPosition)
    {
        Vector3 explostionToTarget = targetPosition - transform.position;
        float distance = explostionToTarget.magnitude; // Vetor3 내장기능 .magnitude(목표물 과 거리)
        float edgeToCenterDistance = explosionRadius - distance;
        float percentage = edgeToCenterDistance / explosionRadius;
        float damage = maxDamage * percentage;

        damage = Mathf.Max(0, damage); // 체력이 회복되는 걸 막기위해 -% 조정
        return damage;
    }



}
