using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
  [SerializeField] private GameObject target;
  [SerializeField] private float fallHeight = 2.6f;

  private void Update()
  {
    if (target.transform.position.y < -fallHeight) Destroy(target);
  }
}
