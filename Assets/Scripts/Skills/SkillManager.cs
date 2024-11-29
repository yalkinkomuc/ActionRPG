using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dash {  get; private set; }
    public Clone_Skill clone { get; private set; }
    public SwordSkill sword { get; private set; }
    public CrystalSkill crystal { get; private set; }
    public BlackHoleSkill blackHole { get; private set; }
    public Parry_Skill parry { get; private set; }
    public Dodge_Skill dodge { get; private set; }
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else 
            instance = this;
    }
    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<SwordSkill>();
        blackHole = GetComponent<BlackHoleSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<Parry_Skill>();
        dodge = GetComponent<Dodge_Skill>();
    }
}
