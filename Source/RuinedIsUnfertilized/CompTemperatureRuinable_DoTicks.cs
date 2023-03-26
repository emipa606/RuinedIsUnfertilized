using HarmonyLib;
using RimWorld;
using Verse;

namespace RuinedIsUnfertilized;

[HarmonyPatch(typeof(CompTemperatureRuinable), "DoTicks")]
public static class CompTemperatureRuinable_DoTicks
{
    public static void Postfix(ref CompTemperatureRuinable __instance, float ___ruinedPercent)
    {
        if (___ruinedPercent < 1f)
        {
            return;
        }

        var parentDef = __instance?.parent?.def;

        if (parentDef == null)
        {
            return;
        }

        var egg = __instance.parent;
        if (RuinedIsUnfertilized.ignoredEggs.Contains(egg))
        {
            return;
        }

        if (RuinedIsUnfertilized.ignoredEggTypes.Contains(parentDef))
        {
            if (parentDef.thingCategories?.Any(def => def == RuinedIsUnfertilized.fertilizedEggsCategoryDef) ==
                false)
            {
                RuinedIsUnfertilized.ignoredEggs.Add(egg);
                return;
            }
        }

        var hatcher = parentDef.GetCompProperties<CompProperties_Hatcher>()?.hatcherPawn;
        if (hatcher == null)
        {
            RuinedIsUnfertilized.ignoredEggs.Add(egg);
            return;
        }

        var unfertilizedEggDef = hatcher.race?.GetCompProperties<CompProperties_EggLayer>()?.eggUnfertilizedDef;
        if (unfertilizedEggDef == null)
        {
            RuinedIsUnfertilized.ignoredEggTypes.Add(parentDef);
            RuinedIsUnfertilized.ignoredEggs.Add(egg);
            return;
        }

        var location = __instance.parent.Position;
        var map = __instance.parent.Map;
        var count = __instance.parent.stackCount;
        __instance.parent.Destroy();
        var thing = ThingMaker.MakeThing(unfertilizedEggDef);
        thing.stackCount = count;
        GenSpawn.Spawn(thing, location, map);
        RuinedIsUnfertilized.ignoredEggs.Add(thing);
        //Log.Message($"{parentDef.label} was ruined by temperature and replaced with {unfertilizedEggDef.label}");
    }
}