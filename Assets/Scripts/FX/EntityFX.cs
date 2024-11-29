using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr;

    [Header("PopUp Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    
    

    [Header("FlashFX")]
    [SerializeField] private Material hitMat;
    private Material originalMaterial;


    [Header("AilmentColors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment Particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalhitFX;

    [SerializeField] private GameObject myHealthBar;
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMaterial = sr.material;
        myHealthBar = GetComponentInChildren<HealthBar_UI>().gameObject;
    }

    

    public void createPopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(0, 2, 0);


        GameObject newText = Instantiate(popUpTextPrefab, transform.position+positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    

   

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;

        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(.2f);

        sr.color = currentColor;
        sr.material = originalMaterial;


    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelColorChange()
    {

        CancelInvoke();
        sr.color = Color.white;
        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void igniteFxFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }


    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }





    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }




    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }




    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void createHitFX(Transform _target, bool _criticalHit)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFX;

        if (_criticalHit)
        {
            hitPrefab = criticalhitFX;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingdir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }



        GameObject newHitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity,_target);

        newHitFX.transform.Rotate(hitFxRotation);

        Destroy(newHitFX, .5f);
    }

    
}
