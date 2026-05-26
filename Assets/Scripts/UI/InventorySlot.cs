using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField]
    private int slotIndex;
    public int SlotIndex
    { get { return slotIndex; } set { slotIndex = value; } }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PartyManager.instance.SelectChars.Count <= 0)
            return;

        Character hero = PartyManager.instance.SelectChars[0];
        Item item = hero.InventoryItems[slotIndex];

        if (item != null)
            UIManager.instance.ShowItemDialog(item, slotIndex, hero);
    }
}
