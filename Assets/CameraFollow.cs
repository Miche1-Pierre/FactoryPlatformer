using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // Le Transform du joueur à suivre
    public Vector3 offset;      // Décalage entre la caméra et le joueur

    private void LateUpdate()
    {
        if (player != null)
        {
            // Mettre à jour la position de la caméra pour qu'elle suive le joueur
            transform.position = player.position + offset;
        }
    }
}