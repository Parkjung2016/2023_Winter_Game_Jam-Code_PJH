using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Parkjung2016;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private Collider col;

    public float Power;

    [SerializeField] private bool tail;

    public bool Tail => tail;


    private void Awake()
    {
        col = GetComponent<Collider>();
        EnableCollider(false);
    }

    public void EnableCollider(bool enable)
    {
        if (col == null)
            col = GetComponent<Collider>();
        col.enabled = enable;
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.TryGetComponent(out IApplyDamage applyDamage))
        {
            if (GetComponentInParent<vThirdPersonController>() != null)
            {

                (SceneManagement.Instance.CurrentScene as GameScene)?.SpawnDamageNumber(Power,
                    other.transform.position);
            }

            applyDamage.ApplyDamage(Power);
            EnableCollider(false);
        }
    }
}