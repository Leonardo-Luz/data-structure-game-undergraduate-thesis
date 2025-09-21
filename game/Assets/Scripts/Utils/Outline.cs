using UnityEngine;

public class Outline : MonoBehaviour
{
  [SerializeField] private Color outlineColor = Color.black;
  [SerializeField]
  private Vector2Int[] outlineOffsets = new Vector2Int[]
  {
      new Vector2Int(1, 0),   // right
      new Vector2Int(-1, 0),  // left
      new Vector2Int(0, 1),   // up
      new Vector2Int(0, -1)   // down
  };

  private GameObject[] outlines = new GameObject[4];
  private SpriteRenderer originalSprite;

  public bool isOutlined = true;

  private void Start()
  {
    originalSprite = GetComponent<SpriteRenderer>();
    if (originalSprite == null)
    {
      Debug.LogError("SpriteRenderer missing!");
      return;
    }

    for (int i = 0; i < outlineOffsets.Length; i++)
    {
      CreateOutline(ref outlines[i], outlineOffsets[i]);
    }
  }

  private void CreateOutline(ref GameObject outlineObj, Vector2Int offset)
  {
    if (!isOutlined || outlineObj != null) return;

    outlineObj = new GameObject("Outline");
    outlineObj.transform.parent = transform;
    outlineObj.transform.localPosition = new Vector3(offset.x / 16f, offset.y / 16f, 1);
    outlineObj.transform.localScale = Vector3.one;

    SpriteRenderer sr = outlineObj.AddComponent<SpriteRenderer>();
    sr.sprite = originalSprite.sprite;
    sr.color = outlineColor;

    Material newMat = new Material(Shader.Find("Sprites/FlickShader"));
    newMat.color = Color.white;

    sr.material = newMat;
    sr.sortingLayerID = originalSprite.sortingLayerID;
    sr.sortingOrder = originalSprite.sortingOrder - 1;
  }

  private void Update()
  {
    originalSprite = GetComponent<SpriteRenderer>();
    UpdateOutlinePositions();
  }

  private void UpdateOutlinePositions()
  {
    for (int i = 0; i < outlineOffsets.Length; i++)
    {
      if (outlines[i] == null) continue;

      Vector2 offset = outlineOffsets[i];

      if (originalSprite.flipX)
        offset.x *= -1;

      outlines[i].transform.localPosition = new Vector3(offset.x / 16f, offset.y / 16f, 1);
      SpriteRenderer sr = outlines[i].GetComponent<SpriteRenderer>();
      sr.sprite = originalSprite.sprite;
      sr.flipX = originalSprite.flipX;
      sr.sortingOrder = originalSprite.sortingOrder;
    }
  }
}

