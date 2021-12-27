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

    [Header("�������� �Ӽ���")]
    [Tooltip("���������� �÷��̾ �󸶸�ŭ�� �Ÿ��� �̵��ϴ°�")]
    public float swipeMove = 2f;
    [Tooltip("�׼��� �Ͼ�� ���� �ʿ��� ���������� �Ÿ�(��ġ)")]
    public float minSwipeDistance = 0.25f;
    /// <summary>
    /// �ȼ��� ��ȯ�� minSwipeDistance ���� ����
    /// </summary>
    private float minSwipeDistancePixels;
    /// <summary>
    ///  ����� ��ġ �̺�Ʈ�� ���� �������� ����
    /// </summary>
    private Vector2 touchStart;

    [Header("�����ϸ� �Ӽ���-��ġ")]
    [Tooltip("�پ�� �� �ִ� �÷��̾� �ּ� ������(����Ƽ ����)")]
    public float minScale = 0.5f;
    [Tooltip("�پ�� �� �ִ� �÷��̾� �ִ� ������(����Ƽ ����)")]
    public float maxScale = 3.0f;
    /// <summary>
    /// �÷��̾��� ���� ������
    /// </summary>
    private float currentScale = 1.0f;


    // Start is call before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // �󸶳� ���� �ȼ��� �̵��ؾ� ���������� �Ǵ��� �������� ���� ����
        minSwipeDistancePixels = minSwipeDistance * Screen.dpi; //dpi ���� 1��ġ�� ������ ����
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
    /// ��� �������� �÷��̾ Ⱦ���� �̵���ų��
    /// </summary>
    /// <param name="pixelPos"></param>
    /// <returns></returns>
    private float CalculateMovement(Vector3 pixelPos)
    {
        // 0�� 1 �����Ϸ� ��ȯ�Ѵ�.
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);
        float xMove = 0f;
        // ����
        if (worldPos.x < 0.5f)
            xMove = -1f;
        else
            xMove = 1f;

        return xMove * dodgeSpeed;
    }
    /// <summary>
    /// �����̳� ���������� ���������� �Ͼ�� �÷��̾ ���� �̵� ��Ų��.
    /// </summary>
    /// <param name="touch"></param>
    private void SwipeTeleport(Touch touch)
    {
        // ��ġ�� ���۵ƴ��� Ȯ��
        if (touch.phase == TouchPhase.Began)
        {
            // touchStart ����
            touchStart = touch.position;
        }
        // ��ġ ��
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            //x ����� ��ġ�� ���۰� ���� ���̸� ���
            float x = touchEnd.x - touchStart.x;

            if (Mathf.Abs(x) < minSwipeDistancePixels)
                return;

            Vector3 moveDirection;
            //������������ �̵��ߴٸ� �������� �̵�
            if (x < 0)
                moveDirection = Vector3.left;
            else
                moveDirection = Vector3.right;
            RaycastHit hit;

            // �浹�Ǵ� ���� ���� ���� �̵��Ѵ�.
            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }
    /// <summary>
    /// �� ���� ��ġ �̺�Ʈ�� �÷��̾� �������� ���̰ų� �ø���.
    /// </summary>
    private void ScalePlayer()
    {
        // ������Ʈ�� �����ϸ��Ѵٸ� �� ���� ��ġ�� �־�߸� �Ѵ�
        if (Input.touchCount != 2)
            return;
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            // ���� �����ӿ��� �� ��ġ�� ��ġ�� ã�´�.
            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            // �� ������ ������ ��ġ �Ը� ã�´�.
            float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // �� �����ӿ��� �Ÿ� ���̸� ã�´�.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // �����ӷ���Ʈ�� ������� ��ȭ�� �����ϰ� �����Ѵ�
            float newScale = currentScale - (deltaMagnitudeDiff * Time.deltaTime);

            // ���ο� �������� �ùٸ� �������� Ȯ��
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            // �÷��̾��� ������ ������Ʈ
            transform.localScale = Vector3.one * newScale;
        }
    }
    /// <summary>
    /// ���� ������Ʈ�� ��ġ�ߴ��� �Ǵ��ϰ�, �׷��ٸ� �̺�Ʈ�� ȣ���Ѵ�.
    /// </summary>
    /// <param name="touch"></param>
    private static void TouchObjects(/*Touch touch*/Vector2 pos)
    {
        // ��ġ�� ����(ray)�� ��ȯ
        Ray touchRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        // ������ ��� ä�ΰ� �浹�� LayerMask ����
        int layerMask = ~0;

        // collider�� �ִ� ������Ʈ�� ��ġ�ϰ� �ֳ�?
        if(Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            // �� ������Ʈ�� ������Ʈ�� �߰��� �ִٸ� PalyerTouch �Լ� ȣ��
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
}
