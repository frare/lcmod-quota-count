using HarmonyLib;

namespace QuotaCount.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
internal static class TimeOfDayPatch
{
    [HarmonyPatch("UpdateProfitQuotaCurrentTime"), HarmonyPostfix]
    internal static void UpdateProfitQuotaCurrentTimePostfix(ref TimeOfDay __instance)
    {
        QuotaCountBase.LogMessage(
            $"Patching \"TimeOfDay UpdateProfitQuotaCurrentTime\"...",
            BepInEx.Logging.LogLevel.Debug
        );

        StartOfRound.Instance.profitQuotaMonitorText.text = 
            $"QUOTA {__instance.timesFulfilledQuota+1} PROFIT:\n${__instance.quotaFulfilled} / ${__instance.profitQuota}";

        if (__instance.timeUntilDeadline <= 0f)
        {
            StartOfRound.Instance.deadlineMonitorText.text = $"QUOTA {__instance.timesFulfilledQuota+1} DEADLINE:\nNOW";
        }
        else
        {
            StartOfRound.Instance.deadlineMonitorText.text = $"QUOTA {__instance.timesFulfilledQuota+1} DEADLINE:\n{__instance.daysUntilDeadline} Days";
        }

        QuotaCountBase.LogMessage("Done!", BepInEx.Logging.LogLevel.Debug);
    }
}