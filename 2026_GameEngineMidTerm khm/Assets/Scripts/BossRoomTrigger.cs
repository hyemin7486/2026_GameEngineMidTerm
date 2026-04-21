using System.Collections;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject leftDoor;
    public CameraFollow cameraFollow;
    public Transform bossFocusPoint;
    public GameObject bossObject;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(BossIntro(collision.gameObject));
        }
    }

    IEnumerator BossIntro(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller != null)
            controller.enabled = false;

        if (leftDoor != null)
            leftDoor.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if (cameraFollow != null)
        {
            cameraFollow.target = bossFocusPoint;
            cameraFollow.ZoomIn(4f, 0.8f);
        }

        yield return new WaitForSeconds(1.5f);

        if (cameraFollow != null)
            cameraFollow.ShakeCamera(0.4f, 0.2f);

        yield return new WaitForSeconds(0.5f);

        if (bossObject != null)
            bossObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (cameraFollow != null)
        {
            cameraFollow.target = player.transform;
            cameraFollow.ResetZoom(0.8f);
        }

        if (controller != null)
            controller.enabled = true;
    }
}
