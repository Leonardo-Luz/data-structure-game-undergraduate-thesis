using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float chargeTime = 0.6f;
  [SerializeField] private float stagger = 1.5f;
  [SerializeField] private ProximityDetection attackRange;
  [SerializeField] private FollowingEnemy following;
  [SerializeField] private ParticleSystem sporesExplosion;

  private void Start()
  {
    attackRange.target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    attackRange.onStayProximity += AttackHandler;
  }

  void OnDestroy()
  {
    attackRange.onStayProximity -= AttackHandler;
  }


  private void AttackHandler()
  {
    if (following.isStaggered) return;

    following.StaggerHandler(chargeTime + stagger);
    StartCoroutine(Attack());
  }

  private IEnumerator Attack()
  {
    yield return new WaitForSeconds(chargeTime);
    sporesExplosion.Play();
  }
}
