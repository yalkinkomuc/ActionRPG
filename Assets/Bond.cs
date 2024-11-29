using System;
using UnityEngine;

public class Bond : MonoBehaviour
{
    public float reactivationTime = 3f; // Bağın yeniden aktif olma süresi
    private bool isActive = true; // Bağın aktif olup olmadığını kontrol eder
    public bool isTorchBurning = false;
    private BondManager bondManager;
    private Animator animator;
    

    void Start()
    {
        bondManager = FindObjectOfType<BondManager>();
        animator = GetComponent<Animator>();
        
    }

    public void Cut()
    {
        if (isActive)
        {
            Debug.Log($"{gameObject.name} kesildi!");
            isActive = false;
            animator.SetBool("Burn",true);
            isTorchBurning = true;// Bağı devre dışı bırak
            if (!bondManager.AllBondsDisabled())
            {
                Invoke(nameof(Reactivate), reactivationTime); // Sadece bağlar hala oluşabilirse yeniden aktif et
            }
        }
    }

    private void Reactivate()
    {
        if (!bondManager.AllBondsDisabled())
        {
            isActive = true;
            //gameObject.SetActive(true); // Bağı tekrar aktif yap
            //TESTTTT
            animator.SetBool("Burn",false);
            isTorchBurning = false;
            Debug.Log($"{gameObject.name} yeniden aktif oldu!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SwordSkillController>() != null)
        {
            
            //animator.SetBool("Burn",true);
            Debug.Log("iwork");
        }
    }
}