using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Game PlayController
/// </summary>
public class GameController : MonoBehaviour
{
    [Tooltip("생성하고자하는 타일 참조")]
    public Transform tile;

    [Tooltip("첫번째 타일이 생성되는 곳")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("사전에 몇 개의 타일이 생성되어야 하는가")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    /// <summary>
    /// 다음 타일이 생성되는 곳
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// 다음 타일의 회전
    /// </summary>
    private Quaternion nextTileRotation;

    /// <summary>
    /// Start is called before the first frame upstae
    /// </summary>
    void Start()
    {
        // 시작 포인트 설정
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for(int i=0;i<initSpawnNum;++i)
        {
            SpawnNextTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 특정 위치에 타일을 생성하고 다음 위치를 설정한다.
    /// </summary>
    public void SpawnNextTile()
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        //다음 아이템을 생성할 위치와 회전 값을 알아낸다.
        var nextTile = newTile.Find("Next Spawn Point");
        Vector3 offset = new Vector3(0, 0, 5);
        nextTileLocation = nextTile.position + offset;
        nextTileRotation = nextTile.rotation;
    }
}
