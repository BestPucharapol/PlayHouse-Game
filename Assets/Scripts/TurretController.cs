using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    #region FIELDS

    /// <summary>
    /// If closer than this then turret fires
    /// </summary>
    public float distanceEngagement;

    public float fireRate;

    public GameObject projectile;

    public float projectileForce;

    public float projectileLifetime;

    private float m_distanceToTarget;

    private float nextFire;

    #endregion

    #region FIELDS SERIALIZED

    [Tooltip("What the turret should shoot at")]
    [SerializeField]
    private Transform m_target;

    [Tooltip("Part which rotates")]
    [SerializeField]
    private Transform m_head;

    [Tooltip("Part which shoots")]
    [SerializeField]
    private Transform m_barrel;

    #endregion

    #region UNITY

    private void Awake()
    {
        // Set player as target
        m_target = GameObject.FindGameObjectWithTag("Player").transform;

        // Set which part to rotate
        m_head = transform.Find("Head");

        // Set which part to fire from
        m_barrel = transform.Find("Head").Find("Barrel");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLook();
    }

    #endregion

    #region METHODS

    private void UpdateLook()
    {
        m_distanceToTarget = Vector3.Distance(m_target.position, transform.position);

        // Rotate when within engagement distance
        if (m_distanceToTarget <= distanceEngagement)
        {
            m_head.LookAt(m_target);

            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Fire();
            }
        }
    }

    private void Fire()
    {
        GameObject turretProjectile = Instantiate(projectile, m_barrel.position, m_head.rotation);
        turretProjectile.GetComponent<Rigidbody>().AddForce(m_head.forward * projectileForce);

        Destroy(turretProjectile, projectileLifetime);
    }

    #endregion

}
