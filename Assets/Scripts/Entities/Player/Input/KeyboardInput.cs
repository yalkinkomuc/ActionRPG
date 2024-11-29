using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyboardInput : IPlayerInput
{
    public float horizontal => Input.GetAxisRaw("Horizontal");
    public float vertical => Input.GetAxisRaw("Vertical");
    public bool jumpPressed => Input.GetKeyDown(KeyCode.Space);
    public bool dashPressed => Input.GetKeyDown(KeyCode.LeftShift);
    public bool attackPressed => Input.GetKeyDown(KeyCode.Mouse0);

    public bool heavyAttackPressed => Input.GetKeyDown(KeyCode.Mouse1);
    public bool counterAttackPressed => Input.GetKeyDown(KeyCode.Q);
    public bool blackHolePressed => Input.GetKeyDown(KeyCode.R);
    public bool aimPressed => Input.GetKeyDown(KeyCode.Mouse1);
    public bool menuPressed => Input.GetKeyDown(KeyCode.Escape);
    public bool crystalSkillPressed => Input.GetKeyDown(KeyCode.F);
    public bool useFlaskPressed => Input.GetKeyDown(KeyCode.Alpha1);

}
