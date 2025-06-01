using Mirror;
using UnityEngine;
using UnityEngine.VFX;

public class FireBallAbility : OnUseAbility
{
    [SerializeField] VisualEffect fireballVFX;
    [SerializeField] private AnimationCurve chargeSampling;

    [SerializeField] private float chargeSpeed;
    [SerializeField] private float unchargeSpeed;

    [SerializeField] private FireballProjectile fireballPrefab;
    [SerializeField] private Transform spawnPoint;
 
    private float _chargeProgress = 0f;
    private bool _isCharging;
    private bool _isUncharging;

    private void Start()
    {
        fireballVFX.gameObject.SetActive(false);
    }
    public override void OnUseButtonDownCLient()
    {
        _isUncharging = false;
        _isCharging = true;
        fireballVFX.gameObject.SetActive(true);
    }

    public override void OnUseButtonUpClient()
    {
        _isCharging = false;
        _isUncharging = true;
    }

    public override void OnUseButtonUpServer()
    {
        if (_chargeProgress > 0.5f)
        {
            var fireball = Instantiate(fireballPrefab, spawnPoint.transform.position, spawnPoint.rotation);
            NetworkServer.Spawn(fireball.gameObject);
            fireball.Initialize(_chargeProgress, transform.root.GetChild(0).forward);
            _chargeProgress = 0f;
            _isCharging = false;
        }
    }


    private void Update()
    {
        
        fireballVFX.SetFloat("Charge", chargeSampling.Evaluate(_chargeProgress));
        if (_isCharging)
        {
            _chargeProgress += chargeSpeed * Time.deltaTime;
            if (_chargeProgress >= 1f )
            {
                _chargeProgress = 1f;
                _isCharging = false;
            }
        }
        else if (_isUncharging )
        {
            _chargeProgress -= unchargeSpeed * Time.deltaTime;
            if( _chargeProgress <= 0f )
            {
                _chargeProgress = 0f;
                _isUncharging = false;
                fireballVFX.gameObject.SetActive( false );
            }
        }
    }
}
