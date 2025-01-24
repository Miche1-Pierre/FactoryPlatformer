using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float jumpForce = 4f;

    private bool _isGrounded;
    private bool _isJumping;

    private Tilemap _tilemap;  // Référence au Tilemap de la scène

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Récupérer le Tilemap de la scène
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAnimations();
        FlipSprite();
    }

    // 🔄 **Déplacement horizontal**
    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = _rb2D.linearVelocity;

        velocity.x = moveX * speed;
        _rb2D.linearVelocity = new Vector2(velocity.x, _rb2D.linearVelocityY);
    }

    // 🦘 **Saut**
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocityX, jumpForce);
            _isJumping = true;
            _isGrounded = false;
            _animator.SetTrigger("Jump");
        }
    }

    // 🎥 **Animations**
    private void HandleAnimations()
    {
        float moveX = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
        _animator.SetBool("IsRunning", _isGrounded && moveX > 0);
        _animator.SetBool("IsJumping", !_isGrounded);
    }

    // 🔄 **Changer la direction du sprite**
    private void FlipSprite()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveX > 0)
            _spriteRenderer.flipX = false;
        else if (moveX < 0)
            _spriteRenderer.flipX = true;
    }

    // 📐 **Détecter le contact avec le sol**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGrounded(collision);
    }

    // 📐 **Détecter si le joueur reste au sol**
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckGrounded(collision);
    }

    // 📐 **Gérer la sortie de contact avec le sol**
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    // 🧪 **Vérification de la normale de la surface pour déterminer si c'est le sol**
    private void CheckGrounded(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // ✅ Seul le contact avec une normale pointant vers le haut est considéré comme le sol
            if (contact.normal.y > 0.5f)
            {
                _isGrounded = true;
                _isJumping = false;
                return;
            }
        }
    }
}
