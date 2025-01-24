using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Animator _animator;

    private bool _isClimbing = false;
    private bool _isInteracting = false;
    private string _currentInteractionType = "";

    [Header("Climbing Settings")]
    public float climbSpeed = 3f;

    [Header("Layer Settings")]
    public LayerMask platformLayer;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (_isInteracting && _currentInteractionType == "Ladders")
        {
            HandleClimbing();
        }
        else
        {
            ResetAnimations();
        }
    }

    private void HandleClimbing()
    {
        // Désactiver la gravité pour permettre un mouvement fluide sur l'échelle
        _rb2D.gravityScale = 0;

        // Ignorer les collisions avec les plateformes pendant l'escalade
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);

        // Obtenir l'entrée verticale du joueur
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical != 0) // Le joueur monte ou descend
        {
            _isClimbing = true;
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocityX, vertical * climbSpeed);

            // Détermine si le joueur monte ou descend pour ajuster l'animation
            _animator.SetBool("IsClimbing", true);
            _animator.SetFloat("ClimbingDirection", vertical); // 1 pour montée, -1 pour descente
        }
        else
        {
            _isClimbing = true;
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocityX, 0);

            _animator.SetBool("IsClimbing", true);
            _animator.SetFloat("ClimbingDirection", 0);
        }
    }

    private void ResetAnimations()
    {
        // Réinitialise l'état de grimpe
        _isClimbing = false;
        _animator.SetBool("IsClimbing", false);
        _animator.SetFloat("ClimbingDirection", 0);

        // Réactiver les collisions avec les plateformes après avoir quitté l'échelle
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le joueur entre en contact avec une échelle
        if (collision.CompareTag("Ladders"))
        {
            _isInteracting = true;
            _currentInteractionType = "Ladders";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Lorsque le joueur quitte l'échelle
        if (collision.CompareTag("Ladders"))
        {
            _isInteracting = false;
            _currentInteractionType = "";
            _isClimbing = false;

            // Réactiver la gravité lorsque le joueur quitte l'échelle
            _rb2D.gravityScale = 1;

            // Réactiver les collisions avec les plateformes
            Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);

            // Réinitialise les animations
            ResetAnimations();
        }
    }
}