using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookController : MonoBehaviour
{
  public static BookController Instance { get; private set; }

  [Header("UI References")]
  [SerializeField] private CanvasGroup book;                 // The whole book UI group
  [SerializeField] private RectTransform overlayParent;      // Tabs overlay container
  [SerializeField] private GameObject bookButtonBorder;
  [SerializeField] private List<BookmarkButton> tabButtons;   // Tabs list

  [Header("Content")]
  [SerializeField] private TextMeshProUGUI contentText;
  [SerializeField] private TextMeshProUGUI pageText;
  [SerializeField] private List<Dialogue> sections;
  [SerializeField] private DialogueManager dialogueManager;

  [Header("Dependencies")]
  [SerializeField] private PauseMenu pause;                      // Reference to your Pause script

  private int currentSection = 0;
  private int currentPage = 0;
  private BookmarkButton currentSelectedTab;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  private void Start()
  {
    if (dialogueManager == null)
      dialogueManager = FindFirstObjectByType<DialogueManager>();

    if (pause == null)
      pause = FindFirstObjectByType<PauseMenu>();

    HideBooks(); // Start hidden

    // Ensure overlay exists
    if (overlayParent == null)
    {
      Canvas canvas = GetComponentInParent<Canvas>();
      if (canvas != null)
      {
        GameObject go = new GameObject("BookOverlay", typeof(RectTransform));
        go.transform.SetParent(canvas.transform, false);
        overlayParent = go.GetComponent<RectTransform>();
        overlayParent.anchorMin = Vector2.zero;
        overlayParent.anchorMax = Vector2.one;
        overlayParent.offsetMin = Vector2.zero;
        overlayParent.offsetMax = Vector2.zero;
      }
    }

    // Reset all tabs
    foreach (var t in tabButtons)
      t.ForceReset();

    // Select first tab
    if (tabButtons.Count > 0)
      SelectTab(tabButtons[0]);

    LoadSection(0);
  }

  private void Update()
  {
    if (book.alpha > 0.1 && Input.GetKeyDown(KeyCode.Escape)) ToggleBooks();

    if (!pause.isPauseMenuOpen && Input.GetKeyDown(KeyCode.B))
      ToggleBooks();
  }

  // =========================
  // BOOK UI VISIBILITY LOGIC
  // =========================

  private void ShowBooks()
  {
    bookButtonBorder.SetActive(true);
    book.alpha = 1f;
    book.interactable = true;
    book.blocksRaycasts = true;
  }

  private void HideBooks()
  {
    bookButtonBorder.SetActive(false);
    book.alpha = 0f;
    book.interactable = false;
    book.blocksRaycasts = false;
  }

  public void ToggleBooks()
  {
    if (pause != null)
      pause.ToggleTime();

    if (book.alpha > 0.1f)
      HideBooks();
    else
      ShowBooks();
  }

  // =========================
  // TAB + PAGE MANAGEMENT
  // =========================

  public void SelectTab(BookmarkButton tab)
  {
    if (currentSelectedTab == tab) return;

    if (currentSelectedTab != null)
      currentSelectedTab.SetSelected(false, overlayParent);

    currentSelectedTab = tab;
    currentSelectedTab.SetSelected(true, overlayParent);

    int idx = tabButtons.IndexOf(tab);
    if (idx >= 0 && idx < sections.Count)
      LoadSection(idx);
  }

  private void LoadSection(int index)
  {
    currentSection = index;
    currentPage = 0;
    UpdateContent();
  }

  private void UpdateContent()
  {
    if (sections == null || sections.Count == 0 || contentText == null || pageText == null) return;

    pageText.text = (currentPage + 1).ToString();

    Dialogue d = sections[currentSection];
    if (d == null || d.lines == null || d.lines.Length == 0)
    {
      contentText.text = "";
      return;
    }

    var line = d.lines[Mathf.Clamp(currentPage, 0, d.lines.Length - 1)];
    contentText.text = (dialogueManager != null && dialogueManager.currentLanguage == Language.English)
        ? line.englishText
        : line.portugueseText;
  }

  public void NextPage()
  {
    if (sections == null || sections.Count == 0) return;
    var count = sections[currentSection].lines.Length;
    if (currentPage < count - 1) currentPage++;
    UpdateContent();
  }

  public void PrevPage()
  {
    if (currentPage > 0) currentPage--;
    UpdateContent();
  }
}

