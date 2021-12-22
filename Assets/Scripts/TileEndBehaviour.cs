using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 끝에 다다랐을 때
/// 새로운 타일 생성과 해당 타일의 제거를 담당한다.
/// </summary>
public class TileEndBehaviour : MonoBehaviour
{
    [Tooltip("끝에 다다랐을 때 제거하기 전 " + "얼마동안 기다리는가")]
    public float destroyTime = 1.5f;

    public void OnTriggerEnter(Collider col)
    {
        // 먼저 플레이어와 충돌했는지 체크
        if(col.gameObject.GetComponent<PlayerBehaviour>())
        {
            //충돌했다면 새로운 타일을 생성한다
            GameObject.FindObjectOfType<GameController>().SpawnNextTile();

            // 조금 기다린 후 현재 타일 전체를 제거한다.
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
