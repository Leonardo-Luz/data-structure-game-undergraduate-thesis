using UnityEngine;
using TMPro;
using System.Collections;

public class ToastManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private CanvasGroup toastCanvas;
  [SerializeField] private TextMeshProUGUI toastText;
  [SerializeField] private DialogueManager languageManager;

  [Header("Settings")]
  [SerializeField] private float toastTime = 3f;
  [SerializeField] private float fadeDuration = 0.4f;

  private Coroutine currentRoutine;

  private void Start()
  {
    if (languageManager == null)
      languageManager = FindFirstObjectByType<DialogueManager>();

    HideInstant();
  }

  public void Toast(DialogueLine line)
  {
    string text = languageManager.currentLanguage == Language.English
        ? line.englishText
        : line.portugueseText;

    if (currentRoutine != null)
      StopCoroutine(currentRoutine);

    currentRoutine = StartCoroutine(ToastRoutine(text));
  }

  private IEnumerator ToastRoutine(string text)
  {
    toastText.text = text;
    yield return StartCoroutine(FadeCanvas(toastCanvas, 0f, 1f, fadeDuration));

    yield return new WaitForSeconds(toastTime);

    yield return StartCoroutine(FadeCanvas(toastCanvas, 1f, 0f, fadeDuration));

    HideInstant();
  }

  private IEnumerator FadeCanvas(CanvasGroup group, float from, float to, float duration)
  {
    float elapsed = 0f;
    group.alpha = from;

    if (to > from)
    {
      group.interactable = true;
      group.blocksRaycasts = true;
    }

    while (elapsed < duration)
    {
      elapsed += Time.deltaTime;
      group.alpha = Mathf.Lerp(from, to, elapsed / duration);
      yield return null;
    }

    group.alpha = to;

    if (to < from)
    {
      group.interactable = false;
      group.blocksRaycasts = false;
    }
  }

  private void HideInstant()
  {
    toastCanvas.alpha = 0f;
    toastCanvas.interactable = false;
    toastCanvas.blocksRaycasts = false;
  }
}
