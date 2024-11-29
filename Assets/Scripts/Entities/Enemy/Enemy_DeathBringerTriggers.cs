using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_DeathBringerTriggers : Enemy_AnimationTriggers
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void relocate() => enemyDeathBringer.findPosition();

    private void MakeInvisible() => enemyDeathBringer.fX.MakeTransparent(true);
    private void MakeVisible() => enemyDeathBringer.fX.MakeTransparent(false);
}
