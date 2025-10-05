using UnityEngine;
using System.Collections.Generic;

public class ElementsIndicator : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 1f, 0);

    [Header("Prefabs")]
    public GameObject elementPrefab;

    [Header("Element Sprites")]
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite airSprite;
    public Sprite emptySprite;

    private List<GameObject> activeIcons = new List<GameObject>();
    private List<Element> currentElements = new List<Element>();

    void Update()
    {
        if (followTarget == null) return;
        transform.position = followTarget.position + offset;

        RefreshIndicator(currentElements);
    }

    public void SetElements(IEnumerable<Element> elements)
    {
        currentElements = new List<Element>(elements);
    }

    private void RefreshIndicator(List<Element> elements)
    {
        while (activeIcons.Count < elements.Count)
        {
            GameObject icon = Instantiate(elementPrefab, transform);
            activeIcons.Add(icon);
        }

        for (int i = 0; i < activeIcons.Count; i++)
        {
            if (i < elements.Count)
            {
                activeIcons[i].SetActive(true);
                activeIcons[i].GetComponent<SpriteRenderer>().sprite = GetSprite(elements[i]);

                float spacing = 0.8f;
                float totalWidth = (elements.Count - 1) * spacing;
                float startX = -totalWidth / 2f;

                activeIcons[i].transform.localPosition = new Vector3(startX + i * spacing, 0f, 0f);
            }
            else
            {
                activeIcons[i].SetActive(false);
            }
        }
    }

    private Sprite GetSprite(Element element)
    {
        switch (element)
        {
            case Element.FIRE: return fireSprite;
            case Element.WATER: return waterSprite;
            case Element.EARTH: return earthSprite;
            case Element.AIR: return airSprite;
            default: return emptySprite;
        }
    }
}
