using UnityEngine;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    #region FIELDS

    [Header("Weapon Properties")]

    public int damage, magazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public bool isAutomatic;

    [Header("References")]

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask target;

    private StarterAssetsInputs _input;
    private bool isShooting, isReadyToShoot, isPlayingAnimation, isChambered;
    private int bulletsShot;

    private Animator animator;

    #endregion

    #region FIELD SERIALIZED

    [Header("Ammunition")]

    [SerializeField]
    private int magazineLeft;

    [SerializeField]
    private int bulletsLeft;

    #endregion

    #region UNITY

    void Awake()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();

        animator = GetComponent<Animator>();

        magazineLeft = magazineSize;
        bulletsLeft = magazineLeft;
        isReadyToShoot = true;
        isPlayingAnimation = false;
        isChambered = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Input();
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Inputs for all weapon functionalities
    /// </summary>
    private void Input()
    {
        // Fire weapon
        if (_input.fireWeapon && isReadyToShoot && isChambered && bulletsLeft > 0)
        {
            // Cancle firing if animation is playing
            if (isPlayingAnimation)
            {
                _input.fireWeapon = false;
                return;
            }

            bulletsShot = bulletsPerTap;
            Fire();
        }

        // Reload wepaon
        if (_input.reloadWeapon && !isPlayingAnimation)
        {
            Reload();
            return;
        }

        if (_input.chargeWeapon && !isPlayingAnimation)
        {
            ChargeWeapon();
            return;
        }

        // Check mag
        if (_input.checkMagainze && !isPlayingAnimation)
        {
            CheckMagazine();
            return;
        }

        // Check chamber
        if (_input.checkChamber && !isPlayingAnimation)
        {
            CheckChamber();
            return;
        }

        // Always life off trigger if no action is taken
        _input.fireWeapon = false;
        return;
    }

    private void Fire()
    {
        Debug.Log("Shot fired");

        isReadyToShoot = false;

        // Bullet spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Direction of spreads
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, target))
        {
            // Due to how Turrent is configured, we need to access parent first
            Transform parentTransform = rayHit.transform.root;

            Debug.Log("Hit object name: " + rayHit.collider.gameObject.name);
            Debug.Log("Parent? -> " + parentTransform.name);

            if (parentTransform.CompareTag("Enemy"))
            {
                parentTransform.GetComponent<HealthController>().Damage(damage);

                Debug.Log("Hit enemy");
            }
        }

        // Ammo handling
        bulletsShot--;
        bulletsLeft--;
        magazineLeft--;
        if (bulletsLeft <= 0)
        {
            isChambered = false;
        }

        Invoke("ResetShot", timeBetweenShooting);

        // Burst fire?
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Fire", timeBetweenShots);
        }

        Debug.Log("Total bullets: " + bulletsLeft);
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
    }

    private void Reload()
    {
        bulletsLeft -= magazineLeft;
        isPlayingAnimation = true;
        animator.Play("Mag Out 0", 0, 0f);
        //Invoke("ReloadFinish", reloadTime);
    }

    // Animation Event
    private void OnReloadFinish()
    {
        magazineLeft = magazineSize;
        bulletsLeft += magazineLeft;

        isPlayingAnimation = false;
        _input.reloadWeapon = false;
    }

    private void CheckMagazine()
    {
        Debug.Log("Bullets in magazine: " + magazineLeft);

        isPlayingAnimation = true;
        animator.Play("Mag Check 0");
    }

    // Animation Event
    private void OnCheckMagazineFinish()
    {
        isPlayingAnimation = false;
        _input.checkMagainze = false;
    }

    private void CheckChamber()
    {
        string msg = isChambered ? "Weapon is chambered" : "Weapon is NOT chambered";
        Debug.Log(msg);

        isPlayingAnimation = true;
        animator.Play("Chamber Check", 0, 0f);
    }

    // Animation Event
    private void OnCheckChamberFinish()
    {
        isPlayingAnimation = false;
        _input.checkChamber = false;
    }

    private void ChargeWeapon()
    {
        isPlayingAnimation = true;
        animator.Play("Chamber Charge", 0, 0f);
    }

    // Animation Event
    private void OnChargeWeaponFinish()
    {
        // Feed a round from magazine, no changes to overall ammo
        // except ammo in magazine, if any
        // Chamber only happens if there's ammo in magazine
        if (!isChambered && magazineLeft > 0)
        {
            isChambered = true;
            magazineLeft--;
        }
        // Eject round if chambered
        else
        {
            // Reduce ammunition
            if (bulletsLeft > 0) bulletsLeft--;
            if (magazineLeft > 0) magazineLeft--;

            // If it's last bullet, render weapon empty
            if (bulletsLeft == 0)
            {
                isChambered = false;
            }
        }
        
        isPlayingAnimation = false;
        _input.chargeWeapon = false;
    }

    #endregion
}
