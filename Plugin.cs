using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace QuotaCount;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class QuotaCountBase : BaseUnityPlugin
{
    // plugin info
    internal const string PLUGIN_GUID = "frare.QuotaCount";
    internal const string PLUGIN_NAME = "Quota Count";
    internal const string PLUGIN_VERSION = "1.1.0";

    // singleton
    internal static QuotaCountBase Instance;

    // harmony instance
    private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

    // for debugging
    internal ManualLogSource logger;

    public static string CurrentQuotaString { get => $"QUOTA {TimeOfDay.Instance.quotaFulfilled + 1}"; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);
        logger.LogInfo($"Mod started! :)");

        harmony.PatchAll();

        logger.LogInfo($"Mod finished loading!");
    }

    public static void LogMessage(string message, LogLevel logLevel = LogLevel.Debug)
    {
        Instance.logger.Log(logLevel, message);
    }
}