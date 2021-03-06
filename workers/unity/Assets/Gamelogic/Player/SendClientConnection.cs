﻿using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class SendClientConnection : MonoBehaviour
    {
        [Require]
        public ClientConnection.Writer ClientConnectionWriter;

        private Coroutine heartbeatCoroutine;
        private void OnEnable()
        {
            SceneManager.UnloadSceneAsync(BuildSettings.SplashScreenScene);
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