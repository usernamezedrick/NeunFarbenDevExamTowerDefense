using System.Collections;
using UnityEngine;

public class EnemyDisappear : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f)); // Random disappearance time

            // Disappear
            spriteRenderer.enabled = false;
            enemyCollider.enabled = false;

            yield return new WaitForSeconds(1f); // Stay invisible for 1 second

            // Reappear
            spriteRenderer.enabled = true;
            enemyCollider.enabled = true;
        }
    }
}
