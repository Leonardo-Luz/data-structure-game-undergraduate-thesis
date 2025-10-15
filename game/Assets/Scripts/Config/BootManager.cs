using System.Collections;
using UnityEngine;

public class BootManager : MonoBehaviour
{
  private void Start()
  {
    StartCoroutine(BootRoutine());
  }

  private IEnumerator BootRoutine()
  {
    yield return new WaitForSeconds(0.2f);
    GameManager.Instance.LoadMainMenu();
  }
}
