using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShockStrike_Controller : MonoBehaviour
{

    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private Animator anim;
    private bool trigerred;
    private int damage;

    void Start()
    {
      anim = GetComponentInChildren<Animator>();   
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats = _targetStats;
        damage = _damage;
    }


    // Update is called once per frame
    void Update()
    {
        if(!targetStats)
            return;



        if (trigerred)
            return;

        transform.position = Vector2.MoveTowards(transform.position,targetStats.transform.position,speed*Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);



            Invoke("DamageAndSelfDestroy",.2f);
            trigerred = true;   
            anim.SetTrigger("Hit");
        }
    }
    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }


}
