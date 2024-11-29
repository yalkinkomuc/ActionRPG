using UnityEngine;
using UnityEngine.UI;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }

    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 LaunchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    private Vector2 finalDir;

    public swordType SwordType = swordType.Regular;

    [Header("BounceInfo")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockedButton;

    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;


    [Header("PierceInfo")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockedButton;

    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity;

    [Header("SpinInfo")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockedButton;

    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [SerializeField] private float bounceVelocity;

    [Header("Passive Skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }

    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    public enum swordType
    {
        Regular,
        Bounce,
        Pierce,
        Spin


    }

    [Header("Aim Dots")]
    [SerializeField] private int numberofDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dotss;



    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
    }

    


    private void SetupGravity()
    {
        if (SwordType == swordType.Bounce)
            swordGravity = bounceGravity;
        else if (SwordType == swordType.Pierce)
            swordGravity = pierceGravity;
        else if (SwordType == swordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        base.Update();


        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * LaunchForce.x, AimDirection().normalized.y * LaunchForce.y);
        
            
        

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dotss.Length; i++)
            {
                dotss[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (SwordType == swordType.Bounce)
        {
            newSwordScript.SetupBounce(true, amountOfBounce, bounceSpeed);
        }
        else if (SwordType == swordType.Pierce)
            newSwordScript.SetupPierce(amountOfPierce);
        else if (SwordType == swordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Unlock Region
    protected override void checkUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockSpinSword();
        UnlockPierceSword();
        UnlockTimeStop();
        UnlockVulnerable();
    }



    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }

    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            SwordType = swordType.Regular;

            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if (bounceUnlockedButton.unlocked)
            SwordType = swordType.Bounce;
    }

    private void UnlockPierceSword()
    {
        if (pierceUnlockedButton.unlocked)
            SwordType = swordType.Pierce;
    }

    private void UnlockSpinSword()
    {
        if (spinUnlockedButton.unlocked)
            SwordType = swordType.Spin;
    }






    #endregion

    #region Aim Region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dotss.Length; i++)
        {
            dotss[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dotss = new GameObject[numberofDots];
        for (int i = 0; i < numberofDots; i++)
        {
            dotss[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dotss[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * LaunchForce.x,
            AimDirection().normalized.y * LaunchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}

