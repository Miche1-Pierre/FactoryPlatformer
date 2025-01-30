using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("Movement Settings")]
    public float speed = 2f;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textCard;
    public float jumpForce = 4f;

    public bool _isGrounded;
    public bool _isJumping;

    private int score = 0; // Compteur de score
    private int card = 0;

    private void Start()
    {
        // Initialisation des composants
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _rb2D.linearVelocity = new Vector2(velocity.x, _rb2D.linearVelocity.y);
    }

    // 🦘 **Saut**
    public void HandleJump()
    {
        // Le saut classique est activé uniquement quand le joueur est au sol
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpForce);
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

    // 📐 **Détecter une pièce et mettre à jour le score**
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Money"))
        {
            // Incrémente le score
            IncrementScore();
            
            // Supprime la pièce
            Destroy(collider.gameObject);
        }

        if(collider.gameObject.CompareTag("Card"))
        {
            IncrementCard();
            Destroy(collider.gameObject);
        }
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
            if (contact.normal.y > 0.5f)
            {
                _isGrounded = true;
                _isJumping = false;
                return;
            }
        }
    }

    // ✅ **Incrémente le score et met à jour l'affichage**
    private void IncrementScore()
    {
        score++;
        textScore.text = $"Score: {score}";
    }

    private void IncrementCard()
    {
        card++;
        textCard.text = $"Card: {card}";
    }
}