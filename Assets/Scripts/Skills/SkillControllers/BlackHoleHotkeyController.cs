using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myTextMesh;

    private Transform myEnemy;
    private BlackHole_SkillController blackHole;

    public void SetupHotkey(KeyCode _myNewHotkey,Transform _myEnemy, BlackHole_SkillController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myTextMesh = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotkey = _myNewHotkey;
        myTextMesh.text = _myNewHotkey.ToString();
    }
    private void Update()
    {
       if(Input.GetKeyDown(myHotkey))
        {
            blackHole.AddEnemyToList(myEnemy);
            myTextMesh.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
