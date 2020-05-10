using UnityEngine;

// value 타입
public struct DamageMessage
{
    public GameObject damager; // 공격 가한 측
    public float amount; // 

    public Vector3 hitPoint;
    public Vector3 hitNormal;
}