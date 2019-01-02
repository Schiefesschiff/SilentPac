using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Canvas keyCanvas;
    private DooropenerPopupController dooropenerPopupController;
    public Canvas fuseCanvas;
    private FuseboxPopupController fuseboxPopupController;

    public bool hasKey;
    public bool hasFuse;

    void Awake()
    {
        dooropenerPopupController = keyCanvas.GetComponent<DooropenerPopupController>();
        fuseboxPopupController = fuseCanvas.GetComponent<FuseboxPopupController>();

        hasKey = false;
        hasFuse = false;
    }

    public void AddKeyToInventory()
    {
        hasKey = true;
        dooropenerPopupController.ChangePopup(dooropenerPopupController.WhichPopup()); //sets the appropriate popup
    }
    public void RemoveKeyFromInventory()
    {
        hasKey = false;
        fuseboxPopupController.ChangePopup(fuseboxPopupController.WhichPopup()); //sets the appropriate popup
    }
    public void AddFuseToInventory()
    {
        hasFuse = true;
        fuseboxPopupController.ChangePopup(fuseboxPopupController.WhichPopup()); //sets the appropriate popup
    }
    public void RemoveFuseFromInventory()
    {
        hasFuse = false;
        fuseboxPopupController.ChangePopup(fuseboxPopupController.WhichPopup()); //sets the appropriate popup
    }


}
