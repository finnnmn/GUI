using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void InteractKey()
    {
        if (PauseHandler.paused)
        {
            return;
        }
        Ray interactRay;
        interactRay = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit rayHit;

        if (Physics.Raycast(interactRay, out rayHit, 10))
        {
            #region item
            if (rayHit.collider.CompareTag("Item"))
            {
                ItemHandler itemHandler = rayHit.transform.GetComponent<ItemHandler>();
                if (itemHandler != null)
                {
                    itemHandler.PickUp();
                }
            }
            #endregion

            #region chest
            else if (rayHit.collider.CompareTag("Storage"))
            {
                Storage storage = rayHit.transform.GetComponent<Storage>();
                if (storage != null)
                {
                    storage.Open();
                }
            }
            #endregion

            #region NPC
            else if (rayHit.collider.CompareTag("NPC"))
            {
                NPC npc = rayHit.transform.GetComponent<NPC>();
                if (npc != null)
                {
                    npc.Talk();
                }
            }
            #endregion
        }

    }
}
