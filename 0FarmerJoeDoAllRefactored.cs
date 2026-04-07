/*
name: FarmerJoeKit0DoAllRefactored
description: Runs the refactored Farmer Joe core script
tags: farmer, joe, refactored
*/
#region includes
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/Tools/BankAllItems.cs
//cs_include Scripts/Other/FarmJoeKits/skua-utils/CoreFarmerJoeRefactored.cs
#endregion includes

using Skua.Core.Interfaces;
using Skua.Core.Options;

public class FarmerJoeKit0DoAllRefactored
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    private static CoreFarmerJoeRefactored CFJ
    {
        get => _CFJ ??= new CoreFarmerJoeRefactored();
        set => _CFJ = value;
    }
    private static CoreFarmerJoeRefactored _CFJ;
    private static BankAllItems BAI
    {
        get => _BAI ??= new BankAllItems();
        set => _BAI = value;
    }
    private static BankAllItems _BAI;

    public string OptionsStorage = CFJ.OptionsStorage;
    public List<IOption> Options = CFJ.Options;
    public bool DontPreconfigure = true;

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        FJDoAll();

        Core.SetOptions(false);
    }

    public void FJDoAll()
    {
        CFJ.DoAll();

        // BAI.BankAll();
    }
}
