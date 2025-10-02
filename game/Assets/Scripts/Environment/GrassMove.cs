using UnityEngine;
using System.Collections;

public class GrassMove : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float swayIntensity = 40f;
    [SerializeField] private float swayRandomness = 1f;

    [Header("Fireflies")]
    [SerializeField] private ParticleSystem fireflyParticles;
    [SerializeField, Range(0f, 1f)] private float fireflyChance = 0.1f;

    private float swayAngle;
    private Vector3 _initialRotation;

    void Start()
    {
        _initialRotation = transform.localEulerAngles;

        fireflyParticles = GetComponent<ParticleSystem>();

        if (fireflyParticles != null)
            fireflyParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Update()
    {
        swayAngle = Mathf.PerlinNoise(Time.time * swaySpeed, transform.position.x + transform.position.z) * swayIntensity +
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
        if (col.CompareTag("Player"))
        {
            setSway(4f, 100f, 0.1f);
            StartCoroutine(ResetSway(0.6f));

            if (fireflyParticles != null && Random.value < fireflyChance)
            {
                fireflyParticles.Play();
            }
        }
    }

    IEnumerator ResetSway(float delay)
    {
        yield return new WaitForSeconds(delay);
        setSway(1.5f, 40f, 1f);
    }
}
