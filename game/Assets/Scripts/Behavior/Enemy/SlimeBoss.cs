using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
    [SerializeField] private ElementsIndicator indicator;
    private SegmentedHealthBarSpawner healthBar;
    private EnemyDamage stats;
    private PatrollingEnemy move;
    private Health health;
    private Grounded grounded;
    private Rigidbody2D rb;

    private float jumpForce = 8f;
    private int lastHP;

    private HashSet<int> triggeredPhases = new HashSet<int>();

    void Start()
    {
        stats = GetComponent<EnemyDamage>();
        health = GetComponent<Health>();
        grounded = GetComponent<Grounded>();
        move = GetComponent<PatrollingEnemy>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<SegmentedHealthBarSpawner>();

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.drawMode = SpriteDrawMode.Simple;

        lastHP = health.GetHP();

        health.OnHealthChanged += HandleHealthChanged;

        stats.SetRandomWeaknesses();
    }

    void Update()
    {
        AutoJump();
    }

    private void AutoJump()
    {
        if (grounded.IsGrounded())
            Jump();
    }

    private void HandleHealthChanged(int curHP, int maxHP)
    {
        if (curHP <= 0)
        {
            lastHP = curHP;
            return;
        }

        if (curHP >= lastHP)
        {
            lastHP = curHP;
            return;
        }

        for (int n = lastHP - 1; n >= curHP; n--)
        {
            if (n % 2 != 0)
                continue;

            if (triggeredPhases.Contains(n))
                continue;

            triggeredPhases.Add(n);
            StartCoroutine(ApplyPhaseChange());
        }

        lastHP = curHP;
    }

    private IEnumerator ApplyPhaseChange()
    {
        yield return new WaitForSeconds(0.1f);
        stats.SetRandomWeaknesses();
        jumpForce = Mathf.Max(2f, jumpForce / 1.6f);
        move.IncreaseSpeed(1f);
        move.wallCheckDistance -= 0.5f;
        healthBar.ReSetOffset(new Vector3(0, healthBar.offset.y + 0.5f, 0));
        indicator.offset = new Vector3(0, indicator.offset.y - 0.4f, 0);

        Transform t = GetComponent<Transform>();

        t.localScale = new Vector3(
            t.localScale.x - (t.localScale.x > 0 ? 1f : -1f),
            t.localScale.y - 1f,
            1f
        );
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnHealthChanged -= HandleHealthChanged;
    }
}
