using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum MoveType
{
    crouch,
    walk,
    sprint
}

public class PlayerControl : BaseStats
{

    #region variables
    [Header("Physics")]
    public CharacterController controller;
    public float gravity = 20f;
    public GameObject checkPoint;

    [Header("Movement Variables")]
    public float crouchSpeed = 2f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    public float jumpSpeed = 8f;
    Vector3 moveDirection;
    float speed;
    MoveType currentMoveType = MoveType.walk;

    [Header("References")]
    public Customisation customisation;
    public Animator animator;
    public PlayerSaveAndLoad PlayerSaveAndLoad;
    public GameObject damageFlash;
    public GameObject deathScreen;

    [Header("Damage")]
    public static bool isDead;
    public bool canHeal;
    public float healTimer;

    [Header("Menus")]
    public string currentMenu;

    [Header("Audio")]
    public AudioClip damageSound;
    public AudioClip deathSound;

    #endregion

    #region setup keys
    //set default key values
    KeyCode forwardKey = KeyCode.W;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;
    KeyCode backwardKey = KeyCode.S;
    KeyCode jumpKey = KeyCode.Space;
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode crouchKey = KeyCode.LeftControl;
    KeyCode interactKey = KeyCode.E;
    [HideInInspector]
    public KeyCode inventoryKey = KeyCode.Tab;
    KeyCode questLogKey = KeyCode.L;
    [HideInInspector]
    public KeyCode openKey = KeyCode.C;
    #endregion


    void Start()
    {
        customisation = GetComponent<Customisation>();

        speed = walkSpeed;
        controller = this.gameObject.GetComponent<CharacterController>();
        deathScreen.SetActive(false);
        GetKeyBindings();
        PlayerSaveAndLoad.Load();

    }

