using System.Collections;
using System.Collections.Generic;
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
    private bool isShooting, isReadyToShoot, isReloading;
    private int bulletsLeft, bulletsShot;

    #endregion

    #region UNITY

    void Awake()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();

        bulletsLeft = magazineSize;
        isReadyToShoot = true;
        isReloading = false;
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
        if (_input.fireWeapon && isReadyToShoot && !isReloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Fire();
            _input.fireWeapon = false;
        }

        // Reload wepaon
        if (_input.reloadWeapon && !isReloading)
        {
            Reload();
        }
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

            Debug.Log(rayHit.collider.gameObject.name);
            Debug.Log("Parent? -> " + parentTransform.name);

            if (parentTransform.CompareTag("Enemy"))
            {
                parentTransform.GetComponent<HealthController>().Damage(damage);

                Debug.Log("Hit enemy");
            }
        }

        // Ammo handling
        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        // Burst fire?
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Fire", timeBetweenShots);
        }

        Debug.Log(bulletsLeft);
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
    }

    private void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
        _input.reloadWeapon = false;
    }

    #endregion
}
