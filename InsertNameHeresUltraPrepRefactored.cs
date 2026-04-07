/*
name: TheosUltraPrep
description: Refactored ultra prep helper for InsertNameHere team setups
tags: ultra, prep, insertnamehere, refactor, gear
*/

#region Includes
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Other/Various/Potions.cs
//cs_include Scripts/Farm/BuyScrolls.cs
//cs_include Scripts/Enhancement/UnlockForgeEnhancements.cs
//cs_include Scripts/Legion/CoreLegion.cs
//cs_include Scripts/Story/Legion/DageChallengeStory.cs
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Skua.Core.Interfaces;
using Skua.Core.Options;

public class InsertNameHeresUltraPrepRefactored
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;

    private static CoreAdvanced Adv
    {
        get => _adv ??= new CoreAdvanced();
        set => _adv = value;
    }
    private static CoreAdvanced _adv;

    private static CoreFarms Farm
    {
        get => _farm ??= new CoreFarms();
        set => _farm = value;
    }
    private static CoreFarms _farm;

    private static PotionBuyer PotionBuyer
    {
        get => _potionBuyer ??= new PotionBuyer();
        set => _potionBuyer = value;
    }
    private static PotionBuyer _potionBuyer;

    private static BuyScrolls ScrollsBuyer
    {
        get => _scrollsBuyer ??= new BuyScrolls();
        set => _scrollsBuyer = value;
    }
    private static BuyScrolls _scrollsBuyer;

    private static UnlockForgeEnhancements Forge
    {
        get => _forge ??= new UnlockForgeEnhancements();
        set => _forge = value;
    }
    private static UnlockForgeEnhancements _forge;

    private static CoreLegion Legion
    {
        get => _legion ??= new CoreLegion();
        set => _legion = value;
    }
    private static CoreLegion _legion;

    private static DageChallengeStory DageStory
    {
        get => _dageStory ??= new DageChallengeStory();
        set => _dageStory = value;
    }
    private static DageChallengeStory _dageStory;

    public string OptionsStorage = "UltraPrepRefactored";
    public bool DontPreconfigure = true;
    public List<IOption> Options;

    private readonly string[] _ultraDropsToBank =
    {
        "Ezrajal Insignia",
        "Warden Insignia",
        "Engineer Insignia",
        "Nulgath Insignia",
        "Kala Insignia",
        "Avatar Tyndarius Insignia",
        "Iara Insignia",
        "Champion Drakath Insignia",
        "Malgor Insignia",
        "King Drago Insignia",
        "Volcanic Essence",
        "Darkon Insignia",
        "Dage the Evil Insignia"
    };

    private readonly string[] _alwaysKeepOutOfBank =
    {
        "The First Speaker Silenced",
        "Scroll of Life Steal",
        "Scroll of Enrage",
        "Potent Malevolence Elixir",
        "Potent Battle Elixir",
        "Might Tonic"
    };

    private readonly Dictionary<UltraRole, string[]> _roleItems = new Dictionary<UltraRole, string[]>
    {
        {
            UltraRole.Player1,
            new[]
            {
                "Legion Revenant",
                "ArchPaladin",
                "StoneCrusher",
                "Chaos Avenger|Classic Legion DoomKnight|Legion DoomKnight"
            }
        },
        {
            UltraRole.Player2,
            new[]
            {
                "Quantum Chronomancer",
                "Legion Revenant",
                "Chaos Avenger|Verus DoomKnight",
                "StoneCrusher"
            }
        },
        {
            UltraRole.Player3,
            new[]
            {
                "Lord Of Order",
                "Legion Revenant"
            }
        },
        {
            UltraRole.Player4,
            new[]
            {
                "ArchPaladin",
                "LightCaster",
                "Verus DoomKnight"
            }
        }
    };

    private readonly Dictionary<string, Action> _unlockActions;
    private readonly EnhancementUnlock[] _unlockChecks;
    private readonly EquipmentDefinition[] _equipmentDefinitions;

    public InsertNameHeresUltraPrepRefactored()
    {
        _unlockActions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            ["Awe"] = Farm.UnlockBoA,
            ["ForgeWeapon"] = Forge.ForgeWeaponEnhancement,
            ["Lacerate"] = Forge.Lacerate,
            ["Smite"] = Forge.Smite,
            ["Valiance"] = Forge.HerosValiance,
            ["ArcanasConcerto"] = Forge.ArcanasConcerto,
            ["Absolution"] = Forge.Absolution,
            ["Vainglory"] = Forge.Vainglory,
            ["Avarice"] = Forge.Avarice,
            ["ForgeCape"] = Forge.ForgeCapeEnhancement,
            ["Elysium"] = Forge.Elysium,
            ["Acheron"] = Forge.Acheron,
            ["Penitence"] = Forge.Penitence,
            ["Lament"] = Forge.Lament,
            ["Vim"] = Forge.Vim,
            ["Examen"] = Forge.Examen,
            ["ForgeHelm"] = Forge.ForgeHelmEnhancement,
            ["Pneuma"] = Forge.Pneuma,
            ["Anima"] = Forge.Anima,
            ["Dauntless"] = Forge.DauntLess,
            ["Praxis"] = Forge.Praxis,
            ["Ravenous"] = Forge.Ravenous
        };

        _unlockChecks = new[]
        {
            new EnhancementUnlock("Awe", "Health Vamp / Awe Blast", 2937),
            new EnhancementUnlock("ForgeWeapon", "Forge Weapon", 8738),
            new EnhancementUnlock("Lacerate", "Lacerate", 8739),
            new EnhancementUnlock("Smite", "Smite", 8740),
            new EnhancementUnlock("Valiance", "Valiance", 8741),
            new EnhancementUnlock("ArcanasConcerto", "Arcanas Concerto", 8742),
            new EnhancementUnlock("Absolution", "Absolution", 8743),
            new EnhancementUnlock("Vainglory", "Vainglory", 8744),
            new EnhancementUnlock("Avarice", "Avarice", 8745),
            new EnhancementUnlock("ForgeCape", "Forge Cape", 8758),
            new EnhancementUnlock("Elysium", "Elysium", 8821),
            new EnhancementUnlock("Acheron", "Acheron", 8820),
            new EnhancementUnlock("Penitence", "Penitence", 8822),
            new EnhancementUnlock("Lament", "Lament", 8823),
            new EnhancementUnlock("Vim", "Vim", 8824),
            new EnhancementUnlock("Examen", "Examen", 8825),
            new EnhancementUnlock("ForgeHelm", "Forge Helm", 8828),
            new EnhancementUnlock("Pneuma", "Pneuma", 8827),
            new EnhancementUnlock("Anima", "Anima", 8826),
            new EnhancementUnlock("Dauntless", "Dauntless", 9172),
            new EnhancementUnlock("Praxis", "Praxis", 9171),
            new EnhancementUnlock("Ravenous", "Ravenous", 9560)
        };

        _equipmentDefinitions = new[]
        {
            new EquipmentDefinition("Dauntless", "Dauntless Weapon", "Weapon to apply Dauntless to.", EnhancementType.Lucky, unlockKey: "Dauntless", weaponSpecial: WeaponSpecial.Dauntless),
            new EquipmentDefinition("Valiance", "Valiance Weapon", "Weapon to apply Valiance to.", EnhancementType.Lucky, unlockKey: "Valiance", weaponSpecial: WeaponSpecial.Valiance),
            new EquipmentDefinition("Arcanas_Concerto", "Arcanas Concerto Weapon", "Weapon to apply Arcanas Concerto to.", EnhancementType.Lucky, unlockKey: "ArcanasConcerto", weaponSpecial: WeaponSpecial.Arcanas_Concerto),
            new EquipmentDefinition("Awe_Blast", "Awe Blast Weapon", "Weapon to apply Awe Blast to.", EnhancementType.Lucky, unlockKey: "Awe", weaponSpecial: WeaponSpecial.Awe_Blast),
            new EquipmentDefinition("Praxis", "Praxis Weapon", "Weapon to apply Praxis to.", EnhancementType.Lucky, unlockKey: "Praxis", weaponSpecial: WeaponSpecial.Praxis),
            new EquipmentDefinition("Ravenous", "Ravenous Weapon", "Weapon to apply Ravenous to.", EnhancementType.Lucky, unlockKey: "Ravenous", weaponSpecial: WeaponSpecial.Ravenous),
            new EquipmentDefinition("Elysium", "Elysium Weapon", "Weapon to apply Elysium to.", EnhancementType.Lucky, unlockKey: "Elysium", weaponSpecial: WeaponSpecial.Elysium),
            new EquipmentDefinition("Lacerate", "Lacerate Weapon", "Weapon to apply Lacerate to.", EnhancementType.Lucky, unlockKey: "Lacerate", weaponSpecial: WeaponSpecial.Lacerate),
            new EquipmentDefinition("HealthVamp", "Health Vamp Weapon", "Weapon to apply Health Vamp to.", EnhancementType.Lucky, unlockKey: "Awe", weaponSpecial: WeaponSpecial.Health_Vamp),

            new EquipmentDefinition("Wizard", "Wizard Helm", "Helm to apply Wizard to.", EnhancementType.Wizard),
            new EquipmentDefinition("Luck", "Lucky Helm", "Helm to apply Lucky to.", EnhancementType.Lucky),
            new EquipmentDefinition("Forge", "Forge Helm", "Helm to apply Forge to.", EnhancementType.Lucky, unlockKey: "ForgeHelm", helmSpecial: HelmSpecial.Forge),
            new EquipmentDefinition("Pneuma", "Pneuma Helm", "Helm to apply Pneuma to.", EnhancementType.Wizard, unlockKey: "Pneuma", helmSpecial: HelmSpecial.Pneuma),
            new EquipmentDefinition("Examen", "Examen Helm", "Helm to apply Examen to.", EnhancementType.Lucky, unlockKey: "Examen", helmSpecial: HelmSpecial.Examen),
            new EquipmentDefinition("Vim", "Vim Helm", "Helm to apply Vim to.", EnhancementType.Lucky, unlockKey: "Vim", helmSpecial: HelmSpecial.Vim),
            new EquipmentDefinition("Anima", "Anima Helm", "Helm to apply Anima to.", EnhancementType.Lucky, unlockKey: "Anima", helmSpecial: HelmSpecial.Anima),
            new EquipmentDefinition("Healer", "Healer Helm", "Helm to apply Healer to.", EnhancementType.Healer),


            new EquipmentDefinition("Absolution", "Absolution Cape", "Cape to apply Absolution to.", EnhancementType.Lucky, unlockKey: "Absolution", capeSpecial: CapeSpecial.Absolution),
            new EquipmentDefinition("Avarice", "Avarice Cape", "Cape to apply Avarice to.", EnhancementType.Lucky, unlockKey: "Avarice", capeSpecial: CapeSpecial.Avarice),
            new EquipmentDefinition("Penitence", "Penitence Cape", "Cape to apply Penitence to.", EnhancementType.Lucky, unlockKey: "Penitence", capeSpecial: CapeSpecial.Penitence),
            new EquipmentDefinition("Vainglory", "Vainglory Cape", "Cape to apply Vainglory to.", EnhancementType.Lucky, unlockKey: "Vainglory", capeSpecial: CapeSpecial.Vainglory),
            new EquipmentDefinition("Lament", "Lament Cape", "Cape to apply Lament to.", EnhancementType.Lucky, unlockKey: "Lament", capeSpecial: CapeSpecial.Lament)
        };

        Options = new List<IOption>
        {
            CoreBots.Instance.SkipOptions,
            new Option<UltraRole>("CurrentRole", "Current Role", "Auto detects from player names unless you select a role manually.", UltraRole.Auto),
            new Option<string>("Player1", "Player 1 name", "Username for Player 1 auto-detection.", "Player1"),
            new Option<string>("Player2", "Player 2 name", "Username for Player 2 auto-detection.", "Player2"),
            new Option<string>("Player3", "Player 3 name", "Username for Player 3 auto-detection.", "Player3"),
            new Option<string>("Player4", "Player 4 name", "Username for Player 4 auto-detection.", "Player4"),
            new Option<bool>("PrepareDageStory", "Prepare Ultra Dage", "Join Legion and complete the Dage challenge story prep.", false),
            new Option<bool>("BankUltraDrops", "Bank Ultra Drops", "Moves insignias and similar ultra drops into the bank.", false),
            new Option<bool>("UnlockMissingEnhancements", "Unlock Missing Enhs", "Checks quest completion and unlocks missing forge/awe enhancements used by your configured gear.", false),
            new Option<bool>("ApplyEnhancements", "Apply Enhancements", "Enhances all configured weapons, helms, and capes.", true),
            new Option<bool>("RestockConsumables", "Restock Consumables", "Buys potions and scrolls used in common ultra setups.", true),
            new Option<int>("malevolenceElixirQuantity", "Potion Quantity", "Target quantity for Potent Malevolence Elixir.", 300),
            new Option<int>("BattleElixirQuantity", "Battle Elixir Qty", "Target quantity for Potent Battle Elixir.", 300),
            new Option<int>("MightTonicQuantity", "Might Tonic Qty", "Target quantity for Might Tonic.", 300),
            new Option<int>("EnrageQuantity", "Enrage Quantity", "Target quantity for Scroll of Enrage.", 1000),
            new Option<int>("LifeStealQuantity", "Life Steal Quantity", "Target quantity for Scroll of Life Steal.", 99),
        };

        Options.AddRange(_equipmentDefinitions.Select(def =>
            (IOption)new Option<string>(def.OptionKey, def.Label, def.Description, string.Empty)
        ));
    }

    public void ScriptMain(IScriptInterface bot)
    {
        Core.BankingBlackList.AddRange(_alwaysKeepOutOfBank);
        Core.SetOptions();

        try
        {
            Run();
        }
        finally
        {
            Core.SetOptions(false);
        }
    }

    private void Run()
    {
        Core.Logger("Ultra prep refactor starting.");

        UltraRole role = ResolveRole();
        Core.Logger($"Resolved role: {role}");

        if (GetBool("PrepareDageStory"))
            PrepareDage();

        if (GetBool("BankUltraDrops"))
            BankUltraDrops();

        UnbankConfiguredGear();
        UnbankAlwaysNeededItems();
        UnbankRoleItems(role);

        if (GetBool("UnlockMissingEnhancements"))
            UnlockConfiguredEnhancements();

        if (GetBool("ApplyEnhancements"))
            ApplyConfiguredEnhancements();

        if (GetBool("RestockConsumables"))
            RestockConsumables();

        Core.Logger("Ultra prep refactor finished.");
    }

    private void PrepareDage()
    {
        Core.Logger("Preparing Legion / Ultra Dage story prerequisites.");
        Legion.JoinLegion();
        DageStory.DageChallengeQuests();
    }

    private void BankUltraDrops()
    {
        Core.Logger("Banking ultra insignias and related drops.");
        Core.ToBank(_ultraDropsToBank);
    }

    private void UnbankAlwaysNeededItems()
    {
        foreach (string item in _alwaysKeepOutOfBank)
            Core.Unbank(item);
    }

    private void UnbankConfiguredGear()
    {
        foreach (ConfiguredEquipmentPlan plan in GetConfiguredEquipmentPlans())
        {
            Core.Unbank(plan.ItemName);
        }
    }

    private void UnbankRoleItems(UltraRole role)
    {
        if (role == UltraRole.Auto)
        {
            Core.Logger("No player role resolved, skipping role-specific class unbanking.");
            return;
        }

        if (!_roleItems.TryGetValue(role, out string[] itemGroups))
            return;

        foreach (string group in itemGroups)
        {
            string selectedItem = ResolvePreferredOwnedItem(group);
            if (string.IsNullOrWhiteSpace(selectedItem))
            {
                Core.Logger($"No owned option found for role item group: {group}");
                continue;
            }

            Core.Unbank(selectedItem);
        }
    }

    private void UnlockConfiguredEnhancements()
    {
        List<string> requiredUnlocks = GetConfiguredEquipmentPlans()
            .Where(plan => !string.IsNullOrWhiteSpace(plan.UnlockKey))
            .Select(plan => plan.UnlockKey!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (requiredUnlocks.Count == 0)
        {
            Core.Logger("No forge or awe unlocks required from the current gear configuration.");
            return;
        }

        Core.Logger("Checking required enhancement unlocks.");
        foreach (string unlockKey in requiredUnlocks)
        {
            EnhancementUnlock? unlock = _unlockChecks.FirstOrDefault(x =>
                x.Key.Equals(unlockKey, StringComparison.OrdinalIgnoreCase)
            );

            if (unlock == null)
            {
                Core.Logger($"No unlock metadata found for {unlockKey}.");
                continue;
            }

            bool completed = Core.isCompletedBefore(unlock.QuestId);
            Core.Logger($"{unlock.DisplayName}: {(completed ? "Unlocked" : "Missing")}");

            if (!completed)
                ExecuteUnlock(unlock.Key);
        }
    }

    private void ExecuteUnlock(string unlockKey)
    {
        if (_unlockActions.TryGetValue(unlockKey, out Action? unlockAction))
        {
            unlockAction();
            return;
        }

        Core.Logger($"Unhandled unlock key: {unlockKey}");
    }

    private void ApplyConfiguredEnhancements()
    {
        Core.Logger("Applying configured enhancements.");

        foreach (ConfiguredEquipmentPlan plan in GetConfiguredEquipmentPlans())
        {
            if (!Owns(plan.ItemName))
            {
                Core.Logger($"Skipping {plan.Label}: '{plan.ItemName}' was not found in inventory or bank.");
                continue;
            }

            if (plan.WeaponSpecial.HasValue)
            {
                Adv.EnhanceItem(
                    plan.ItemName,
                    plan.EnhancementType,
                    wSpecial: plan.WeaponSpecial.Value
                );
                continue;
            }

            if (plan.HelmSpecial.HasValue)
            {
                Adv.EnhanceItem(
                    plan.ItemName,
                    plan.EnhancementType,
                    hSpecial: plan.HelmSpecial.Value
                );
                continue;
            }

            if (plan.CapeSpecial.HasValue)
            {
                Adv.EnhanceItem(
                    plan.ItemName,
                    plan.EnhancementType,
                    cSpecial: plan.CapeSpecial.Value
                );
                continue;
            }

            Adv.EnhanceItem(plan.ItemName, plan.EnhancementType);
        }
    }

    private void RestockConsumables()
    {
        int malevolenceElixirQuantity = Math.Max(1, GetInt("malevolenceElixirQuantity"));
        int battleElixirQuantity = Math.Max(1, GetInt("BattleElixirQuantity"));
        int mightTonicQuantity = Math.Max(1, GetInt("MightTonicQuantity"));
        int enrageQuantity = Math.Max(1, GetInt("EnrageQuantity"));
        int lifeStealQuantity = Math.Max(1, GetInt("LifeStealQuantity"));

        Core.Logger("Restocking ultra consumables.");

        PotionBuyer.INeedYourStrongestPotions(
            new[] { "Potent Malevolence Elixir" },
            PotionQuant: malevolenceElixirQuantity,
            BuyReagents: true,
            Seperate: true
        );

        PotionBuyer.INeedYourStrongestPotions(
            new[] { "Potent Battle Elixir" },
            PotionQuant: battleElixirQuantity,
            BuyReagents: true,
            Seperate: true
        );

        PotionBuyer.INeedYourStrongestPotions(
            new[] { "Might Tonic" },
            PotionQuant: mightTonicQuantity,
            BuyReagents: true,
            Seperate: true
        );

        ScrollsBuyer.BuyScroll(Scrolls.Enrage, enrageQuantity);

        int currentLifeSteal = Bot.Inventory.GetQuantity("Scroll of Life Steal");
        if (currentLifeSteal < lifeStealQuantity)
        {
            Adv.BuyItem(
                "terminatemple",
                2328,
                "Scroll of Life Steal",
                lifeStealQuantity - currentLifeSteal
            );
        }
    }

    private UltraRole ResolveRole()
    {
        UltraRole configuredRole = Bot.Config!.Get<UltraRole>("CurrentRole");
        if (configuredRole != UltraRole.Auto)
            return configuredRole;

        Dictionary<UltraRole, string> configuredNames = new Dictionary<UltraRole, string>
        {
            { UltraRole.Player1, GetText("Player1") },
            { UltraRole.Player2, GetText("Player2") },
            { UltraRole.Player3, GetText("Player3") },
            { UltraRole.Player4, GetText("Player4") }
        };

        foreach (KeyValuePair<UltraRole, string> entry in configuredNames)
        {
            if (string.IsNullOrWhiteSpace(entry.Value))
                continue;

            if (entry.Value.Equals(Bot.Player.Username, StringComparison.OrdinalIgnoreCase))
                return entry.Key;
        }

        return UltraRole.Auto;
    }

    private string ResolvePreferredOwnedItem(string options)
    {
        foreach (string option in options.Split('|'))
        {
            string trimmed = option.Trim();
            if (Owns(trimmed))
                return trimmed;
        }

        return string.Empty;
    }

    private bool Owns(string itemName) => Core.CheckInventory(itemName, toInv: false);

    private List<ConfiguredEquipmentPlan> GetConfiguredEquipmentPlans() =>
        _equipmentDefinitions
            .Select(def => new ConfiguredEquipmentPlan(
                def.OptionKey,
                def.Label,
                GetText(def.OptionKey),
                def.EnhancementType,
                def.UnlockKey,
                def.WeaponSpecial,
                def.HelmSpecial,
                def.CapeSpecial
            ))
            .Where(plan => !string.IsNullOrWhiteSpace(plan.ItemName))
            .ToList();

    private string GetText(string optionName)
    {
        string? value = Bot.Config!.Get<string>(optionName);
        return value?.Trim() ?? string.Empty;
    }

    private bool GetBool(string optionName) => Bot.Config!.Get<bool>(optionName);
    private int GetInt(string optionName) => Bot.Config!.Get<int>(optionName);

    public enum UltraRole
    {
        Auto,
        Player1,
        Player2,
        Player3,
        Player4
    }

    private class EnhancementUnlock
    {
        public EnhancementUnlock(string key, string displayName, int questId)
        {
            Key = key;
            DisplayName = displayName;
            QuestId = questId;
        }

        public string Key { get; }
        public string DisplayName { get; }
        public int QuestId { get; }
    }

    private class EquipmentDefinition
    {
        public EquipmentDefinition(
            string optionKey,
            string label,
            string description,
            EnhancementType enhancementType,
            string? unlockKey = null,
            WeaponSpecial? weaponSpecial = null,
            HelmSpecial? helmSpecial = null,
            CapeSpecial? capeSpecial = null
        )
        {
            OptionKey = optionKey;
            Label = label;
            Description = description;
            EnhancementType = enhancementType;
            UnlockKey = unlockKey;
            WeaponSpecial = weaponSpecial;
            HelmSpecial = helmSpecial;
            CapeSpecial = capeSpecial;
        }

        public string OptionKey { get; }
        public string Label { get; }
        public string Description { get; }
        public EnhancementType EnhancementType { get; }
        public string? UnlockKey { get; }
        public WeaponSpecial? WeaponSpecial { get; }
        public HelmSpecial? HelmSpecial { get; }
        public CapeSpecial? CapeSpecial { get; }
    }

    private class ConfiguredEquipmentPlan
    {
        public ConfiguredEquipmentPlan(
            string optionKey,
            string label,
            string itemName,
            EnhancementType enhancementType,
            string? unlockKey = null,
            WeaponSpecial? weaponSpecial = null,
            HelmSpecial? helmSpecial = null,
            CapeSpecial? capeSpecial = null
        )
        {
            OptionKey = optionKey;
            Label = label;
            ItemName = itemName;
            EnhancementType = enhancementType;
            UnlockKey = unlockKey;
            WeaponSpecial = weaponSpecial;
            HelmSpecial = helmSpecial;
            CapeSpecial = capeSpecial;
        }

        public string OptionKey { get; }
        public string Label { get; }
        public string ItemName { get; }
        public EnhancementType EnhancementType { get; }
        public string? UnlockKey { get; }
        public WeaponSpecial? WeaponSpecial { get; }
        public HelmSpecial? HelmSpecial { get; }
        public CapeSpecial? CapeSpecial { get; }
    }
}
