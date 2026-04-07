/*
name: CoreFarmerJoeRefactored
description: Refactored copy of CoreFarmerJoe with cached state and milestone-driven progression
tags: farmer, joe, refactored, optimized
*/
#region includes
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Chaos/DrakathsArmor.cs
//cs_include Scripts/Chaos/EternalDrakathSet.cs
//cs_include Scripts/Dailies/0AllDailies.cs
//cs_include Scripts/Dailies/Cryomancer.cs
//cs_include Scripts/Dailies/LordOfOrder.cs
//cs_include Scripts/Darkon/CoreDarkon.cs
//cs_include Scripts/Darkon/Various/PrinceDarkonsPoleaxePreReqs.cs
//cs_include Scripts/Enhancement/UnlockForgeEnhancements.cs
//cs_include Scripts/Good/GearOfAwe/CapeOfAwe.cs
//cs_include Scripts/Good/ArchPaladin.cs
//cs_include Scripts/Hollowborn/MergeShops/ShadowrealmMerge.cs
//cs_include Scripts/Legion/YamiNoRonin/CoreYnR.cs
//cs_include Scripts/Nation/AFDL/EnoughDOOMforanArchfiend.cs
//cs_include Scripts/Nation/Various/Archfiend.cs
//cs_include Scripts/Other/Classes/BloodSorceress.cs
//cs_include Scripts/Other/Classes/Daily-Classes/BlazeBinder.cs
//cs_include Scripts/Other/Classes/DragonOfTime.cs
//cs_include Scripts/Other/Classes/DragonShinobi.cs
//cs_include Scripts/Other/Classes/Dragonslayer.cs
//cs_include Scripts/Other/Classes/DragonslayerGeneral.cs
//cs_include Scripts/Other/Classes/REP-based/GlacialBerserker.cs
//cs_include Scripts/Other/Classes/REP-based/MasterRanger.cs
//cs_include Scripts/Other/Classes/REP-based/Shaman.cs
//cs_include Scripts/Other/Classes/ScarletSorceress.cs
//cs_include Scripts/Other/Classes/FrostSpiritReaver.cs
//cs_include Scripts/Other/FreeBoosts/FreeBoostsQuest(10mns)[Rng].cs
//cs_include Scripts/Other/MergeShops/SynderesMerge.cs
//cs_include Scripts/Other/Weapons/BurningBlade.cs
//cs_include Scripts/Other/Weapons/BurningBladeOfAbezeth.cs
//cs_include Scripts/Other/Weapons/ExaltedApotheosisPreReqs.cs
//cs_include Scripts/Seasonal/Frostvale/NorthlandsMonk.cs
//cs_include Scripts/Story/Glacera.cs
//cs_include Scripts/Story/LordsofChaos/Core13LoC.cs
//cs_include Scripts/Story/Mazumi.cs
//cs_include Scripts/Story/QueenofMonsters/Extra/CelestialArena.cs
//cs_include Scripts/Story/Tutorial.cs
#endregion includes

using System;
using System.Collections.Generic;
using System.Linq;
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Options;

public class CoreFarmerJoeRefactored
{
    public static IScriptInterface Bot => IScriptInterface.Instance;
    public static CoreBots Core => CoreBots.Instance;

