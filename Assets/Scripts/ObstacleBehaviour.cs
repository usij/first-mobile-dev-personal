using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬을 불러오기 위해

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("게임 재시작 전 대기 시간")]
    public float waitTime = 2.0f;
    [Tooltip("파티클 프리팹")]
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        // 가장 먼저 플레이어와 충돌했는지 체크
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            // 플레이어 제거
            Destroy(collision.gameObject);
            // 대기 시간이 지나면 ResetGame함수 호출
            Invoke("ResetGame", waitTime);
        }
    }
    /// <summary>
    /// 현재 레벨을 다시 시작한다.
    /// </summary>
    private void ResetGame()
    {
        // 현재 레벨을 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// 오브젝트가 탭됐으면 폭발을 생성하고 이 오브젝트를 제거한다.
    /// </summary>
    private void PlayerTouch()
    {
        if(explosion != null)
        {
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f);
        }
        Destroy(this.gameObject);
    }
}