using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool chestIsActive;
    private ItemDrop myDropSystem;
    
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void Start()
    {
        chestIsActive = false;
    }

    //private void Update()
    //{
        //OpenChest();
    //}

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null&&chestIsActive)
        {
            //animator.SetBool("ChestOpen",true);
            myDropSystem.GenerateDrop();
            chestIsActive = false;
            
        }
    }
}
