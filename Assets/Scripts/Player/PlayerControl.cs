using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Movement Variables")]
    public float crouchSpeed = 2f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    public float jumpSpeed = 8f;
    Vector3 moveDirection;
    float speed;
    MoveType CurrentMoveType = MoveType.walk;

    [Header("References")]
    public PlayerSaveAndLoad PlayerSaveAndLoad;
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
    #endregion


    void Start()
    {
        speed = walkSpeed;
        controller = this.gameObject.GetComponent<CharacterController>();
        getKeyBindings();
        PlayerSaveAndLoad.Load();

    }

    #region get key bindings
     public void getKeyBindings()
     {
        //get keys from dictionary and assign keycodes
         forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
         leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
         rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
         backwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
         jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
         sprintKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
         crouchKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl"));

    }
    #endregion

    
    void Update()
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
            if (Input.GetKey(jumpKey))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        #endregion

        #region Crouch/Sprint

        if (Input.GetKeyDown(sprintKey))
        {
            changeMoveType(MoveType.sprint);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            changeMoveType(MoveType.crouch);
        }

        if (CurrentMoveType != MoveType.walk)
        {
            if (!(Input.GetKey(sprintKey) || Input.GetKey(crouchKey)))
            {
                changeMoveType(MoveType.walk);
            }
            else if ((CurrentMoveType == MoveType.crouch) && !(Input.GetKey(crouchKey)))
            {
                changeMoveType(MoveType.sprint);
            }
            else if ((CurrentMoveType == MoveType.sprint) && !(Input.GetKey(sprintKey)))
            {
                changeMoveType(MoveType.crouch);
            }
        }

        #endregion

        #region fill status bars
        for (int i = 0; i < characterStatus.Length; i++)
        {
            characterStatus[i].displayImage.fillAmount = Mathf.Clamp01(characterStatus[i].currentValue/characterStatus[i].maxValue);
        }

        #endregion

    }

    void changeMoveType(MoveType type)
    {
        if (type == MoveType.walk)
        {
            CurrentMoveType = MoveType.walk;
            speed = walkSpeed;
        }
        else if (type == MoveType.sprint)
        {
            CurrentMoveType = MoveType.sprint;
            speed = sprintSpeed;
        }
        else if (type == MoveType.crouch)
        {
            CurrentMoveType = MoveType.crouch;
            speed = crouchSpeed;
        }
    }
}