    private static CoreAdvanced Adv => _adv ??= new CoreAdvanced();
    private static CoreAdvanced _adv;
    private static CoreDailies Daily => _daily ??= new CoreDailies();
    private static CoreDailies _daily;
    private static CoreFarms Farm => _farm ??= new CoreFarms();
    private static CoreFarms _farm;
    private static FreeBoosts Boosts => _boosts ??= new FreeBoosts();
    private static FreeBoosts _boosts;
    private static Tutorial Tutorial => _tutorial ??= new Tutorial();
    private static Tutorial _tutorial;
    private static Mazumi Mazumi => _mazumi ??= new Mazumi();
    private static Mazumi _mazumi;
    private static MasterRanger MasterRanger => _masterRanger ??= new MasterRanger();
    private static MasterRanger _masterRanger;
    private static Dragonslayer Dragonslayer => _dragonslayer ??= new Dragonslayer();
    private static Dragonslayer _dragonslayer;
    private static ScarletSorceress ScarletSorceress => _scarletSorceress ??= new ScarletSorceress();
    private static ScarletSorceress _scarletSorceress;
    private static DragonslayerGeneral DragonslayerGeneral => _dragonslayerGeneral ??= new DragonslayerGeneral();
    private static DragonslayerGeneral _dragonslayerGeneral;
    private static BurningBlade BurningBlade => _burningBlade ??= new BurningBlade();
    private static BurningBlade _burningBlade;
    private static BlazeBinder BlazeBinder => _blazeBinder ??= new BlazeBinder();
    private static BlazeBinder _blazeBinder;
    private static Cryomancer Cryomancer => _cryomancer ??= new Cryomancer();
    private static Cryomancer _cryomancer;
    private static DragonShinobi DragonShinobi => _dragonShinobi ??= new DragonShinobi();
    private static DragonShinobi _dragonShinobi;
    private static GlacialBerserker GlacialBerserker => _glacialBerserker ??= new GlacialBerserker();
    private static GlacialBerserker _glacialBerserker;
    private static ArchPaladin ArchPaladin => _archPaladin ??= new ArchPaladin();
    private static ArchPaladin _archPaladin;
    private static EnoughDOOMforanArchfiend ArchfiendDoomLord => _archfiendDoomLord ??= new EnoughDOOMforanArchfiend();
    private static EnoughDOOMforanArchfiend _archfiendDoomLord;
    private static ArchFiend Archfiend => _archfiend ??= new ArchFiend();
    private static ArchFiend _archfiend;
    private static CapeOfAwe CapeOfAwe => _capeOfAwe ??= new CapeOfAwe();
    private static CapeOfAwe _capeOfAwe;
    private static LordOfOrder LordOfOrder => _lordOfOrder ??= new LordOfOrder();
    private static LordOfOrder _lordOfOrder;
    private static FrostSpiritReaver FrostSpiritReaver => _frostSpiritReaver ??= new FrostSpiritReaver();
    private static FrostSpiritReaver _frostSpiritReaver;
    private static NorthlandsMonk NorthlandsMonk => _northlandsMonk ??= new NorthlandsMonk();
    private static NorthlandsMonk _northlandsMonk;
    private static Shaman Shaman => _shaman ??= new Shaman();
    private static Shaman _shaman;
    private static UnlockForgeEnhancements Forge => _forge ??= new UnlockForgeEnhancements();
    private static UnlockForgeEnhancements _forge;
    private static CelestialArenaQuests CelestialArena => _celestialArena ??= new CelestialArenaQuests();
    private static CelestialArenaQuests _celestialArena;
    private static BurningBladeOfAbezeth BurningBladeOfAbezeth => _burningBladeOfAbezeth ??= new BurningBladeOfAbezeth();
    private static BurningBladeOfAbezeth _burningBladeOfAbezeth;
    private static CoreYnR YnR => _ynr ??= new CoreYnR();
    private static CoreYnR _ynr;
    private static DragonOfTime DragonOfTime => _dragonOfTime ??= new DragonOfTime();
    private static DragonOfTime _dragonOfTime;
    private static ShadowrealmMerge ShadowrealmMerge => _shadowrealmMerge ??= new ShadowrealmMerge();
    private static ShadowrealmMerge _shadowrealmMerge;
    private static SynderesMerge SynderesMerge => _synderesMerge ??= new SynderesMerge();
    private static SynderesMerge _synderesMerge;
    private static ExaltedApotheosisPreReqs ExaltedApotheosisPreReqs => _exaltedApotheosisPreReqs ??= new ExaltedApotheosisPreReqs();
    private static ExaltedApotheosisPreReqs _exaltedApotheosisPreReqs;

    private const int Rank10ClassPoints = 302500;

