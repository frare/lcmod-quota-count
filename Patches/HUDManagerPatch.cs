using System;
using HarmonyLib;
using System.Linq;
using TMPro;

namespace QuotaCount.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal static class HUDManagerPatch
{
    private static TextMeshProUGUI tmp;
    private static string originalText;

    [HarmonyPatch("Awake"), HarmonyPostfix]
    internal static void AwakePostfix(ref HUDManager __instance)
    {
        QuotaCountBase.LogMessage(
            $"Patching \"HUDManager Awake\"..."
        );

        var textComponents = __instance.playersFiredAnimator.GetComponentsInChildren<TextMeshProUGUI>();
        tmp = textComponents.First(component => component.text.Contains("quota", StringComparison.OrdinalIgnoreCase));
        originalText = tmp.text;
        QuotaCountBase.LogMessage("Static TMP component reference and original text are ready");

        if (tmp == null)
        {
            QuotaCountBase.LogMessage($"Could not find the TMP component containing profit quota text", BepInEx.Logging.LogLevel.Error);
            return;
        }

        QuotaCountBase.LogMessage("Done!");
    }

    [HarmonyPatch("ShowPlayersFiredScreen"), HarmonyPostfix]
    internal static void ShowPlayersFiredScreenPostfix(ref HUDManager __instance, bool show)
    {
        if (show == false) return;

        QuotaCountBase.LogMessage(
            $"Patching \"HUDManager ShowPlayersFiredScreen\"..."
        );

        if (QuotaCountBase.DisplayInGameOver == false)
        {
            QuotaCountBase.LogMessage("NOT UPDATING GAME OVER SCREEN:Config is set to false");
            return;
        }

        if (tmp != null)
        {
            tmp.text = originalText + "\nTOTAL QUOTAS FULFILLED: " + TimeOfDay.Instance.timesFulfilledQuota;
            QuotaCountBase.LogMessage("Updated game over screen");
        }
        else
        {
            QuotaCountBase.LogMessage("Could not find the static TMP component reference", BepInEx.Logging.LogLevel.Error);
            return;
        }

        QuotaCountBase.LogMessage("Done!");
    }
}
