using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // ���� �ҷ����� ����

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("���� ����� �� ��� �ð�")]
    public float waitTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ���� �÷��̾�� �浹�ߴ��� üũ
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            // �÷��̾� ����
            Destroy(collision.gameObject);
            // ��� �ð��� ������ ResetGame�Լ� ȣ��
            Invoke("ResetGame", waitTime);
        }
    }
    /// <summary>
    /// ���� ������ �ٽ� �����Ѵ�.
    /// </summary>
    private void ResetGame()
    {
        // ���� ������ �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}