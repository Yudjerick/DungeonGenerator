using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerHealth>().TakeDamageServer(50f );
    }
}
