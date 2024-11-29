using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("RestoreWithParry")]
    [SerializeField] private UI_SkillTreeSlot restoreWithParryUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreWithParryUnlocked {get; private set; }

    [Header("ParryWithMirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }   


    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreWithParryUnlocked)
        {
            int restoreAmount =Mathf.RoundToInt(player.stats.getMaxHealthValue()*restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);

        }
    }
    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(unlockParry);
        restoreWithParryUnlockButton.GetComponent<Button>().onClick.AddListener(restoreWithParry);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(parryWithMirage);
    }

    protected override void checkUnlock()
    {
        unlockParry();
        restoreWithParry();
        parryWithMirage();
    }

    private void unlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;

    }

    private void restoreWithParry()
    {
        if (restoreWithParryUnlockButton.unlocked)
            restoreWithParryUnlocked = true;

    }

    private void parryWithMirage()
    {
        if(parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
