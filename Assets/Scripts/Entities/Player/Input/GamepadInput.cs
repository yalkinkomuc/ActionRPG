using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GamePadInput : IPlayerInput
{
        
    public float horizontal => Input.GetAxisRaw("Horizontal");
    public float vertical => Input.GetAxisRaw("Vertical");
    public bool jumpPressed => Input.GetKeyDown(KeyCode.JoystickButton0);
    public bool dashPressed => Input.GetKeyDown(KeyCode.JoystickButton1);
    public bool attackPressed => Input.GetKeyDown(KeyCode.JoystickButton2);

    public bool heavyAttackPressed =>  Input.GetAxis("RT") > .1F;
    public bool counterAttackPressed => Input.GetKeyDown(KeyCode.JoystickButton4);

    public bool blackHolePressed => Input.GetKeyDown(KeyCode.JoystickButton8)&& Input.GetKeyDown(KeyCode.JoystickButton9);
    public bool aimPressed => Input.GetAxis("LT") > .1F;
    public bool menuPressed => Input.GetKeyDown(KeyCode.JoystickButton7);
    public bool crystalSkillPressed => Input.GetKeyDown(KeyCode.JoystickButton3);

    public bool useFlaskPressed => Input.GetAxis("DPadUp") > .1F;
    
}
