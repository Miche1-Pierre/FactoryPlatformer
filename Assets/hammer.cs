using UnityEngine;
public class HammerColliderControl : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public float speed = 1.54f;
    public float maxHeight = 1f;

    private float currentHeight = 0f;
    private bool movingDown = true;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (movingDown)
        {
            currentHeight -= speed * Time.deltaTime;
            if (currentHeight <= -maxHeight)
            {
                movingDown = false;
            }
        }
        else
        {
            currentHeight += speed * Time.deltaTime;
            if (currentHeight >= 0)
            {
                movingDown = true;
            }
        }

        boxCollider.size = new Vector2(boxCollider.size.x, Mathf.Abs(currentHeight));
        boxCollider.offset = new Vector2(boxCollider.offset.x, currentHeight / 2f);
    }
}
