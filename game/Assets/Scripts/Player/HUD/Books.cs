using UnityEngine;

public class Books : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private PauseMenu pause;
  [SerializeField] private CanvasGroup book;

  [Header("Books")]
  [SerializeField] private GameObject StackBook;
  [SerializeField] private GameObject QueueBook;
  [SerializeField] private GameObject ListBook;

  private void Start()
  {
    HideBooks();

    ShowStack();
  }

  private void Update()
  {
    if (book.alpha > 0.1 && Input.GetKeyDown(KeyCode.Escape)) ToggleBooks();

    if (Input.GetKeyDown(KeyCode.B)) ToggleBooks();
  }

  public void ShowQueue()
  {
    ListBook.SetActive(false);
    StackBook.SetActive(false);

    QueueBook.SetActive(true);
  }

  public void ShowList()
  {
    QueueBook.SetActive(false);
    StackBook.SetActive(false);

    ListBook.SetActive(true);
  }

  public void ShowStack()
  {
    QueueBook.SetActive(false);
    ListBook.SetActive(false);

    StackBook.SetActive(true);
  }

  private void ShowBooks()
  {
    book.alpha = 1f;
    book.interactable = true;
    book.blocksRaycasts = true;
  }

  private void HideBooks()
  {
    book.alpha = 0f;
    book.interactable = false;
    book.blocksRaycasts = false;
  }

  private void ToggleBooks()
  {
    pause.TogglePause();

    if (book.alpha > 0.1) HideBooks();
    else ShowBooks();
  }
}
