using UnityEngine;

public class BorneInteraction : MonoBehaviour
{
    public GameObject door;
    private Animator doorAnimator;
    public float interactionDistance = 2f;
    private bool isPlayerInRange = false;

    private Player player;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        doorAnimator = door.GetComponent<Animator>();
    }

    void Update()
    {

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void TryOpenDoor()
    {
        if (player != null && player.card > 0)
        {
            doorAnimator.SetBool("IsOpened", true);

            player.DecrementCard();

            Debug.Log("Porte ouverte, une carte a été utilisée !");
        }
        else
        {
            Debug.Log("Pas de carte disponible !");
        }
    }
}
