using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬을 불러오기 위해

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("게임 재시작 전 대기 시간")]
    public float waitTime = 2.0f;

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
}