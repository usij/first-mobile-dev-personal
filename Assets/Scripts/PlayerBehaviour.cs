using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// to process Player moves and input 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// refrence to Rigidbody component
    /// </summary>
    private Rigidbody rb;

    [Tooltip("how to move fast left/right that ball")]
    public float dodgeSpeed = 5f;

    [Tooltip("how to move fast automatically that ball")]
    public float rollSpeed = 5f;

    public enum MobileHorizMovement
    {
        Accelerometer,
        ScreenTouch,
    }

    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;

    [Header("스와이프 속성들")]
    [Tooltip("스와이프에 플레이어가 얼마만큼의 거리를 이동하는가")]
    public float swipeMove = 2f;
    [Tooltip("액션이 일어나기 위해 필요한 스와이프의 거리(인치)")]
    public float minSwipeDistance = 0.25f;
    /// <summary>
    /// 픽셀로 변환한 minSwipeDistance 값을 저장
    /// </summary>
    private float minSwipeDistancePixels;
    /// <summary>
    ///  모바일 터치 이벤트의 시작 포지션을 저장
    /// </summary>
    private Vector2 touchStart;

    [Header("스케일링 속성들-핀치")]
    [Tooltip("줄어들 수 있는 플레이어 최소 사이즈(유니티 유닛)")]
    public float minScale = 0.5f;
    [Tooltip("줄어들 수 있는 플레이어 최대 사이즈(유니티 유닛)")]
    public float maxScale = 3.0f;
    /// <summary>
    /// 플레이어의 현재 스케일
    /// </summary>
    private float currentScale = 1.0f;


    // Start is call before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 얼마나 많은 픽셀을 이동해야 스와이프로 판단할 것인지에 대한 기준
        minSwipeDistancePixels = minSwipeDistance * Screen.dpi; //dpi 값은 1인치당 점들의 숫자
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        //var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        //if (horizMovement == MobileHorizMovement.Accelerometer)
        //{
        //    horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        //}
        //if (Input.touchCount > 0)
        //{
        //    if (horizMovement == MobileHorizMovement.ScreenTouch)
        //    {
        //        Touch touch = Input.touches[0];
        //        horizontalSpeed = CalculateMovement(touch.position);
        //        //SwipeTeleport(touch);
        //        //ScalePlayer();
        //    }
        //}

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            SwipeTeleport(touch);
            TouchObjects(touch.position);
            ScalePlayer();
        }

#endif

    }
    /// <summary>
    /// FixedUpdate
    /// It is good to put time-based functions.
    /// </summary>
    void FixedUpdate()
    {
        //var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        //var verticalSpeed = Input.GetAxis("Vertical") * rollSpeed;
        //rb.AddForce(horizontalSpeed, 0, rollSpeed);
        ////Vector3 getVel = new Vector3(horizontalSpeed, 0, verticalSpeed);
        ////rb.velocity = getVel;

        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

#if UNITY_STANDALONE || UNITY_WEBPLAER || UNITY_EDITOR
        horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

        if (Input.GetMouseButton(0))
        {
            horizontalSpeed = CalculateMovement(Input.mousePosition);
        }
#elif UNITY_IOS || UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            horizontalSpeed = CalculateMovement(touch.position);
        }
#endif
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    /// <summary>
    /// 어느 방향으로 플레이어를 횡으로 이동시킬까
    /// </summary>
    /// <param name="pixelPos"></param>
    /// <returns></returns>
    private float CalculateMovement(Vector3 pixelPos)
    {
        // 0과 1 스케일로 변환한다.
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);
        float xMove = 0f;
        // 왼쪽
        if (worldPos.x < 0.5f)
            xMove = -1f;
        else
            xMove = 1f;

        return xMove * dodgeSpeed;
    }
    /// <summary>
    /// 왼쪽이나 오른쪽으로 스와이프가 일어나면 플레이어를 순간 이동 시킨다.
    /// </summary>
    /// <param name="touch"></param>
    private void SwipeTeleport(Touch touch)
    {
        // 터치가 시작됐는지 확인
        if (touch.phase == TouchPhase.Began)
        {
            // touchStart 설정
            touchStart = touch.position;
        }
        // 터치 끝
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            //x 축상의 터치의 시작과 끝의 차이를 계산
            float x = touchEnd.x - touchStart.x;

            if (Mathf.Abs(x) < minSwipeDistancePixels)
                return;

            Vector3 moveDirection;
            //음수방향으로 이동했다면 왼쪽으로 이동
            if (x < 0)
                moveDirection = Vector3.left;
            else
                moveDirection = Vector3.right;
            RaycastHit hit;

            // 충돌되는 것이 없을 때만 이동한다.
            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }
    /// <summary>
    /// 두 개의 터치 이벤트로 플레이어 스케일을 줄이거나 늘린다.
    /// </summary>
    private void ScalePlayer()
    {
        // 오브젝트를 스케일링한다면 두 개의 터치가 있어야만 한다
        if (Input.touchCount != 2)
            return;
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            // 이전 프레임에서 각 터치의 위치를 찾는다.
            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            // 각 프레임 사이의 터치 규모를 찾는다.
            float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // 각 프레임에서 거리 차이를 찾는다.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // 프레임레이트에 상관없이 변화를 일정하게 유지한다
            float newScale = currentScale - (deltaMagnitudeDiff * Time.deltaTime);

            // 새로운 스케일이 올바른 범주인지 확인
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            // 플레이어의 스케일 업데이트
            transform.localScale = Vector3.one * newScale;
        }
    }
    /// <summary>
    /// 게임 오브젝트를 터치했는지 판단하고, 그렇다면 이벤트를 호출한다.
    /// </summary>
    /// <param name="touch"></param>
    private static void TouchObjects(/*Touch touch*/Vector2 pos)
    {
        // 위치를 광선(ray)로 변환
        Ray touchRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        // 가능한 모든 채널과 충돌할 LayerMask 생성
        int layerMask = ~0;

        // collider가 있는 오브젝트를 터치하고 있나?
        if(Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            // 이 오브젝트에 컴포넌트로 추가돼 있다면 PalyerTouch 함수 호출
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
}
