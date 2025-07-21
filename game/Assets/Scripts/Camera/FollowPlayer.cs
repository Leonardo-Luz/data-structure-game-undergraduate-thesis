using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private Vector3 offset;
  [SerializeField] private bool lockStartY = false;
  [SerializeField] private float followSpeed = 5f;

  void Start()
  {
    if (player == null)
      player = GameObject.FindGameObjectWithTag("Player").transform;

    if (lockStartY)
      transform.position = new Vector3(
          player.position.x + offset.x,
          player.position.y + offset.y,
          transform.position.z
        );
  }

  void LateUpdate()
  {
    if (lockStartY)
    {
      float newX = Mathf.Lerp(transform.position.x, player.position.x + offset.x, followSpeed * Time.deltaTime);
      transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    else
    {
      Vector3 targetPos = new Vector3(
          player.position.x + offset.x,
          player.position.y + offset.y,
          transform.position.z
        );
      transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
  }
}
