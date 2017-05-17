using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class SendClientConnection : MonoBehaviour
    {
        [Require]
        private ClientConnection.Writer ClientConnectionWriter;

        private Coroutine heartbeatCoroutine;
        private void OnEnable()
        {
            heartbeatCoroutine = StartCoroutine(TimerUtils.CallRepeatedly(SimulationSettings.HeartbeatSendingIntervalSecs, SendHeartbeat));
        }

        private void OnDisable()
        {
            StopCoroutine(heartbeatCoroutine);
        }

        private void SendHeartbeat()
        {
            ClientConnectionWriter.Send(new ClientConnection.Update().AddHeartbeat(new Heartbeat()));
        }
    }
}