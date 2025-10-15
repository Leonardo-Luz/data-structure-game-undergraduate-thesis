using UnityEngine;
using System.Collections;

public class PickupTalk : MonoBehaviour
{
  private ConsumableObject consumable;

  [SerializeField] private Dialogue dialogue;
  [SerializeField] private GameObject spawnTalkPrefab;

  private void Start()
  {
    consumable = GetComponent<ConsumableObject>();
    consumable.onPickup += StartTalk;
  }

  private void StartTalk()
  {
    StartCoroutine(SpawnTalkNextFrame());
  }

  private IEnumerator SpawnTalkNextFrame()
  {
    yield return new WaitForSeconds(0.1f);

    if (spawnTalkPrefab != null)
    {
      GameObject prefab = Instantiate(spawnTalkPrefab);
      prefab.GetComponent<SpawnTalk>().Run(dialogue);
    }

    Destroy(consumable.gameObject);
  }
}
