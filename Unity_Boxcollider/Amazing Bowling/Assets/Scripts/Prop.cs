using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int score = 5;
    public ParticleSystem explostionParticle;
    public float hp = 10f;

    [System.Obsolete]
    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            ParticleSystem instance = Instantiate(explostionParticle, transform.position, transform.rotation);

            AudioSource explostionAudio = instance.GetComponent<AudioSource>();
            explostionAudio.Play();

            GameManager.instance.AddSocre(score);

            Destroy(instance.gameObject, instance.duration);
            gameObject.SetActive(false);
        }
    }
}
