using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;

    public float minForce = 15f;
    public float maxForce = 30f;
    public float chargingTime = 0.75f;

    private float currentForce; // 현재 힘
    private float chargeSpeed; // 누르고 있는 동안에 충전될 힘
    private bool fired; // 발사 체크

    private void OnEnable() // 컴포넌트가 꺼져있다가 켜지는 순간 발동
    {
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
    }

    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;
    }

    private void Update()
    {
        if (fired == true)
        {
            return;
        }
        powerSlider.value = minForce;

        if(currentForce >= maxForce && !fired) // 힘이 충분해서 바로 발사해야 될 경우
        {
            currentForce = maxForce;
            Fire();
            //발사처리
        }
        else if(Input.GetButtonDown("Fire1")) // 버튼을 처음 눌렀을 때
        {
            //fired = false; // 연속 발사 로직
            currentForce = minForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if(Input.GetButton("Fire1") && !fired) //버튼을 누르고있는 동안
        {
            currentForce = currentForce + chargeSpeed * Time.deltaTime;

            powerSlider.value = currentForce;
        }
        else if (Input.GetButtonUp("Fire1") && !fired) //버튼에서 땐 순간 발사
        {
            // 발사로직
            Fire();
        }
    }

    private void Fire() // 발사 로직
    {
        fired = true;

        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);

        ballInstance.velocity = currentForce * firePos.forward; // 힘 * 방향

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce;
    }


}
