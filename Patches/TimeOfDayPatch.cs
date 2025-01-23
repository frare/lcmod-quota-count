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

        var profitQuotaMonitorText = StartOfRound.Instance.profitQuotaMonitorText;
        profitQuotaMonitorText.text = $"{QuotaCountBase.CurrentQuotaString} PROFIT:\n${__instance.quotaFulfilled} / ${__instance.profitQuota}";

        var deadlineMonitorText = StartOfRound.Instance.deadlineMonitorText;
        deadlineMonitorText.text = $"{QuotaCountBase.CurrentQuotaString + "\n"}" + deadlineMonitorText.text;

        QuotaCountBase.LogMessage("Done!");
    }
}