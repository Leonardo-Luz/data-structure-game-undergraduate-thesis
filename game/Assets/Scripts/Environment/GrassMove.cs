using UnityEngine;
using System.Collections;

public class GrassMove : MonoBehaviour
{
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float swayIntensity = 40f;
    [SerializeField] private float swayRandomness = 1f;
    private float swayAngle;

    private Vector3 _initialRotation;

    void Start()
    {
        _initialRotation = transform.localEulerAngles;
    }

    void Update()
    {
        this.swayAngle = Mathf.PerlinNoise(Time.time * swaySpeed, transform.position.x + transform.position.z) * swayIntensity +
                          Mathf.PerlinNoise(Time.time * swaySpeed, transform.position.z - transform.position.x) * swayRandomness;

        transform.localEulerAngles = _initialRotation + Vector3.up * swayAngle;
    }

    private void setSway(float speed, float intensity, float randomness)
    {
	swaySpeed = speed;
	swayIntensity = intensity;
	swayRandomness = randomness;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bool playerCollision = col.CompareTag("Player");
        if (playerCollision)
        {
		setSway(4f, 100f, 0.1f);
		StartCoroutine(ResetSway(0.6f));
        }
    }

    IEnumerator ResetSway(float delay)
    {
        yield return new WaitForSeconds(delay);
	setSway(1.5f, 40f, 1f);
    }
}
