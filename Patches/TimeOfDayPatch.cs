using HarmonyLib;

namespace QuotaCount.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
internal static class TimeOfDayPatch
{
    [HarmonyPatch("UpdateProfitQuotaCurrentTime"), HarmonyPostfix]
    internal static void UpdateProfitQuotaCurrentTimePostfix(ref TimeOfDay __instance)
    {
        QuotaCountBase.LogMessage(
            $"Patching \"TimeOfDay UpdateProfitQuotaCurrentTime\"..."
        );

        if (QuotaCountBase.DisplayInProfit)
        {
            var profitQuotaMonitor = StartOfRound.Instance.profitQuotaMonitorText;
            profitQuotaMonitor.text = $"{QuotaCountBase.CurrentQuotaString} PROFIT:\n${__instance.quotaFulfilled} / ${__instance.profitQuota}";
            QuotaCountBase.LogMessage("Updated profit quota monitor");
        }

        if (QuotaCountBase.DisplayInDeadline)
        {
            var deadlineMonitor = StartOfRound.Instance.deadlineMonitorText;
            deadlineMonitor.text = $"{QuotaCountBase.CurrentQuotaString} DEADLINE:\n{(__instance.daysUntilDeadline <= 0 ? "NOW" : __instance.daysUntilDeadline + " Days")}";
            QuotaCountBase.LogMessage("Updating deadline monitor");
        }

        QuotaCountBase.LogMessage("Done!");
    }
}