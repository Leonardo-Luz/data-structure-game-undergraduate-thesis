using System.Collections;
using UnityEngine;

public class DoubleJumpOrb : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private bool canRespawn = true;
  [SerializeField] private float respawnTimeout = 5f;
  [SerializeField] private Outline outline;

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (!collider.CompareTag("Player")) return;

    collider.GetComponent<PlayerMove>().EnableDoubleJump();

    if (canRespawn)
      StartCoroutine(RespawnRoutine());
    else
      Destroy(gameObject);
  }

  private IEnumerator RespawnRoutine()
  {
    outline.isOutlined = false;
    gameObject.GetComponent<SpriteRenderer>().enabled = false;
    gameObject.GetComponent<Collider2D>().enabled = false;

    yield return new WaitForSeconds(respawnTimeout);

    gameObject.GetComponent<SpriteRenderer>().enabled = true;
    gameObject.GetComponent<Collider2D>().enabled = true;
    outline.isOutlined = true;
  }
}
