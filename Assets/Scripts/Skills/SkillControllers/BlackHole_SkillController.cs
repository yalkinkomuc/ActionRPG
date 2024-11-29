using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_SkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keys;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;
    public bool canGrow = true;
    public bool canShrink;
    private bool playerCanDissappear=true;


    private bool CloneAttackReleased;    
    private bool canCreateHotKeys = true;
    private int amountOfAttacks = 4 ;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    public bool playerCanExitState {  get; private set; }   

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();


    public void SetupBlackHole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackholeDuration;

        if(SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDissappear = false;

    }
    

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if(blackHoleTimer < 0 )
        {

            blackHoleTimer = Mathf.Infinity;

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }



        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();

        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }

    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        DestroyHotKeys();
        CloneAttackReleased = true;
        canCreateHotKeys = false;

        if(playerCanDissappear)
        {
            playerCanDissappear = false;
            PlayerManager.instance.player.playerFX.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && CloneAttackReleased&&amountOfAttacks>0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 1;

            else
                xOffset = -1;


            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }


            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", .8f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        
        canShrink = true;
        CloneAttackReleased = false;

    }

    private void DestroyHotKeys()
    {
        if (createdHotkey.Count <= 0)
            return;

        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)

                collision.GetComponent <Enemy>().FreezeTime(false);
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (keys.Count <= 0)
        {
            Debug.LogWarning("Not Enough Hotkeys in a key code list!");
            return;
        }

        if (!canCreateHotKeys)
            return;


        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotKey);

        KeyCode choosenKey = keys[Random.Range(0, keys.Count)];
        keys.Remove(choosenKey);

        BlackHoleHotkeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();

        newHotKeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
