using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    #region FIELDS

    private StarterAssetsInputs _input;

    #endregion

    #region UNITY

    private void Awake()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_input != null)
        {
            if (_input.fireWeapon)
            {
                Fire();
                _input.fireWeapon = false;
            }
        }
    }

    #endregion

    #region METHODS

    void Fire()
    {
        Debug.Log("Firing");
    }

    #endregion
}
