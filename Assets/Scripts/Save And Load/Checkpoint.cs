using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string ID;
    public bool activationStatus;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate Checkpoint ID")]
    private void generateID()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            activateCheckpoint();
    }

    public void activateCheckpoint()
    {
        if(activationStatus == false)
        AudioManager.instance.PlaySfx(5,transform);

        activationStatus = true;
        anim.SetBool("active",true);

    }
}
