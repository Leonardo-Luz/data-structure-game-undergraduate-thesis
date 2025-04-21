using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    void Start()
    {
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