    #region get key bindings
     public void GetKeyBindings()
     {
        //get keys from dictionary and assign keycodes
        forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
        leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
        backwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
        sprintKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
        crouchKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl"));
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "LeftControl"));
        inventoryKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory", "Tab"));
        questLogKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("QuestLog", "L"));
        openKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Character", "C"));

    }
    #endregion

    
    void Update()
    {
        if (!isDead)
        {
            #region movement
            if (controller.isGrounded)
            {
                #region get movement inputs

                float vMove = 0;
                float hMove = 0;
                if (Input.GetKey(forwardKey))
                {
                    vMove += 1;
                }
                if (Input.GetKey(backwardKey))
                {
                    vMove -= 1;
                }
                if (Input.GetKey(leftKey))
                {
                    hMove -= 1;
                }
                if (Input.GetKey(rightKey))
                {
                    hMove += 1;
                }
                #endregion

                moveDirection = transform.TransformDirection(new Vector3(hMove, 0, vMove));
                moveDirection *= speed;

                //animation
                if (animator.GetBool("Move") == false && (hMove != 0 || vMove != 0))
                {
                    animator.SetBool("Move", true);
                }
                else if (animator.GetBool("Move") == true && (hMove == 0 && vMove ==0))
                {
                    animator.SetBool("Move", false);
                }

                if (Input.GetKey(jumpKey))
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            //cannot sprint if no stamina
            
            if (currentMoveType == MoveType.sprint)
            {
                if (characterStatus[2].currentValue <= 0)
                {
                    characterStatus[2].currentValue = 0;
                    speed = walkSpeed;
                }
                else
                {
                    characterStatus[2].currentValue -= 8 * Time.deltaTime;
                }
            }
            controller.Move(moveDirection * Time.deltaTime);
            #endregion

            #region Crouch/Sprint

            if (Input.GetKeyDown(sprintKey))
            {
                ChangeMoveType(MoveType.sprint);
            }

            if (Input.GetKeyDown(crouchKey))
            {
                ChangeMoveType(MoveType.crouch);
            }

            if (currentMoveType != MoveType.walk)
            {
                if (!(Input.GetKey(sprintKey) || Input.GetKey(crouchKey)))
                {
                    ChangeMoveType(MoveType.walk);
                }
                else if ((currentMoveType == MoveType.crouch) && !(Input.GetKey(crouchKey)))
                {
                    ChangeMoveType(MoveType.sprint);
                }
                else if ((currentMoveType == MoveType.sprint) && !(Input.GetKey(sprintKey)))
                {
                    ChangeMoveType(MoveType.crouch);
                }
            }

            #endregion

            #region interact
            if (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown(1))
            {
                if (currentMenu == "Dialogue")
                {
                    if (GetComponent<DialogueControl>().choice == false)
                    {
                        GetComponent<DialogueControl>().AdvanceDialogue();
                    }
                }
                else if (currentMenu == "Inventory")
                {
                    if (DialogueControl.inShop)
                    {
                        GetComponent<DialogueControl>().CloseShop();
                    }
                    else
                    {
                        GetComponent<Inventory>().CloseInventory();
                    }
                }
                else
                {
                    GetComponent<Interact>().InteractKey();
                }
                
            }
            #endregion

            #region quest log
            if (Input.GetKeyDown(questLogKey) && (currentMenu == null || currentMenu == "QuestLog"))
            {
                if (currentMenu == null)
                {
                    GetComponent<QuestHandler>().OpenQuestLog();
                }
                else
                {
                    GetComponent<QuestHandler>().CloseQuestLog();
                }
            }
            #endregion

            #region fill status bars
            for (int i = 0; i < characterStatus.Length; i++)
            {
                characterStatus[i].displayImage.fillAmount = Mathf.Clamp01(characterStatus[i].currentValue / characterStatus[i].maxValue);
            }

            #endregion

            #region heal timer
            if (!canHeal)
            {
                healTimer += Time.deltaTime;
                if (healTimer >= 5)
                {
                    canHeal = true;
                }
            }
            #endregion

            #region damage key
#if UNITY_EDITOR
            //for testing damage
            if (Input.GetKeyDown(KeyCode.X))
            {
                DamagePlayer(10);
            }
#endif
            #endregion
        }

    }
    #region Crouch/Sprint functions
    void ChangeMoveType(MoveType type)
    {
        if (type == MoveType.walk)
        {
            currentMoveType = MoveType.walk;
            speed = walkSpeed;
        }
        else if (type == MoveType.sprint)
        {
            currentMoveType = MoveType.sprint;
            speed = sprintSpeed;
        }
        else if (type == MoveType.crouch)
        {
            currentMoveType = MoveType.crouch;
            speed = crouchSpeed;
        }
    }
    #endregion

    #region damage, death and healing
    private void LateUpdate()
    {
        if (characterStatus[0].currentValue <= 0 && !isDead)
        {
            Death();
        }
        //regen HP
        if (canHeal && characterStatus[0].currentValue < characterStatus[0].maxValue && characterStatus[0].currentValue > 0)
        {
            characterStatus[0].currentValue += Time.deltaTime * characterStatus[0].regenValue;
        }
        //regen MP
        if (characterStatus[1].currentValue < characterStatus[1].maxValue)
        {
            characterStatus[1].currentValue += Time.deltaTime * characterStatus[1].regenValue;
        }
        //regen stamina
        if (characterStatus[2].currentValue < characterStatus[2].maxValue && currentMoveType != MoveType.sprint)
        {
            characterStatus[2].currentValue += Time.deltaTime * characterStatus[2].regenValue;
        }
    }

    void Death()
    {
        isDead = true;
        deathScreen.SetActive(true);
        HideDamageFlash();
        //allow mouse use
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //play sound
        GetComponent<AudioSource>().PlayOneShot(deathSound);
    }

    public void Respawn()
    {
        //hide mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //set values and move player
        isDead = false;
        for (int i = 0; i < characterStatus.Length; i++)
        {
            characterStatus[i].currentValue = characterStatus[0].maxValue;
        }
        gameObject.SetActive(false);
        transform.position = checkPoint.transform.position;
        moveDirection = new Vector2(0, 0);
        gameObject.SetActive(true);
        deathScreen.SetActive(false);
    }
    
        

    public void QuitFromDeath()
    {
        
        isDead = false;
        characterStatus[0].currentValue = characterStatus[0].maxValue;
        gameObject.SetActive(false);
        transform.position = checkPoint.transform.position;
        gameObject.SetActive(true);
        PlayerSaveAndLoad.Save();
        SceneManager.LoadScene(0);

    }

    public void DamagePlayer(float damage)
    {
        characterStatus[0].currentValue -= damage;

        //Set flash
        damageFlash.SetActive(true);
        Invoke("HideDamageFlash", 0.2f);
        //unable to heal
        canHeal = false;
        healTimer = 0;
        //play sound
        GetComponent<AudioSource>().PlayOneShot(damageSound);
    }

    void HideDamageFlash()
    {
        damageFlash.SetActive(false);
    }

    #endregion

    #region load customisation
    public void SetCustomisation()
    {
        customisation.SetTexture("Skin", customIndex[0]);
        customisation.SetTexture("Hair", customIndex[1]);
        customisation.SetTexture("Eyes", customIndex[2]);
        customisation.SetTexture("Mouth", customIndex[3]);
        customisation.SetTexture("Clothes", customIndex[4]);
        customisation.SetTexture("Armour", customIndex[5]);
    }
    #endregion

    #region enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamagePlayer(10);
        }
    }
    #endregion
}










