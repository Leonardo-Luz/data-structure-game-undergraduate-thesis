using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] private double maxCooldown;
    private double currentTime;

    public bool CanProduce()
    {
        if ((currentTime += Time.deltaTime) > maxCooldown)
        {
            currentTime = 0;
            return true;
        }
        return false;
    }
}
