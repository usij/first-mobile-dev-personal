using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // ���� �ҷ����� ����

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("���� ����� �� ��� �ð�")]
    public float waitTime = 2.0f;
    [Tooltip("��ƼŬ ������")]
    public GameObject explosion;

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
    /// <summary>
    /// ������Ʈ�� �ǵ����� ������ �����ϰ� �� ������Ʈ�� �����Ѵ�.
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