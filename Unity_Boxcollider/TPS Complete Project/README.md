# TPS Project

## Description

### Retr0 Boxcollider 강좌 영상을 통해 TPS 게임을 만듭니다.

## Improt Package

- Cinemachine (카메라 동작)
- Post Processing (화면 필터)
- Timeline (Cinemachine의 의존 패키지)
- **Probuilder (아트 레벨 디자인, 맵 구성등)**

## TimeLine

- **`20-05-09`**
  - 초기씬 구성
  - Lighting setting
    - Auto Generate에 관해서
    - Environmnet Reflections : 환경광 설정
    - Global lllumination : 간접광 설정
      - 매우 높은 성능을 요구
    - Lightmapping Settings
      - 라이팅 처리에 관한 설정
    - Cinemachine Cam
      - FreeLook : Virtual Cam의 확장 버전
        - Follow : 타겟의 위치 값
        - Look At : 타켓의 회전 값
      - Cinemachine Add Extension
        - Collider : 카메라가 충돌체가 있을 경우 타켓을 가리지 않게 조정함
    - Animation 설정
      - player Aiming 설정
        - Motion Time : 수치에 따른 애니메이션 지점
    - Character Controller Component : rigidbody를 사용하지 않은 움직임 구현
      - Script로 움직임을 설정해야함
      - 그 자체로 Collider 기능을 함
      - capsule collider를 확장해 만든 컴포넌트

- **`20-05-10`**
  - 스크립트 관리 방법 (범용성)
  - 크로스헤어의 2중 사용 (화면에 보이는 크로스헤어, 실제 탄이 맞는 크로스헤어)
  - 파티클 효과 사용
  - NavMesh 활용 AI
    - **`기능 관련 메소드 알아보기`**

## 만들면서

- player Input Script 구성
  - Input, Movement script를 따로 구성하여 유지보수에 용이하게 제작
- Vector
  - .magnitude : 인자로 들어온 벡터의 길이를 반환
  - .sqrMagnitude : 인자로 들어온 벡터의 길이의 제곱한 값을 반환
- #if UNITY_EDITOR
  - 전처리 기능
- Physics 관련
  - 매소드 알아보기
- Animation Clip
  - Animation Event를 설정하면 이벤트 Function과 동일한 이름의 함수가 스크립트에 존재한다면
  - 애니메이션의 실행 시간의 위치에 따라 자동으로 실행됨
  - [참고 링크](https://jaeho0613.tistory.com/) : 설명 링크
- Rendering Path 설정 관련
  - Camera 랜더링 설정
  - deferred로 설정하면 MSAA를 동작할 수 없기때문에 off 해줘야 된다.
- post-proess 관련
  