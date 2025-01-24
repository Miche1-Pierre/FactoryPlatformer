using UnityEngine;

public class PlayerSwimming : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Animator _animator;

    [Header("Water Settings")]
    public Collider2D waterZone;
    public float swimSpeed = 2f;
    public float buoyancyForce = 25f;
    public float surfaceOscillationStrength = 10f;
    public float surfaceOscillationSpeed = 5f;
    public float verticalDrag = 10f;
    public float horizontalDrag = 1.5f;
    public float jumpOutOfWaterForce = 7f;

    private bool _isInWater;
    private bool _isAtSurface;
    private float _waterSurfaceY;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectWater();

        if (_isInWater)
        {
            HandleSwimming();
        }

        UpdateAnimations();

        // Ajout du boost pour sortir de l'eau quand on appuie sur espace
        if (_isAtSurface && Input.GetKeyDown(KeyCode.Space))
        {
            ApplyBoostOutOfWater();
        }
    }

    private void FixedUpdate()
    {
        if (_isInWater)
        {
            ApplyBuoyancy();
        }
    }

    private void DetectWater()
    {
        // Vérifie si le joueur est dans la zone d'eau
        if (waterZone != null)
        {
            _isInWater = waterZone.bounds.Contains(transform.position);

            if (_isInWater)
            {
                _waterSurfaceY = waterZone.bounds.max.y;
            }
            else
            {
                _isAtSurface = false;
            }
        }
    }

    private void HandleSwimming()
    {
        // Mouvement horizontal
        float moveX = Input.GetAxisRaw("Horizontal");
        _rb2D.linearVelocity = new Vector2(moveX * swimSpeed, _rb2D.linearVelocity.y);

        // Mouvement vertical (descente sous l'eau uniquement si Input "descend")
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, -swimSpeed);
        }
    }

    private void ApplyBuoyancy()
    {
        float playerDepth = _waterSurfaceY - transform.position.y;

        if (playerDepth > 0)
        {
            // Applique la force de flottabilité si sous l'eau
            float buoyancy = buoyancyForce * playerDepth;
            _rb2D.AddForce(Vector2.up * buoyancy, ForceMode2D.Force);

            // Simule une résistance pour des mouvements plus réalistes
            _rb2D.linearVelocity = new Vector2(
                _rb2D.linearVelocity.x * (1f - Time.fixedDeltaTime * horizontalDrag),
                _rb2D.linearVelocity.y * (1f - Time.fixedDeltaTime * verticalDrag)
            );

            _isAtSurface = Mathf.Abs(playerDepth) < 0.5f && _rb2D.linearVelocity.y > -0.1f;

            if (_isAtSurface)
            {
                // Ajoute un flottement naturel à la surface
                float oscillation = Mathf.Sin(Time.time * surfaceOscillationSpeed) * surfaceOscillationStrength;
                _rb2D.AddForce(Vector2.up * oscillation, ForceMode2D.Force);
            }
        }
    }

    private void ApplyBoostOutOfWater()
    {
        // Ajoute une force pour propulser le joueur hors de l'eau
        _rb2D.AddForce(Vector2.up * jumpOutOfWaterForce, ForceMode2D.Impulse);
    }

    private void UpdateAnimations()
    {
        if (_isInWater)
        {
            bool isMoving = Mathf.Abs(_rb2D.linearVelocity.x) > 0.1f || Mathf.Abs(_rb2D.linearVelocity.y) > 0.1f;

            _animator.SetBool("IsSwimming", isMoving);
            _animator.SetBool("IsIdle", !isMoving);
        }
        else
        {
            _animator.SetBool("IsSwimming", false);
            _animator.SetBool("IsIdle", false);
        }
    }

    private void OnDrawGizmos()
    {
        if (waterZone != null)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(waterZone.bounds.center, waterZone.bounds.size);
        }
    }
}