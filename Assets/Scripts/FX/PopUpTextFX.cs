using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float Speed;
    [SerializeField] private float disappearingSpeed;
    [SerializeField] private float colorDisappearingSpeed;

    [SerializeField] private float lifeTime;

    private float textTimer;
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), Speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if(textTimer < 0 )
        {
            float alpha = myText.color.a - colorDisappearingSpeed * Time.deltaTime;

            myText.color = new Color(myText.color.r,myText.color.g,myText.color.b,alpha);

            if (myText.color.a < 50)
                Speed = disappearingSpeed;
            
            if(myText.color.a <=0)
                Destroy(gameObject);

        }
    }
}
