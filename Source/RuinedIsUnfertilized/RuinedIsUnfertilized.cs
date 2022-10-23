using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RuinedIsUnfertilized;

[StaticConstructorOnStartup]
public class RuinedIsUnfertilized
{
    public static readonly ThingCategoryDef fertilizedEggsCategoryDef;
    public static readonly List<ThingDef> ignoredEggs;

    static RuinedIsUnfertilized()
    {
        fertilizedEggsCategoryDef = ThingCategoryDef.Named("EggsFertilized");
        ignoredEggs = new List<ThingDef>();
        var harmony = new Harmony("Mlie.RuinedIsUnfertilized");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}