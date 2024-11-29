using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
    float horizontal { get; }
    float vertical { get; }
    bool jumpPressed { get; }
    bool dashPressed { get; }
    bool attackPressed { get; }
    bool heavyAttackPressed { get; }
    bool counterAttackPressed {get;}
    bool blackHolePressed { get; }
    bool menuPressed { get; }
    bool crystalSkillPressed { get; }
    bool useFlaskPressed { get; }
}