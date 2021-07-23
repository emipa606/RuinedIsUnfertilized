using HarmonyLib;
using RimWorld;
using Verse;

namespace RuinedIsUnfertilized
{
    [HarmonyPatch(typeof(CompTemperatureRuinable), "DoTicks")]
    public class CompTemperatureRuinable_DoTicks
    {
        public static void Postfix(ref CompTemperatureRuinable __instance, float ___ruinedPercent)
        {
            if (___ruinedPercent < 1f)
            {
                return;
            }

            var parentDef = __instance.parent.def;

            if (RuinedIsUnfertilized.ignoredEggs.Contains(parentDef))
            {
                if (parentDef.thingCategories?.Any(def => def == RuinedIsUnfertilized.fertilizedEggsCategoryDef) ==
                    false)
                {
                    return;
                }
            }

            var hatcher = parentDef?.GetCompProperties<CompProperties_Hatcher>().hatcherPawn;
            var unfertilizedEggDef = hatcher?.race.GetCompProperties<CompProperties_EggLayer>().eggUnfertilizedDef;
            if (unfertilizedEggDef == null)
            {
                RuinedIsUnfertilized.ignoredEggs.Add(parentDef);
                return;
            }

            var location = __instance.parent.Position;
            var map = __instance.parent.Map;
            var count = __instance.parent.stackCount;
            __instance.parent.Destroy();
            var thing = ThingMaker.MakeThing(unfertilizedEggDef);
            thing.stackCount = count;
            GenSpawn.Spawn(thing, location, map);

            //Log.Message($"{parentDef.label} was ruined by temperature and replaced with {unfertilizedEggDef.label}");
        }
    }
}