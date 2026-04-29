/*
name: Skua Outfit Slots
description: Equip wear and show items for weapon, helm, armor, and cape
tags: outfit, equip, vanity, wear, show
*/
#region includes
//cs_include Scripts/CoreBots.cs
#endregion includes

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Options;

public class SkuaOutfitSlots
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;

    public string OptionsStorage = "SkuaOutfitSlots";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        new Option<string>("WeaponEquip", "Weapon - Equip", "Combat slot. Blank = unequip weapon.", string.Empty),
        new Option<string>("HelmEquip", "Helm - Equip", "Combat slot. Blank = unequip helm.", string.Empty),
        new Option<string>("ArmorEquip", "Armor - Equip", "Combat slot. Blank = unequip armor.", string.Empty),
        new Option<string>("CapeEquip", "Cape - Equip", "Combat slot. Blank = unequip cape.", string.Empty),
        new Option<string>("PetEquip", "Pet - Equip", "Combat slot. Blank = unequip pet.", string.Empty),
        new Option<string>("WeaponShow", "Weapon - Show", "Vanity slot. Blank = hide vanity weapon.", string.Empty),
        new Option<string>("HelmShow", "Helm - Show", "Vanity slot. Blank = hide vanity helm.", string.Empty),
        new Option<string>("ArmorShow", "Armor - Show", "Vanity slot. Blank = hide vanity armor.", string.Empty),
        new Option<string>("CapeShow", "Cape - Show", "Vanity slot. Blank = hide vanity cape.", string.Empty),
        new Option<string>("PetShow", "Pet - Show", "Vanity slot. Blank = hide vanity pet.", string.Empty),
        CoreBots.Instance.SkipOptions,
    };

    public void ScriptMain(IScriptInterface bot)
    {
        ApplyConfiguredOutfit();
    }

    public void ApplyConfiguredOutfit()
    {
        string weaponEquip = ReadOption("WeaponEquip");
        string helmEquip = ReadOption("HelmEquip");
        string armorEquip = ReadOption("ArmorEquip");
        string capeEquip = ReadOption("CapeEquip");
        string petEquip = ReadOption("PetEquip");

        string weaponShow = ReadOption("WeaponShow");
        string helmShow = ReadOption("HelmShow");
        string armorShow = ReadOption("ArmorShow");
        string capeShow = ReadOption("CapeShow");
        string petShow = ReadOption("PetShow");

        ApplyEquipOrUnequip(OutfitSlot.Weapon, weaponEquip);
        ApplyEquipOrUnequip(OutfitSlot.Helm, helmEquip);
        ApplyEquipOrUnequip(OutfitSlot.Armor, armorEquip);
        ApplyEquipOrUnequip(OutfitSlot.Cape, capeEquip);
        ApplyEquipOrUnequip(OutfitSlot.Pet, petEquip);

        ApplyShowOrClear(OutfitSlot.Weapon, weaponShow, weaponEquip);
        ApplyShowOrClear(OutfitSlot.Helm, helmShow, helmEquip);
        ApplyShowOrClear(OutfitSlot.Armor, armorShow, armorEquip);
        ApplyShowOrClear(OutfitSlot.Cape, capeShow, capeEquip);
        ApplyShowOrClear(OutfitSlot.Pet, petShow, petEquip);

        Core.Logger("Configured wear/show outfit applied.");
    }

    private string ReadOption(string key)
    {
        string value = Bot.Config.Get<string>(key) ?? string.Empty;
        return value.Trim();
    }

    private void ApplyEquipOrUnequip(OutfitSlot slot, string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            UnequipSlot(slot);
            return;
        }

        InventoryItem item = EnsureItemInInventory(itemName);
        if (item == null)
        {
            Core.Logger($"Equip skipped: \"{itemName}\" not found in inventory or bank.");
            return;
        }

        if (IsCorrectItemAlreadyEquipped(slot, item.Name))
            return;

        Core.Equip(item.Name);

        if (Bot.Inventory.IsEquipped(item.ID))
            CleanupSlotEquipState(slot, new HashSet<int> { item.ID });
    }

    private void ApplyShowOrClear(OutfitSlot slot, string itemName, string equipItemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            ClearShowSlot(slot, equipItemName);
            return;
        }

        InventoryItem item = EnsureItemInInventory(itemName);
        if (item == null)
        {
            Core.Logger($"Show skipped: \"{itemName}\" could not be loaded into inventory.");
            return;
        }

        if (IsCorrectItemAlreadyWorn(slot, item.Name))
            return;

        HashSet<int> keepIds = new();
        InventoryItem equipItem = EnsureItemInInventory(equipItemName);
        if (equipItem != null && Bot.Inventory.IsEquipped(equipItem.ID))
            keepIds.Add(equipItem.ID);

        if (item.CategoryString.Equals("item", StringComparison.OrdinalIgnoreCase))
        {
            if (!Bot.Inventory.IsEquipped(item.ID))
                ToggleItemEquip(item, true);

            if (Bot.Inventory.IsEquipped(item.ID))
                keepIds.Add(item.ID);

            CleanupSlotEquipState(slot, keepIds);
        }
        else
        {
            SendWearPacket(item);
            ApplyShowVisual(slot, item);
        }

        Core.Logger($"Applied vanity {slot}: {item.Name}");
    }

    private void UnequipSlot(OutfitSlot slot)
    {
        List<InventoryItem> equippedItems = GetEquippedSlotItems(slot);
        if (equippedItems.Count == 0)
            return;

        foreach (InventoryItem equippedItem in equippedItems)
        {
            Core.SendPackets($"%xt%zm%unequipItem%{Bot.Map.RoomID}%{equippedItem.ID}%");
            Bot.Wait.ForTrue(() => !Bot.Inventory.IsEquipped(equippedItem.ID), 10);
            Core.Logger($"Unequipped {slot}: {equippedItem.Name}");
        }
    }

    private List<InventoryItem> GetEquippedSlotItems(OutfitSlot slot)
    {
        List<InventoryItem> equippedItems = new();
        foreach (InventoryItem item in Bot.Inventory.Items)
        {
            if (item == null || !item.Equipped)
                continue;

            if (MatchesSlot(item, slot))
                equippedItems.Add(item);
        }

        return equippedItems;
    }

    private List<InventoryItem> GetWornSlotItems(OutfitSlot slot)
    {
        List<InventoryItem> wornItems = new();
        foreach (InventoryItem item in Bot.Inventory.Items)
        {
            if (item == null || !item.Wearing)
                continue;

            if (MatchesSlot(item, slot))
                wornItems.Add(item);
        }

        return wornItems;
    }

    private void CleanupSlotEquipState(OutfitSlot slot, HashSet<int> keepItemIds)
    {
        foreach (InventoryItem equippedItem in GetEquippedSlotItems(slot))
        {
            if (keepItemIds.Contains(equippedItem.ID))
                continue;

            ToggleItemEquip(equippedItem, false);
            Core.Logger($"Removed extra equipped {slot}: {equippedItem.Name}");
        }
    }

    private bool IsCorrectItemAlreadyEquipped(OutfitSlot slot, string itemName)
    {
        return GetEquippedSlotItems(slot).Any(item =>
            item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)
        );
    }

    private bool IsCorrectItemAlreadyWorn(OutfitSlot slot, string itemName)
    {
        return GetWornSlotItems(slot).Any(item =>
            item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)
        );
    }

    private bool HasVanityOverride(OutfitSlot slot)
    {
        return GetWornSlotItems(slot).Count > 0;
    }

    private bool MatchesSlot(InventoryItem item, OutfitSlot slot)
    {
        switch (slot)
        {
            case OutfitSlot.Weapon:
                return item.ItemGroup == "Weapon"
                    || item.Category == ItemCategory.Sword
                    || item.Category == ItemCategory.Axe
                    || item.Category == ItemCategory.Bow
                    || item.Category == ItemCategory.HandGun
                    || item.Category == ItemCategory.Gauntlet
                    || item.Category == ItemCategory.Dagger
                    || item.Category == ItemCategory.Rifle
                    || item.Category == ItemCategory.Whip
                    || item.Category == ItemCategory.Mace
                    || item.Category == ItemCategory.Polearm
                    || item.Category == ItemCategory.Staff
                    || item.Category == ItemCategory.Wand;
            case OutfitSlot.Helm:
                return item.Category == ItemCategory.Helm;
            case OutfitSlot.Armor:
                return item.Category == ItemCategory.Armor;
            case OutfitSlot.Cape:
                return item.Category == ItemCategory.Cape;
            case OutfitSlot.Pet:
                return item.Category == ItemCategory.Pet;
            default:
                return false;
        }
    }

    private InventoryItem EnsureItemInInventory(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return null;

        string resolvedItemName = ResolveOwnedItemName(itemName);
        if (string.IsNullOrWhiteSpace(resolvedItemName))
            return null;

        if (Bot.Inventory.TryGetItem(resolvedItemName, out InventoryItem inventoryItem))
            return inventoryItem;

        InventoryItem bankItem = GetBankItemByName(resolvedItemName);
        if (bankItem == null)
            return null;

        for (int attempt = 0; attempt < 5; attempt++)
        {
            TryMoveItemFromBankToInventory(bankItem);
            Core.Sleep(500);

            if (Bot.Inventory.TryGetItem(resolvedItemName, out inventoryItem))
                return inventoryItem;
        }

        if (!Bot.Inventory.TryGetItem(resolvedItemName, out inventoryItem))
            return null;

        return inventoryItem;
    }

    private string ResolveOwnedItemName(string itemName)
    {
        string trimmedName = itemName.Trim();

        InventoryItem inventoryItem = Bot.Inventory.Items.FirstOrDefault(item =>
            item != null && item.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)
        );
        if (inventoryItem != null)
            return inventoryItem.Name;

        InventoryItem bankItem = GetBankItemByName(trimmedName);
        if (bankItem != null)
            return bankItem.Name;

        if (Core.CheckInventory(trimmedName, toInv: false))
            return trimmedName;

        return string.Empty;
    }

    private InventoryItem GetBankItemByName(string itemName)
    {
        return Bot.Bank.Items.FirstOrDefault(item =>
            item != null && item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)
        );
    }

    private void TryMoveItemFromBankToInventory(InventoryItem bankItem)
    {
        try
        {
            Core.Unbank(bankItem.Name);
        }
        catch
        {
        }

        if (Bot.Inventory.Contains(bankItem.ID) || Bot.Inventory.Contains(bankItem.Name))
            return;

        try
        {
            Bot.Bank.ToInventory(bankItem.ID);
        }
        catch
        {
            try
            {
                Bot.Bank.ToInventory(bankItem.Name);
            }
            catch
            {
            }
        }
    }

    private void ToggleItemEquip(InventoryItem item, bool shouldEquip)
    {
        bool isEquipped = Bot.Inventory.IsEquipped(item.ID);
        if (isEquipped == shouldEquip)
            return;

        dynamic display = new ExpandoObject();
        display.ItemID = item.ID;
        display.sLink = Bot.Flash.GetGameObject<string>($"world.invTree.{item.ID}.sLink");
        display.sES = item.ItemGroup;
        display.sType = item.CategoryString;
        display.sIcon = Bot.Flash.GetGameObject<string>($"world.invTree.{item.ID}.sIcon");
        display.sFile = Bot.Flash.GetGameObject<string>($"world.invTree.{item.ID}.sFile");
        display.bUpg = item.Upgrade ? 1 : 0;
        display.sDesc = item.Description;
        display.bEquip = isEquipped ? 1 : 0;
        display.sName = item.Name;
        display.sMeta = item.Meta;

        Bot.Flash.CallGameFunction("toggleItemEquip", display);
        Bot.Wait.ForTrue(() => Bot.Inventory.IsEquipped(item.ID) == shouldEquip, 10);
        Core.Sleep(500);
    }

    private void ClearShowSlot(OutfitSlot slot, string equipItemName)
    {
        if (!HasVanityOverride(slot))
            return;

        SendUnwearPacket(slot);

        InventoryItem equipItem = EnsureItemInInventory(equipItemName);

        if (equipItem != null)
            ApplyShowVisual(slot, equipItem);
        else
            ClearShowVisual(slot);

        Core.Logger($"Cleared vanity {slot}.");
    }

    private void ApplyShowVisual(OutfitSlot slot, InventoryItem item)
    {
        string itemGroup = ResolveSlotMovieGroup(slot);
        if (string.IsNullOrWhiteSpace(itemGroup))
            return;

        dynamic display = new ExpandoObject();
        display.sFile = Bot.Flash.GetGameObject<string>($"world.invTree.{item.ID}.sFile");
        display.sLink = Bot.Flash.GetGameObject<string>($"world.invTree.{item.ID}.sLink");
        display.sType = item.CategoryString;

        Bot.Flash.SetGameObject($"world.myAvatar.objData.eqp[{itemGroup}]", display);
        Bot.Flash.CallGameFunction("world.myAvatar.loadMovieAtES", itemGroup, display.sFile, display.sLink);
        Bot.Wait.ForTrue(() => Bot.Player.Loaded, 10);
        Core.Sleep(250);
    }

    private void ClearShowVisual(OutfitSlot slot)
    {
        string itemGroup = ResolveSlotMovieGroup(slot);
        if (string.IsNullOrWhiteSpace(itemGroup))
            return;

        dynamic display = new ExpandoObject();
        display.sFile = null;
        display.sLink = null;
        display.sType = null;

        Bot.Flash.SetGameObject($"world.myAvatar.objData.eqp[{itemGroup}]", display);
        Bot.Flash.CallGameFunction("world.myAvatar.loadMovieAtES", itemGroup, null, null);
        Bot.Wait.ForTrue(() => Bot.Player.Loaded, 10);
        Core.Sleep(250);
    }

    private void SendUnwearPacket(OutfitSlot slot)
    {
        string slotCode = ResolveVanitySlotCode(slot);
        if (string.IsNullOrWhiteSpace(slotCode))
            return;

        Core.SendPackets($"%xt%zm%unwearItem%{Bot.Map.RoomID}%{slotCode}%");
        Core.Sleep(500);
    }

    private void SendWearPacket(InventoryItem item)
    {
        Core.SendPackets($"%xt%zm%wearItem%{Bot.Map.RoomID}%{item.ID}%");
        Core.Sleep(500);
    }

    private string ResolveSlotMovieGroup(OutfitSlot slot)
    {
        switch (slot)
        {
            case OutfitSlot.Weapon:
                return "Weapon";
            case OutfitSlot.Helm:
                return "he";
            case OutfitSlot.Armor:
                return "co";
            case OutfitSlot.Cape:
                return "ba";
            case OutfitSlot.Pet:
                return "pe";
            default:
                return string.Empty;
        }
    }

    private string ResolveVanitySlotCode(OutfitSlot slot)
    {
        switch (slot)
        {
            case OutfitSlot.Weapon:
                return "Weapon";
            case OutfitSlot.Helm:
                return "he";
            case OutfitSlot.Armor:
                return "co";
            case OutfitSlot.Cape:
                return "ba";
            case OutfitSlot.Pet:
                return "pe";
            default:
                return string.Empty;
        }
    }

    private enum OutfitSlot
    {
        Weapon,
        Helm,
        Armor,
        Cape,
        Pet,
    }
}
