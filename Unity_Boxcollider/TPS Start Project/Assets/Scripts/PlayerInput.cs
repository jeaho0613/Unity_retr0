using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string fireButtonName = "Fire1";                 // GetAxis 값
    public string jumpButtonName = "Jump";                  // GetAxis 값
    public string moveHorizontalAxisName = "Horizontal";    // GetAxis 값
    public string moveVerticalAxisName = "Vertical";        // GetAxis 값
    public string reloadButtonName = "Reload";              // Custom GetAxis 값

    public Vector2 moveInput { get; private set; } // 값을 외부에서 쓸 수 없게 프로퍼티 구성
    public bool fire { get; private set; } // 값을 외부에서 쓸 수 없게 프로퍼티 구성
    public bool reload { get; private set; } // 값을 외부에서 쓸 수 없게 프로퍼티 구성
    public bool jump { get; private set; } // 값을 외부에서 쓸 수 없게 프로퍼티 구성

    private void Update()
    {
        // Gameover 상태에서 유저 입력을 제거
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover)
        {
            moveInput = Vector2.zero;
            fire = false;
            reload = false;
            jump = false;
            return;
        }

        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized; // 조이스틱의 최대 크기는 0.7 (조이스틱은 원을 그리면서 입력되므로)
                                                                          // 키보드는 1.0이므로 후에 조이스틱에 대응될 수 있게 normalized (1의 값)으로 변경해줌
                                                                          // sqrMagnitude 연산이 가볍기 때문에 사용

        jump = Input.GetButtonDown(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}