using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;

    private Camera followCam; // 카메라의 방향을 위한 변수

    public float speed = 6f; // 움직임 속도
    public float jumpVelocity = 20f; // 점프 힘
    [Range(0.01f, 1f)] public float airControlPercent; // 공중에 체류하는 동안의 움직임 조정

    public float speedSmoothTime = 0.1f; // 플레이어의 움직임 변화에 따른 애니메이션 딜레이
    public float turnSmoothTime = 0.1f; // 플레이어의 움직임 변화에 따른 애니메이션 딜레이

    private float speedSmoothVelocity; // 댐핑에 사용 될 변수
    private float turnSmoothVelocity; // 댐핑에 사용 될 변수

    private float currentVelocityY; // 중력을 줄 변수

    public float currentSpeed => // 지면상에 현재 속도
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        followCam = Camera.main;
    }

    private void FixedUpdate()
    {
        // Cam의 화면 조정
        if (currentSpeed > 0.2f || playerInput.fire) Rotate();

        Move(playerInput.moveInput);

        if (playerInput.jump) Jump();
    }

    private void Update()
    {
        UpdateAnimation(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
        var targetSpeed = speed * moveInput.magnitude; // 조이 스틱을 위한 magnitude
        var moveDirection // Normailze로 정규화 해준다. (1로)
            = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);

        // 공중 체류동안 키보드 동작에 딜레이 값
        var smoothTime = characterController.isGrounded ? speedSmoothTime : speedSmoothTime / airControlPercent;

        // currentSpeed -> targetSpeed로 변하는 값을 부드럽게 이어줌, speedSmoothVelocity : 변화량, smoothTime : 지연 시간
        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        currentVelocityY += Time.deltaTime * Physics.gravity.y; // Physics.gravity : 중력 가속도

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded) currentVelocityY = 0f;
    }

    public void Rotate()
    {
        var targetRotation = followCam.transform.eulerAngles.y;

        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void Jump()
    {
        if (!characterController.isGrounded) return;
        currentVelocityY = jumpVelocity;
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        var animationSpeedPercent = currentSpeed / speed; // 벽에 닿을 때 (현재속도 / 속도)을 구해서 %로 만들어줌
        animator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime); // 현재 속도에 따른 애니메이션 %
        animator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime); // 현재 속도에 따른 애니메이션 %
    }
}