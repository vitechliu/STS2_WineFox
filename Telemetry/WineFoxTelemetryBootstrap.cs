using STS2_WineFox.Character;
using STS2RitsuLib;
using STS2RitsuLib.Content;
using STS2RitsuLib.Telemetry;

namespace STS2_WineFox.Telemetry
{
    internal static class WineFoxTelemetryBootstrap
    {
        private const string ApplicantId = Const.ModId;
        private const string BackendEndpoint = "https://winefox-telemetry.ritsukage.com/v1/ingest";
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;
            RitsuLibFramework.RegisterTelemetryApplicant(new()
            {
                ApplicantId = ApplicantId,
                OwnerModId = Const.ModId,
                DisplayName = Const.Name,
                Adapter = new HttpJsonTelemetryAdapter(BackendEndpoint),
                Requests =
                [
                    TelemetryRequest.RunHistory(
                        "WineFox run history for balance and compatibility analysis.",
                        captureFilter: IsWineFoxRun),
                ],
            });
        }

        private static bool IsWineFoxRun(RunEndedEvent evt)
        {
            var wineFoxEntry = ModContentRegistry.GetFixedPublicEntry(Const.ModId, typeof(WineFox));
            return evt.Run.Players.Any(player =>
                string.Equals(player.CharacterId?.Entry, wineFoxEntry, StringComparison.OrdinalIgnoreCase));
        }
    }
}
