using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

[System.Serializable]
public class WaveEnemy
{
  public GameObject enemyPrefab;
  public Transform spawnPoint;
}

[System.Serializable]
public class Wave
{
  public WaveEnemy[] enemies;
}

public class Arena : MonoBehaviour
{
  [Header("Camera")]
  [SerializeField] private CinemachineCamera arenaCam;

  [Header("Combat")]
  [SerializeField] private Wave[] waves;
  [SerializeField] private float waveTimeout = 1.0f;
  [SerializeField] private bool isOneTime = true;
  private bool isCompleted = false;

  [Header("Arena Bounds")]
  [SerializeField] private Collider2D arenaConfinerCollider;
  [SerializeField] private Collider2D arenaWalls;

  private int currentWave = -1;
  private List<GameObject> aliveEnemies = new List<GameObject>();
  private Transform player;
  private bool arenaActive = false;

  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    if (arenaWalls != null) arenaWalls.enabled = false;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (arenaActive) return;
    if (!isCompleted && other.CompareTag("Player")) StartArena();
  }

  public void StartArena()
  {
    arenaActive = true;
    if (arenaWalls != null) arenaWalls.enabled = true;
    if (arenaCam != null)
    {
      arenaCam.Priority = 20;
      var confiner = arenaCam.GetComponent<CinemachineConfiner2D>();
      if (confiner != null && arenaConfinerCollider != null)
      {
        confiner.BoundingShape2D = arenaConfinerCollider;
        confiner.InvalidateBoundingShapeCache();
      }
    }
    StartCoroutine(RunArena());
  }

  private IEnumerator RunArena()
  {
    for (int i = 0; i < waves.Length; i++)
    {
      currentWave = i;
      foreach (WaveEnemy we in waves[i].enemies)
      {
        if (we.enemyPrefab == null || we.spawnPoint == null) continue;
        GameObject enemy = Instantiate(we.enemyPrefab, we.spawnPoint.position, we.spawnPoint.rotation);
        aliveEnemies.Add(enemy);
        Health e = enemy.GetComponent<Health>();
        if (e == null) e = enemy.GetComponentInChildren<Health>();
        if (e != null) e.OnDeath += () => aliveEnemies.Remove(enemy);
      }
      yield return new WaitUntil(() => aliveEnemies.Count == 0);
      yield return new WaitForSeconds(waveTimeout);
    }
    EndArena();
  }

  private void EndArena()
  {
    if (arenaWalls != null) arenaWalls.enabled = false;
    if (arenaCam != null) arenaCam.Priority = -1;
    if (isOneTime) isCompleted = true;

    arenaActive = false;
  }

  public void ArenaReset()
  {
    // Stop the running coroutine so it doesn’t continue spawning
    StopAllCoroutines();

    // Destroy any enemies that are still alive
    foreach (GameObject enemy in aliveEnemies)
    {
      if (enemy != null)
        Destroy(enemy);
    }
    aliveEnemies.Clear();

    // Reset state
    currentWave = -1;
    arenaActive = false;

    // Disable arena walls
    if (arenaWalls != null)
      arenaWalls.enabled = false;

    // Reset camera
    if (arenaCam != null)
    {
      arenaCam.Priority = -1;
      var confiner = arenaCam.GetComponent<CinemachineConfiner2D>();
      if (confiner != null)
      {
        confiner.BoundingShape2D = null;
        confiner.InvalidateBoundingShapeCache();
      }
    }

    Debug.Log("[Arena] Reset complete — waiting for player to re-enter.");
  }
}
