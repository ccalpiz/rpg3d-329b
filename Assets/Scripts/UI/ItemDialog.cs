using UnityEngine;
using UnityEngine.UI;

public class ItemDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject grayOverlay;

    [SerializeField]
    private GameObject dialogPanel;

    [SerializeField]
    private Text itemNameText;

    private Item currentItem;
    private int currentSlotIndex;
    private Character currentHero;

    public void Show(Item item, int slotIndex, Character hero)
    {
        currentItem = item;
        currentSlotIndex = slotIndex;
        currentHero = hero;

        itemNameText.text = item.ItemName;
        grayOverlay.SetActive(true);
        dialogPanel.SetActive(true);
    }

    public void OnUseButton()
    {
        if (currentItem == null || currentHero == null)
            return;

        if (currentItem.Type == ItemType.Consumable)
            currentHero.HealHP(currentItem.Power);

        InventoryManager.instance.RemoveItem(currentHero, currentSlotIndex);
        UIManager.instance.ClearInventory();
        UIManager.instance.ShowInventory();

        Hide();
    }

    public void OnDoneButton()
    {
        Hide();
    }

    private void Hide()
    {
        currentItem = null;
        currentHero = null;
        grayOverlay.SetActive(false);
        dialogPanel.SetActive(false);
    }
}
