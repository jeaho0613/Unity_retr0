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

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius); // 자동으로 폭팔력에 작용함

            Prop targetProp = colliders[i].GetComponent<Prop>();

            float damage = CalculateDamage(colliders[i].transform.position);

            targetProp.TakeDamage(damage);
        }


        explosionParticle.transform.parent = null; // 자식 오브젝트 > 부모 해제

        explosionParticle.Play();
        explosionAudio.Play();

        GameManager.instance.OnBallDestroy();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }

    // 폭팔 거리가 가까울수록 데미지 차등 로직
    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explostionToTaget = targetPosition - transform.position;

        float distance = explostionToTaget.magnitude; // magnitude = 길이 값

        float edgeToCenterDistance = explosionRadius - distance; // 원의 끝쪽에서 안쪽으로의 거리

        float percentage = edgeToCenterDistance / explosionRadius; // 안쪽으로 들어간 정도의 % 값

        float damage = maxDamage * percentage;
        damage = Mathf.Max(0, damage); // -damge의 값이 리턴되는 걸 방지, 최소 값은 0

        return damage;
    }
}
