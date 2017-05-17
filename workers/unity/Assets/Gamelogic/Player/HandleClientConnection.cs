using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class HandleClientConnection : MonoBehaviour
    {
        [Require]
        private HeartbeatCounter.Writer HeartbeatCounterWriter;

        [Require]
        private ClientConnection.Reader ClientConnectionReader;

        private Coroutine heartbeatCoroutine;

        private void OnEnable()
        {
            ClientConnectionReader.HeartbeatTriggered.Add(OnHeartbeat);
            heartbeatCoroutine = StartCoroutine(TimerUtils.CallRepeatedly(SimulationSettings.HeartbeatCheckIntervalSecs, CheckHeartbeat));
        }

        private void OnDisable()
        {
            ClientConnectionReader.HeartbeatTriggered.Remove(OnHeartbeat);
            StopCoroutine(heartbeatCoroutine);
        }

        private void OnHeartbeat(Heartbeat _)
        {
            SetHeartbeat(SimulationSettings.TotalHeartbeatsBeforeTimeout);
        }

        private void SetHeartbeat(uint timeoutBeatsRemaining)
        {
            HeartbeatCounterWriter.Send(new HeartbeatCounter.Update().SetTimeoutBeatsRemaining(timeoutBeatsRemaining));
        }

        private void CheckHeartbeat()
        {
            var heartbeatsRemainingBeforeTimeout = HeartbeatCounterWriter.Data.timeoutBeatsRemaining;
            if (heartbeatsRemainingBeforeTimeout == 0)
            {
                StopCoroutine(heartbeatCoroutine);
                DeletePlayerEntity();
                return;
            }
            SetHeartbeat(heartbeatsRemainingBeforeTimeout - 1);
        }

        private void DeletePlayerEntity()
        {
            SpatialOS.Commands.DeleteEntity(HeartbeatCounterWriter, gameObject.EntityId());
        }
    }
}