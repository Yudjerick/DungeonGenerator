using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        var health = other.GetComponent<BaseHealth>();
        print(other.name);
        health?.TakeDamageServer(50f);
    }
}
