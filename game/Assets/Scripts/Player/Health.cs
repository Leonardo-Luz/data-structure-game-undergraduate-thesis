using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int max;
    private int current;

    private void Start()
    {
        this.current = this.max;
    }

    public void Damage(int value)
    {
        if ((this.current -= value) < 0)
            this.current = 0;
    }

    public void Heal(int value)
    {
        if ((this.current += value) > this.max)
            this.current = this.max;
    }
}
