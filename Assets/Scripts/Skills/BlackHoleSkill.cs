using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    [SerializeField] public bool BlackHoleUnlocked;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackHole_SkillController currentBlackhole;

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            BlackHoleUnlocked = true;
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackhole = newBlackHole.GetComponent<BlackHole_SkillController>();
        currentBlackhole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,blackHoleDuration);

        AudioManager.instance.PlaySfx(6, player.transform);
        AudioManager.instance.PlaySfx(3, player.transform);

    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);

    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;


        if(currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }
    
    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
            
    }

    protected override void checkUnlock()
    {
        base.checkUnlock();

        UnlockBlackHole();
    }
}
