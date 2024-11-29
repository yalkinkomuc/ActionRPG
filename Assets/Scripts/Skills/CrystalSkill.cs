using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal Simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }


    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveCrystalButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multiple Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultipleCrystalButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfCrystal;
    [SerializeField] float multiStackCooldown;
    [SerializeField] float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultipleCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleCrystal);


    }
    #region UnlockSkillRegion

    protected override void checkUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultipleCrystal();

    }


    private void UnlockCrystal()
    {
        if(unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if(unlockCrystalInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if(unlockExplosiveCrystalButton.unlocked)
            canExplode = true;
    }

    private void UnlockMovingCrystal()
    {
        if(unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultipleCrystal()
    {
        if(unlockMultipleCrystalButton.unlocked)
            canUseMultiStacks = true;
    }
    #endregion
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if(currentCrystal == null )
        {
            CreateCrystal();
        }

        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position; 
            currentCrystal.transform.position = playerPos;  

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }

            else
            {
                currentCrystal.GetComponent<Crystall_Skill_Controller>()?.FinishCrystal();
            }





            currentCrystal.GetComponent<Crystall_Skill_Controller>()?.FinishCrystal();
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystall_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystall_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);       
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystall_Skill_Controller>().chooseRandomEnemy();
   

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {

            if (crystalLeft.Count > 0)
                Invoke("ResetAbility",useTimeWindow);
            {

                if (crystalLeft.Count == amountOfCrystal)
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position,Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystall_Skill_Controller>().
                SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnemy(newCrystal.transform), player);

                if(crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    private void RefillCrystal()
    {

        int amountToAdd = amountOfCrystal - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
