using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    #region FIELD SERIALIZED

    [SerializeField]
    private float m_health = 100f;

    #endregion

    #region UNITY

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion UNITY

    #region METHODS

    public void Damage(float damage)
    {
        m_health -= damage;

        // Kill object getting damaged
        if (m_health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
