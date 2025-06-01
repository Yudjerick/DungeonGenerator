using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FireballProjectile : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChargeHook))]
    public float charge;
    [SyncVar]
    public Vector3 direction;

    [SerializeField] private float shotSpeed;

    [SerializeField] VisualEffect fireballVFX;
    [SerializeField] private AnimationCurve chargeSampling;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    void Start()
    {
        
    }

    public override void OnStartServer()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        UpdatePosition();
    }

    [Server]
    public void UpdatePosition()
    {
        transform.Translate(direction.normalized  * shotSpeed * Time.deltaTime, Space.World);
    }

    [Server]
    public void Initialize(float chargeValue, Vector3 directionVal)
    {
        charge = chargeValue;
        direction = directionVal;
    }

    private void ChargeHook(float oldVal, float newVal)
    {
        fireballVFX.SetFloat("Charge", chargeSampling.Evaluate(newVal));
    }

    [Server]
    public void Explode()
    {
        print("Exploding");
        RpcExplode();
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            var health = collider.gameObject.GetComponent<BaseHealth>();
            if(health != null)
            {
                health.TakeDamageServer(explosionDamage);
            }
                
        }
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    [ClientRpc]
    public void RpcExplode()
    {
        var explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 0.4f);
        gameObject.SetActive(false);
    }

}
