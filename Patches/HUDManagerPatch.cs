using System;
using HarmonyLib;
using System.Linq;
using TMPro;

namespace QuotaCount.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal static class HUDManagerPatch
{
    [HarmonyPatch("Start"), HarmonyPostfix]
    public static void StartPostfix(ref HUDManager __instance)
    {
        if (!QuotaCountBase.DisplayInGameOver) return;

        QuotaCountBase.LogMessage(
            $"Patching \"HUDManager Start\"..."
        );

        var textComponents = __instance.playersFiredAnimator.GetComponentsInChildren<TextMeshProUGUI>();
        var quotaText = textComponents.First(component => component.text.Contains("quota", StringComparison.OrdinalIgnoreCase));
        if (quotaText != null)
        {
            quotaText.text = quotaText.text + $"\n<b>TOTAL QUOTAS FULFILLED: {TimeOfDay.Instance.timesFulfilledQuota}</b>";
        }
        else
        {
            QuotaCountBase.LogMessage($"Could not find the TMP component containing profit quota text", BepInEx.Logging.LogLevel.Error);
        }

        QuotaCountBase.LogMessage("Done!");
    }
}
