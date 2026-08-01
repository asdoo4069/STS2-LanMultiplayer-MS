using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace LanMultiplayerMS.Patchs
{
    [HarmonyPatch(typeof(CombatStateSynchronizer), "WaitForSync")]
    internal class CombatStateSynchronizerWaitForSyncPatch
    {
        private static bool Prefix(CombatStateSynchronizer __instance, Logger ____logger,
            INetGameService ____netService, TaskCompletionSource? ____syncCompletionSource,
            Dictionary<ulong, SerializablePlayer> ____syncData, RunState ____runState, RunLobby? ____runLobby,
            SerializableRunRngSet? ____rngSet, SerializableRelicGrabBag? ____sharedRelicGrabBag, ref Task __result)
        {
            //Whether is LAN game was not checked, because the sync issue may also occur when connect via Steam

            __result = TaskHelper.RunSafely(WaitForSync(__instance, ____logger, ____netService,
                ____syncCompletionSource, ____syncData, ____runState, ____runLobby, ____rngSet,
                ____sharedRelicGrabBag));

            return false;
        }

        private static async Task WaitForSync(CombatStateSynchronizer instance, Logger logger,
            INetGameService netService, TaskCompletionSource? syncCompletionSource,
            Dictionary<ulong, SerializablePlayer> syncData, RunState runState, RunLobby? runLobby,
            SerializableRunRngSet? rngSet, SerializableRelicGrabBag? sharedRelicGrabBag)
        {
            logger.Debug("[LanMultiplayer-MS] Waiting to receive all sync messages from all clients");
            if (netService.Type == NetGameType.Singleplayer || instance.IsDisabled)
                return;

            if (syncCompletionSource == null)
            {
                throw new InvalidOperationException("[LanMultiplayer-MS] StartSync must be called before WaitForSync!");
            }

            var startTime = DateTime.Now;

            const int timeoutSeconds = 30;

            while (!syncCompletionSource.Task.IsCompleted)
            {
                if ((DateTime.Now - startTime).TotalSeconds > timeoutSeconds)
                {
                    logger.Warn("[LanMultiplayer-MS] Receive all sync messages timeout, skip waiting for all clients");
                    break;
                }

                await Task.Delay(100);
            }

            // Prefix 캡처 시점의 스냅샷은 대기 중 갱신되지 않으므로, 여기서 인스턴스 필드를 다시 읽는다.
            var traverse = Traverse.Create(instance);
            rngSet = traverse.Field("_rngSet").GetValue<SerializableRunRngSet?>();
            sharedRelicGrabBag = traverse.Field("_sharedRelicGrabBag").GetValue<SerializableRelicGrabBag?>();

            foreach (var syncDatum in syncData)
            {
                if (runLobby != null && !runLobby.PlayerIds.Contains(syncDatum.Key))
                {
                    logger.Debug($"[LanMultiplayer-MS] Skipping sync for disconnected player {syncDatum.Key}");
                    continue;
                }

                var player = runState.GetPlayer(syncDatum.Key);
                if (!LocalContext.IsMe(player))
                {
                    player?.SyncWithSerializedPlayer(syncDatum.Value);
                }
            }

            if (netService.Type != NetGameType.Host)
            {
                if (rngSet != null)
                {
                    runState.Rng.LoadFromSerializable(rngSet);
                }
                else if (runState.Players.Count > 1)
                {
                    logger.Error("[LanMultiplayer-MS] There are two or more players and we are a client, but we never received the RNG set!");
                }

                if (sharedRelicGrabBag != null)
                {
                    runState.SharedRelicGrabBag.LoadFromSerializable(sharedRelicGrabBag);
                }
                else if (runState.Players.Count > 1)
                {
                    logger.Error("[LanMultiplayer-MS] There are two or more players and we are a client, but we never received the shared relic grab bag!");
                }
            }

            syncData.Clear();
            traverse.Field("_rngSet").SetValue(null);
            traverse.Field("_sharedRelicGrabBag").SetValue(null);
            traverse.Field("_syncCompletionSource").SetValue(null);
        }
    }
}