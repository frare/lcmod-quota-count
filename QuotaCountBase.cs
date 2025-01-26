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
    internal const string PLUGIN_VERSION = "1.1.5";

    // singleton
    internal static QuotaCountBase Instance;

    // harmony instance
    private readonly Harmony harmony = new(PLUGIN_GUID);

    // for debugging
    internal ManualLogSource logger;

    // cfg file options
    public static bool DisplayInProfit { get; private set; }
    public static bool DisplayInDeadline { get; private set; }
    public static bool DisplayInGameOver { get; private set; }

    public static string CurrentQuotaString { get => $"QUOTA {TimeOfDay.Instance.timesFulfilledQuota + 1}"; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);
        logger.LogInfo($"Mod started! :)");

        LoadFromConfig();

        harmony.PatchAll();

        logger.LogInfo($"Mod finished loading!");
    }

    private void LoadFromConfig()
    {
        DisplayInProfit = Config.Bind("General", "DisplayInProfitQuotaMonitor", true, "Display count in PROFIT QUOTA monitor").Value;
        DisplayInDeadline = Config.Bind("General", "DisplayInDeadlineMonitor", true, "Display count in DEADLINE monitor").Value;
        DisplayInGameOver = Config.Bind("General", "DisplayInGameOverScreen", true, "Display total quotas fulfilled in the YOU ARE FIRED screen (game over screen)").Value;
    }

    public static void LogMessage(string message, LogLevel logLevel = LogLevel.Debug)
    {
        Instance.logger.Log(logLevel, message);
    }
}