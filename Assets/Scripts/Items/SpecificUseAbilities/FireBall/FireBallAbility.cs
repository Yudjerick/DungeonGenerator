using Assets.Scripts.Items;
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

    [SerializeField] private float cooldown;
    [SerializeField] private float _currentCooldown;
    

    private void Start()
    {
        fireballVFX.gameObject.SetActive(false);
    }
    public override void OnUseButtonDownCLient()
    {
        if(_currentCooldown > 0f)
        {
            return;
        }
        _isUncharging = false;
        _isCharging = true;
        fireballVFX.gameObject.SetActive(true);
    }

    public override void OnUseButtonUpClient()
    {
        if (_chargeProgress > 0.5f)
        {
            _chargeProgress = 0f;
            _currentCooldown = cooldown;
            _isCharging = false;
        }
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
            _currentCooldown = cooldown;
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
        else if(_currentCooldown > 0f)
        {
            _currentCooldown -= Time.deltaTime;
            if(_currentCooldown < 0f)
            {
                _currentCooldown = 0f;
            }
        }
    }

    public override StateBundle GetStateBundle()
    {
        var bundle = new StateBundle();
        bundle.PutFloat("cooldown", _currentCooldown);
        return bundle;
    }

    public override void SetStateFromBundle(StateBundle bundle)
    {
        
        _currentCooldown = bundle.GetFloat("cooldown");
    }
}
