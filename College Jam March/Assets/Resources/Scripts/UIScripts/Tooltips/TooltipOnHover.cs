using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum TooltipType
{
    Tooltip,
    LeftHandWeapon,
    RightHandWeapon,
    InventoryTooltip //Unused
}

public class TooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Display a tooltip prefab while hovering over the element

    public GameObject tooltipPrefab = null;
    public TooltipType type;
    public int yOffset;
    public bool followMouse = true;
    public bool toParent = true;
    private GameObject tooltip = null;
    private Tooltip tooltipClass = null;
    private bool pointerOver = false;

    [HideInInspector] public Inventory.InventoryItem passItem;

    private void SetVars()
    {
        switch (type)
        {
            case TooltipType.LeftHandWeapon:
                WeaponTooltip weaponTooltipLeft = (WeaponTooltip)tooltipClass;
                weaponTooltipLeft.referenceWeapon = (Inventory.WeaponItem)Player.PlayerRefs.instance.playerClass.playerAssignment.leftHandItem;
                break;
            case TooltipType.RightHandWeapon:
                WeaponTooltip weaponTooltipRight = (WeaponTooltip)tooltipClass;
                weaponTooltipRight.referenceWeapon = (Inventory.WeaponItem)Player.PlayerRefs.instance.playerClass.playerAssignment.rightHandItem;
                break;
            // case TooltipType.InventoryTooltip:
            //     InventoryTooltip inventoryTooltip = (InventoryTooltip)tooltipClass;
            //     inventoryTooltip.referenceItem = passItem;
            //     break;
        }
    }

    public void OnPointerEnter(PointerEventData pointerData)
    {
        pointerOver = true;
        if (tooltipPrefab != null && tooltip == null)
        {
            Vector3 newPos = Mouse.current.position.ReadValue();
            newPos.y += yOffset;
            if (toParent) tooltip = Instantiate(tooltipPrefab, transform.parent);
            else tooltip = Instantiate(tooltipPrefab, transform);
            tooltip.transform.position = newPos;
            tooltipClass = tooltip.GetComponent<Tooltip>();
            SetVars();
            tooltipClass.UpdateTooltip();
        }
    }

    public void OnPointerExit(PointerEventData pointerData)
    {
        pointerOver = false;
    }

    private void FixedUpdate() 
    {
        if (tooltip != null && followMouse)
        {
            Vector3 newPos = Mouse.current.position.ReadValue();
            newPos.y += yOffset;
            tooltip.transform.position = newPos;
        }

        if (tooltip != null && !pointerOver&& !tooltipClass.pointerOver)
        {
            Destroy(tooltip);
        }
    }
}