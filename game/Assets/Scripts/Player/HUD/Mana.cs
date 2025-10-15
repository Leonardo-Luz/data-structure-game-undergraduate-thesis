using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    [SerializeField] private Image manaBar;
    [SerializeField] private GenerateElement elementGen;

    private void Start()
    {
        if (elementGen == null)
            elementGen = GetComponent<GenerateElement>();
    }

    private void Update()
    {
        if (elementGen == null || manaBar == null) return;

        UpdateBar(elementGen.curMana, elementGen.maxMana);
    }

    private void UpdateBar(float cur, float max)
    {
        if (max <= 0f) return;

        float normalized = Mathf.Clamp01(cur / max);
        manaBar.fillAmount = normalized;
    }
}
