using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    /// <summary>
    /// 호출되면 새로운 씬을 불러온다
    /// </summary>
    /// <param name="levelName">이동하기 원하는 레벨의 이름</param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
