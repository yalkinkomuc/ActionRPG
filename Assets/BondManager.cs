using UnityEngine;

public class BondManager : MonoBehaviour
{
    //[SerializeField] private GameObject TestObjectPrefab;
    public Bond firstBond; // İlk bağ referansı
    public float checkRadius = 15f; // Kontrol mesafesi
    private bool allBondsDisabled = false; // Tüm bağlar devre dışıysa true olur
    [SerializeField] private Chest chest;
    void Start()
    {
        CheckAndDisableIfAllBondsInactive();
    }

    void Update()
    {
        if (!allBondsDisabled && AreNearbyBondsInactive())
        {
            Debug.Log("15 birim mesafede aktif bağ kalmadı!");
            allBondsDisabled = true; // Tekrar oluşmasını engellemek için bayrak
            chest.chestIsActive = true;
            chest.animator.SetBool("ChestOpen",true);
            AudioManager.instance.PlaySfx(26,null); 
        }
    }

    

    private bool AreNearbyBondsInactive()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(firstBond.transform.position, checkRadius);

        foreach (var collider in nearbyColliders)
        {
            Bond bond = collider.GetComponent<Bond>();
            if (bond != null && !bond.isTorchBurning)
            {
                return false; // Hâlâ aktif bir Bond bulundu
            }
        }
        return true; // Tüm yakın Bond'lar inaktif
    }

    private void CheckAndDisableIfAllBondsInactive()
    {
        if (AreNearbyBondsInactive())
        {
            Debug.Log("Oyun başlangıcında bağlar zaten devre dışı.");
            allBondsDisabled = true; // Oyun başında tüm bağlar devre dışıysa bayrağı ayarla
        }
    }

    public bool AllBondsDisabled()
    {
        return allBondsDisabled;
    }
}