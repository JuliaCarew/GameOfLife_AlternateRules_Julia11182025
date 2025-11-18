using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = false;
    private Color aliveColor = Color.white;
    private Color deadColor = Color.black;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetState(bool alive)
    {
        isAlive = alive;
        UpdateVisual();
    }

    public bool GetState()
    {
        return isAlive;
    }

    public void SetColors(Color alive, Color dead)
    {
        aliveColor = alive;
        deadColor = dead;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isAlive ? aliveColor : deadColor;
        }
    }
}

