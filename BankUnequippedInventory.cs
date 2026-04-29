/*
name: Bank Unequipped Inventory
description: Banks all inventory items that are not equipped or being worn as vanity.
tags: bank, inventory, cleanup
*/
//cs_include Scripts/CoreBots.cs
using System.Linq;
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;

public class BankUnequippedInventory
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;

    public bool DontPreconfigure = true;

    public void ScriptMain(IScriptInterface bot)
    {
        BankUnequippedItems();
    }

    public void BankUnequippedItems()
    {
        InventoryItem[] inventoryItems = Bot.Inventory.Items.Where(item => item != null).ToArray();
        InventoryItem[] itemsToBank = inventoryItems
            .Where(item =>
                !item.Temp
                && !item.Equipped
                && !item.Wearing
            )
            .ToArray();

        if (itemsToBank.Length == 0)
        {
            foreach (InventoryItem item in inventoryItems)
            {
                if (!item.Temp && !item.Equipped && !item.Wearing)
                    continue;

                Core.Logger(
                    $"Skipped {item.Name}: Temp={item.Temp}, Equipped={item.Equipped}, Wearing={item.Wearing}, Category={item.CategoryString}"
                );
            }
            Core.Logger("Nothing to bank.");
            return;
        }

        int moved = 0;
        bool bankFullLogged = false;

        foreach (InventoryItem item in itemsToBank)
        {
            if (Bot.Bank.FreeSlots <= 0 && !item.Coins)
            {
                if (!bankFullLogged)
                {
                    Core.Logger("Bank is full.");
                    bankFullLogged = true;
                }
                break;
            }

            bool banked = false;
            for (int attempt = 0; attempt < 5 && !banked; attempt++)
            {
                banked =
                    Bot.Inventory.EnsureToBank(item.ID)
                    || Bot.Inventory.EnsureToBank(item.Name)
                    || Bot.Bank.Contains(item.ID)
                    || Bot.Bank.Contains(item.Name);
                if (!banked)
                    Core.Sleep();
            }

            if (banked)
            {
                moved++;
                Core.Logger($"Banked: {item.Name}");
            }
            else
                Core.Logger($"Failed to bank: {item.Name}");
        }

        Core.Logger($"Banked {moved} item{(moved == 1 ? "" : "s")}.");
    }
}
