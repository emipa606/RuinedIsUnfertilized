using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RuinedIsUnfertilized;

[StaticConstructorOnStartup]
public class RuinedIsUnfertilized
{
    public static readonly ThingCategoryDef fertilizedEggsCategoryDef;
    public static readonly List<ThingDef> ignoredEggTypes;
    public static readonly List<Thing> ignoredEggs;

    static RuinedIsUnfertilized()
    {
        fertilizedEggsCategoryDef = ThingCategoryDef.Named("EggsFertilized");
        ignoredEggTypes = new List<ThingDef>();
        ignoredEggs = new List<Thing>();
        var harmony = new Harmony("Mlie.RuinedIsUnfertilized");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}