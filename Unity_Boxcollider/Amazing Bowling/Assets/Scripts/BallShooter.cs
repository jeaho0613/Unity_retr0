using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public CamFollow cam;

    public Rigidbody ball; // 볼의 리지드 바디
    public Transform firePos; // 볼이 생성되는 위치
    public Slider powerSlider; // 힘이 보여질 UI
    public AudioSource shootingAudio; // 발사 시 재생할 소리
    public AudioClip fireClip; // 발사 소리
    public AudioClip chargingClip; // 힘모으는 소리

    public float minForce = 15f; // 시작 힘
    public float maxForce = 30f; // 최대 힘
    public float chargingTime = 0.75f; // 힘이 체워지는 시간

    private float currentForce; // 현재 힘
    private float chargeSpeed; // 체워지는 스피드
    private bool fired; // 발사 체크

    // 초기화
    private void OnEnable()
    {
        currentForce = minForce;
        powerSlider.value = 15;
        fired = false;
    }

    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;
    }

    private void Update()
    {
        if (fired) return;

        powerSlider.value = minForce;

        // 힘이 다 체워진 경우
        if(currentForce >= maxForce && !fired)
        {
            currentForce = maxForce;
            // 발사 처리
            Fire();
        }
        // 발사를 누른 처음 순간
        else if(Input.GetButtonDown("Fire1"))
        {
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        // 발사를 누르는 동안
        else if(Input.GetButton("Fire1") && !fired)
        {
            currentForce = currentForce + chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        // 발사 버튼을 땔 때
        else if(Input.GetButtonUp("Fire1") && !fired)
        {
            // 발사처리
            Fire();
        }
    }

    // 발사 로직
    private void Fire()
    {
        fired = true;

        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);

        ballInstance.velocity = currentForce * firePos.forward;
        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce;

        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }
}