    public string OptionsStorage = "FarmerJoePetRefactored";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        new Option<bool>("OutFit", "Get a Pre-Made Outfit", "Farm the themed outfit pieces near the end.", false),
        new Option<bool>("EquipOutfit", "Equip Outfit", "Equip the FarmerJoe outfit at the end.", false),
        new Option<bool>("SellStarterClasses", "Sell Starter Classes", "Allow cleanup of weak starter classes after better ones are secured.", false),
        new Option<bool>("GetBoosts", "Get Boosts", "Farm free boosts to speed up leveling and rep gains.", false),
        new Option<PetChoice>("PetChoice", "Choose Your Pet", "Pick an optional pet to farm and equip.", PetChoice.None),
        CoreBots.Instance.SkipOptions,
    };

    private static readonly string[] SoloClassPriority =
    {
        "Void Highlord",
        "Legion Revenant",
        "Dragon of Time",
        "ArchPaladin",
        "Dragonslayer General",
        "Glacial Berserker",
        "Dragonslayer",
        "DragonSoul Shinobi",
        "Assassin",
        "Classic Ninja",
        "Ninja Warrior",
        "Ninja",
        "Ninja (Rare)",
        "Rogue (Rare)",
        "Rogue",
        "Healer (Rare)",
        "Healer",
    };

    private static readonly string[] FarmClassPriority =
    {
        "Legion Revenant",
        "Blaze Binder",
        "Archfiend",
        "Scarlet Sorceress",
        "Master Ranger",
        "Mage (Rare)",
        "Mage",
    };

    private PlayerState _state;
    private string _resolvedSolo;
    private string _resolvedFarm;
    private int _enhancedAtLevel = -1;
    private string _enhancedClassName;
    private int _stateVersion;
    private int _classConfigVersion = -1;
    private bool _milestoneSetupPrepared;

    public static void ScriptMain(IScriptInterface bot) => Core.RunCore();

    public void DoAll()
    {
        Adv.GearStore(EnhAfter: true);
        RefreshState(force: true);
        EnsureConfiguredClasses();
        EnsureBoostStock();

        RunMilestone("Beginner", () => !NeedsBeginnerProgression(), RunBeginnerProgression);
        RunMilestone("Level 30", () => !NeedsLevel30Milestone(), RunLevel30Milestone);
        RunMilestone("Level 50", () => !NeedsLevel50Milestone(), RunLevel50Milestone);
        RunMilestone("Level 55", () => !NeedsLevel55Milestone(), RunLevel55Milestone);
        RunMilestone("Level 60", () => !NeedsLevel60Milestone(), RunLevel60Milestone);
        RunMilestone("Level 65", () => !NeedsLevel65Milestone(), RunLevel65Milestone);
        RunMilestone("Level 75", () => !NeedsLevel75Milestone(), RunLevel75Milestone);
        RunMilestone("Late Game", () => !NeedsLateGameProgression(), RunLateGameProgression);
        RunMilestone("End Game", () => false, RunEndGame);

        if (Bot.Config.Get<bool>("OutFit"))
            RunOutfit();

        RunPets(Bot.Config.Get<PetChoice>("PetChoice"));
        CleanupClasses();
        Adv.GearStore(true, EnhAfter: true);
    }

    private void RunMilestone(string name, Func<bool> skip, Action action)
    {
        RefreshState();
        if (skip())
        {
            Core.Logger($"Skipping {name}: requirements already met.");
            return;
        }

        Core.Logger($"Starting {name}.");
        _milestoneSetupPrepared = false;
        PrepareMilestoneSetup();
        action();
        _milestoneSetupPrepared = false;
        RefreshState(force: true);
    }

    private void RunBeginnerProgression()
    {
        EnsureTutorialAccess();
        EnsureBasicGear();
        EnsureLevel(10);

        if (!HasAnyItem("Rogue", "Rogue (Rare)") && !HasRank10Class("Rogue"))
            Adv.BuyItem("classhalla", 172, "Rogue");

        if (!HasAnyItem("Ninja", "Ninja Warrior", "Assassin"))
        {
            Adv.RankUpClass("Rogue");
            RefreshState(force: true);
            EnsureLevel(25);
            EnsureConfiguredClasses(forceRefresh: true);
            Mazumi.MazumiQuests();
            Core.BuyItem("classhalla", 178, "Ninja");
            InvalidateState();
        }

        EnsureRank10Class("Ninja");

        if (!HasAnyItem("Mage", "Mage (Rare)"))
        {
            Core.Logger("Getting Mage as starter farming class.");
            Adv.BuyItem("classhalla", 174, 15653, shopItemID: 9845);
            InvalidateState();
        }

        EnsureConfiguredClasses(forceRefresh: true);
        Core.Logger("Class points may desync at rank 9. If you get stuck, relog and rerun.");
    }

    private void RunLevel30Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(30);

        if (Daily.CheckDailyv2(802, true, true, "Elders' Blood"))
            Daily.EldersBlood();

        EnsureClass("Master Ranger", delegate
        {
            if (HasItem("Venom Head"))
                Core.SellItem("Venom Head");
            MasterRanger.GetMR();
        });

        EnsureClass("Dragonslayer", delegate { Dragonslayer.GetDragonslayer(); });
        EnsureBladeOfAweSetup();
    }

    private void RunLevel50Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(50);

        EnsureClass("Scarlet Sorceress", delegate { ScarletSorceress.GetSSorc(); });
        EnsureClass("Dragonslayer General", delegate { DragonslayerGeneral.GetDSGeneral(); });

        if (!HasItem("Burning Blade"))
        {
            Core.Logger("Getting Burning Blade.");
            BurningBlade.GetBurningBlade();
            InvalidateState();
        }
    }

    private void RunLevel55Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(55);

        EnsureClass("Blaze Binder", delegate { BlazeBinder.GetClass(); });
        EnsureClass("Cryomancer", delegate { Cryomancer.DoCryomancer(); });
    }

    private void RunLevel60Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(60);
        EnsureClass("DragonSoul Shinobi", delegate { DragonShinobi.GetDSS(); });
    }

    private void RunLevel65Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(65);

        EnsureClass("Glacial Berserker", delegate { GlacialBerserker.GetGB(); });
        EnsureClass("ArchPaladin", delegate { ArchPaladin.GetAP(); });
    }

    private void RunLevel75Milestone()
    {
        EnsureConfiguredClasses();
        EnsureLevel(75);

        bool needsAfdl = !HasItem("Archfiend DoomLord")
            || !Adv.HasMinimalBoost(GenericGearBoost.dmgAll, 30);

        if (needsAfdl)
        {
            Core.Logger("Getting Archfiend DoomLord.");
            ArchfiendDoomLord.AFDL();
            InvalidateState();
        }

        EnsureClass("Archfiend", delegate { Archfiend.GetArchfiend(); });
    }

    private void RunLateGameProgression()
    {
        EnsureConfiguredClasses();
        EnsureHealerForChaos();

        if (!HasItem("Cape of Awe"))
        {
            CapeOfAwe.GetCoA();
            InvalidateState();
        }

        if (HasItem("Cape of Awe"))
            Core.Equip("Cape of Awe");

        EnsureConfiguredClasses(forceRefresh: true);
        Core13LoC loc = new Core13LoC();
        loc.Complete13LOC();

        LordOfOrder.GetLoO();
        string[] looRewards = Core.EnsureLoad(7156).Rewards.Select(i => i.Name).ToArray();
        if (looRewards.Length > 0)
            Core.ToBank(looRewards);

        EnsureClass("Frost Spirit Reaver", delegate { FrostSpiritReaver.GetFSR(); });
        EnsureClass("Northlands Monk", delegate { NorthlandsMonk.GetNlMonk(); });
        EnsureClass("Shaman", delegate { Shaman.GetShaman(); });

        UnlockCoreForgeEnhancements();

        EnsureLevel(80);
        EnsureConfiguredClasses();

        if (!HasItem("Blinding Bright of Destiny"))
        {
            CelestialArena.DoAll();
            BurningBladeOfAbezeth.GetBBoA();
            InvalidateState();
        }

        UnlockCapeForgeEnhancements();

        if (!HasItem("Yami no Ronin") && !HasItem("Yin & Yang Roentgenium"))
            YnR.GetYnR();

        EnsureConfiguredClasses(forceRefresh: true);
        EnsureClass("Dragon of Time", delegate { DragonOfTime.GetDoT(); });
        EnsureLevel(100);

        if (!HasItem("Hollowborn Reaper's Scythe"))
        {
            ShadowrealmMerge.BuyAllMerge("Hollowborn Reaper's Scythe");
            InvalidateState();
        }
    }

    private void RunEndGame()
    {
        EnsureConfiguredClasses();
        Forge.HerosValiance();
        Forge.Elysium();
        Forge.ArcanasConcerto();
        Forge.Ravenous();
        Forge.DauntLess();
        ExaltedApotheosisPreReqs.PreReqs();
    }

    public void RunOutfit()
    {
        EnsureConfiguredClasses();

        if (!HasItem("NO BOTS Armor"))
            SynderesMerge.BuyAllMerge("NO BOTS Armor");

        if (!HasItem("Scarecrow Hat"))
            Adv.BuyItem("yulgar", 16, "Scarecrow Hat");

        if (!HasItem("The Server is Down"))
        {
            Core.HuntMonster(
                "undergroundlabb",
                "Rabid Server Hamster",
                "The Server is Down",
                isTemp: false,
                log: false
            );
            Bot.Wait.ForPickup("The Server is Down");
            InvalidateState();
        }

        EnsureSmartEnhance();

        if (Bot.Config.Get<bool>("EquipOutfit"))
        {
            Core.Equip(
                new[]
                {
                    "NO BOTS Armor",
                    "Scarecrow Hat",
                    "The Server is Down",
                    "Hollowborn Reaper's Scythe",
                }
            );
        }
    }

    public void RunPets(PetChoice petChoice = PetChoice.None)
    {
        if (petChoice == PetChoice.None)
            return;

        EnsureConfiguredClasses();

        switch (petChoice)
        {
            case PetChoice.HotMama:
                if (!HasItem("Hot Mama"))
                {
                    Core.HuntMonster("battleundere", "Hot Mama", "Hot Mama", isTemp: false, log: false);
                    Bot.Wait.ForPickup("Hot Mama");
                    InvalidateState();
                }
                if (HasItem("Hot Mama"))
                    Core.Equip("Hot Mama");
                break;

            case PetChoice.Akriloth:
                if (!HasItem("Akriloth Pet"))
                {
                    Core.HuntMonster(
                        "gravestrike",
                        "Ultra Akriloth",
                        "Akriloth Pet",
                        isTemp: false,
                        log: false
                    );
                    Bot.Wait.ForPickup("Akriloth Pet");
                    InvalidateState();
                }
                if (HasItem("Akriloth Pet"))
                    Core.Equip("Akriloth Pet");
                break;
        }
    }

    private void EnsureConfiguredClasses(bool forceRefresh = false)
    {
        RefreshState(forceRefresh);

        if (!forceRefresh && _classConfigVersion == _stateVersion)
            return;

        Core.ReadCBO();

        string solo = ResolveClass(Core.SoloClass, SoloClassPriority);
        string farm = ResolveClass(Core.FarmClass, FarmClassPriority);

        if (!string.IsNullOrEmpty(solo) && !string.Equals(Core.SoloClass, solo, StringComparison.OrdinalIgnoreCase))
            Core.SoloClass = solo;

        if (!string.IsNullOrEmpty(farm) && !string.Equals(Core.FarmClass, farm, StringComparison.OrdinalIgnoreCase))
            Core.FarmClass = farm;

        _resolvedSolo = solo;
        _resolvedFarm = farm;

        if (!string.IsNullOrEmpty(Core.SoloClass) && !HasRank10Class(Core.SoloClass))
            Adv.RankUpClass(Core.SoloClass);

        if (!string.IsNullOrEmpty(Core.FarmClass) && !HasRank10Class(Core.FarmClass))
            Adv.RankUpClass(Core.FarmClass);

        _classConfigVersion = _stateVersion;
    }

    private string ResolveClass(string configured, IEnumerable<string> priority)
    {
        if (!string.IsNullOrEmpty(configured)
            && !string.Equals(configured, "Generic", StringComparison.OrdinalIgnoreCase)
            && HasItem(configured))
        {
            return configured;
        }

        string resolved = priority.FirstOrDefault(HasItem);
        return resolved ?? configured;
    }

    private void EnsureClass(string className, Action acquire)
    {
        RefreshState();
        if (HasRank10Class(className))
            return;

        Core.Logger($"Acquiring {className}.");
        PrepareMilestoneSetup();
        acquire();
        InvalidateState();
        RefreshState(force: true);

        if (HasItem(className) && !HasRank10Class(className))
            Adv.RankUpClass(className);

        InvalidateState();
    }

    private void EnsureRank10Class(string className)
    {
        RefreshState();
        if (!HasItem(className))
            return;

        if (HasRank10Class(className))
            return;

        Adv.RankUpClass(className);
        InvalidateState();
    }

    private void EnsureBladeOfAweSetup()
    {
        if (HasItem("Awethur's Accoutrements"))
            return;

        Core.Logger("Farming Blade of Awe reputation and getting Awethur's Accoutrements.");
        Farm.BladeofAweREP();
        Adv.BuyItem("museum", 631, "Awethur's Accoutrements");
        InvalidateState();
    }

    private void EnsureHealerForChaos()
    {
        RefreshState();

        if (HasRank10Class("Dragon of Time"))
            return;

        if (HasAnyItem("Dragon of Time"))
        {
            EnsureRank10Class("Dragon of Time");
            return;
        }

        string healer = HasItem("Healer (Rare)") ? "Healer (Rare)" : "Healer";
        if (!HasAnyItem("Healer", "Healer (Rare)"))
        {
            Adv.BuyItem("classhalla", 176, "Healer");
            InvalidateState();
        }

        EnsureRank10Class(healer);
    }

    private void UnlockCoreForgeEnhancements()
    {
        Forge.ForgeHelmEnhancement();
        Forge.Vim();
        Forge.Examen();
        Forge.Anima();
        Forge.Pneuma();
        Forge.ForgeWeaponEnhancement();
        Forge.Lacerate();
        Forge.Smite();
        Forge.Praxis();
    }

    private void UnlockCapeForgeEnhancements()
    {
        Forge.ForgeCapeEnhancement();
        Forge.Absolution();
        Forge.Vainglory();
        Forge.Avarice();
        Forge.Lament();
    }

    private void EnsureBoostStock()
    {
        if (!Bot.Config.Get<bool>("GetBoosts"))
            return;

        Core.OneTimeMessage(
            "GetBoosts Message",
            "Farming a stock of free boosts once so later milestones spend less time on setup."
        );

        Farm.GetBoost("REP", 10, true);
        if (Bot.Player.Level < 100)
            Farm.GetBoost("XP", 10, true);
        Boosts.GetBoostsSelect(10, 10, 0);
    }

    private void EnsureLevel(int targetLevel)
    {
        if (Bot.Player.Level >= targetLevel)
            return;

        PrepareMilestoneSetup();
        Farm.Experience(targetLevel);
        RefreshState(force: true);
    }

    private void PrepareMilestoneSetup()
    {
        if (_milestoneSetupPrepared)
            return;

        EnsureConfiguredClasses();
        EnsureSmartEnhance();
        _milestoneSetupPrepared = true;
    }

    private void EnsureTutorialAccess()
    {
        if (Bot.Player.Level > 1 || HasItem("Battle Oracle Hood") || HasItem("Battle Oracle Wings"))
            return;

        Core.Logger("Doing Tutorial badges for fresh accounts.");
        Tutorial.Badges();
        RefreshState(force: true);
    }

    private void EnsureBasicGear()
    {
        bool hasHelmEquipped = Bot.Inventory.Items.Any(i => i.Equipped && i.Category == ItemCategory.Helm);
        bool hasCapeEquipped = Bot.Inventory.Items.Any(i => i.Equipped && i.Category == ItemCategory.Cape);
        InventoryItem defaultWeapon = Bot.Inventory.Items.FirstOrDefault(i => i.Name.StartsWith("Default", StringComparison.OrdinalIgnoreCase));

        if (!hasHelmEquipped)
        {
            Core.BuyItem("classhalla", 299, "Battle Oracle Hood");
            Core.Equip("Battle Oracle Hood");
        }

        if (!hasCapeEquipped)
        {
            Core.BuyItem("classhalla", 299, "Battle Oracle Wings");
            Core.Equip("Battle Oracle Wings");
        }

        if (defaultWeapon != null && Core.CheckInventory(defaultWeapon.ID))
        {
            Core.BuyItem("classhalla", 299, "Battle Oracle Battlestaff");
            Core.Equip("Battle Oracle Battlestaff");
            Bot.Shops.SellItem(defaultWeapon.ID);
            Bot.Wait.ForTrue(delegate { return !Bot.Inventory.Contains(defaultWeapon.ID); }, 20);
        }
    }

    private void EnsureSmartEnhance()
    {
        string currentClass = Bot.Player.CurrentClass != null
            ? Bot.Player.CurrentClass.Name
            : (!string.IsNullOrEmpty(Core.FarmClass) ? Core.FarmClass : Core.SoloClass ?? string.Empty);

        if (string.IsNullOrEmpty(currentClass))
            return;

        bool staleGearExists = Bot.Inventory.Items.Any(item => item != null && item.EnhancementLevel < Bot.Player.Level);
        if (_enhancedAtLevel == Bot.Player.Level
            && string.Equals(_enhancedClassName, currentClass, StringComparison.OrdinalIgnoreCase)
            && !staleGearExists)
        {
            return;
        }

        Adv.SmartEnhance(currentClass);
        _enhancedAtLevel = Bot.Player.Level;
        _enhancedClassName = currentClass;
    }

    private void CleanupClasses()
    {
        if (!Bot.Config.Get<bool>("SellStarterClasses"))
            return;

        RefreshState();
        string[] starterClasses =
        {
            "Rogue",
            "Mage",
            "Healer",
        };

        foreach (string starter in starterClasses)
        {
            if (HasItem(starter)
                && !string.Equals(starter, _resolvedSolo, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(starter, _resolvedFarm, StringComparison.OrdinalIgnoreCase))
            {
                Core.ToBank(starter);
            }
        }
    }

    private bool NeedsBeginnerProgression()
    {
        RefreshState();

        bool hasHighTierSolo = SoloClassPriority
            .Except(new[] { "Assassin", "Ninja Warrior", "Ninja", "Rogue", "Rogue (Rare)", "Healer", "Healer (Rare)" }, StringComparer.OrdinalIgnoreCase)
            .Any(HasItem);

        bool hasHighTierFarm = FarmClassPriority
            .Except(new[] { "Mage", "Mage (Rare)" }, StringComparer.OrdinalIgnoreCase)
            .Any(HasItem);

        bool hasRankedBeginners = Bot.Player.Level >= 30
            && HasRank10Class("Ninja")
            && (HasRank10Class("Mage") || HasRank10Class("Mage (Rare)"));

        return !(hasHighTierSolo && hasHighTierFarm) && !hasRankedBeginners;
    }

    private bool NeedsLevel30Milestone()
    {
        return Bot.Player.Level < 30
            || !HasRank10Class("Master Ranger")
            || !HasRank10Class("Dragonslayer")
            || !HasItem("Awethur's Accoutrements");
    }

    private bool NeedsLevel50Milestone()
    {
        return Bot.Player.Level < 50
            || !HasRank10Class("Scarlet Sorceress")
            || !HasRank10Class("Dragonslayer General")
            || !HasItem("Burning Blade");
    }

    private bool NeedsLevel55Milestone()
    {
        return Bot.Player.Level < 55
            || !HasRank10Class("Blaze Binder")
            || !HasRank10Class("Cryomancer");
    }

    private bool NeedsLevel60Milestone()
    {
        return Bot.Player.Level < 60 || !HasRank10Class("DragonSoul Shinobi");
    }

    private bool NeedsLevel65Milestone()
    {
        return Bot.Player.Level < 65
            || !HasRank10Class("Glacial Berserker")
            || !HasRank10Class("ArchPaladin");
    }

    private bool NeedsLevel75Milestone()
    {
        return Bot.Player.Level < 75
            || !HasRank10Class("Archfiend")
            || !HasItem("Archfiend DoomLord")
            || !Adv.HasMinimalBoost(GenericGearBoost.dmgAll, 30);
    }

    private bool NeedsLateGameProgression()
    {
        RefreshState();
        return Bot.Player.Level < 100
            || !HasItem("Cape of Awe")
            || !HasRank10Class("Dragon of Time")
            || !HasItem("Hollowborn Reaper's Scythe")
            || !HasAnyItem("Yami no Ronin", "Yin & Yang Roentgenium")
            || !HasRank10Class("Frost Spirit Reaver")
            || !HasRank10Class("Northlands Monk")
            || !HasRank10Class("Shaman");
    }

    private bool HasItem(string itemName)
    {
        RefreshState();
        return _state.OwnedItems.Contains(itemName);
    }

    private bool HasAnyItem(params string[] itemNames)
    {
        RefreshState();
        return itemNames.Any(name => _state.OwnedItems.Contains(name));
    }

    private bool HasRank10Class(string className)
    {
        RefreshState();
        return _state.Rank10Classes.Contains(className);
    }

    private void RefreshState(bool force = false)
    {
        if (!force && _state != null)
            return;

        IEnumerable<InventoryItem> items = Bot.Inventory.Items.Concat(Bot.Bank.Items).Where(i => i != null);
        HashSet<string> owned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> rank10 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (InventoryItem item in items)
        {
            if (string.IsNullOrEmpty(item.Name))
                continue;

            owned.Add(item.Name.Trim());
            if (item.Category == ItemCategory.Class && item.Quantity >= Rank10ClassPoints)
                rank10.Add(item.Name.Trim());
        }

        _state = new PlayerState
        {
            OwnedItems = owned,
            Rank10Classes = rank10,
        };
    }

    private void InvalidateState()
    {
        _state = null;
        _stateVersion++;
        _classConfigVersion = -1;
        _milestoneSetupPrepared = false;
    }

    private sealed class PlayerState
    {
        public HashSet<string> OwnedItems { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> Rank10Classes { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public enum PetChoice
    {
        None,
        HotMama,
        Akriloth,
    }
}
