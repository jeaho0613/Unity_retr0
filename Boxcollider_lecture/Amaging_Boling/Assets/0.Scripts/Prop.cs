using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour{

    public int score = 5;
    public ParticleSystem explosionParticle;
    public float hp = 10f;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            ParticleSystem instantiate = Instantiate(explosionParticle,transform.position,transform.rotation);
            Destroy(instantiate.gameObject, instantiate.duration);

            AudioSource explosionAudio = instantiate.GetComponent<AudioSource>();
            explosionAudio.Play();
            gameObject.SetActive(false);
        }
    }
}
