﻿using Assets.Gamelogic.EntityTemplates;
using Improbable;
using Improbable.Core;
using Improbable.Entity.Component;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class PlayerCreatingBehaviour : MonoBehaviour
    {
        [Require]
        private PlayerCreation.Writer PlayerCreationWriter;

        void OnEnable()
        {
            PlayerCreationWriter.CommandReceiver.OnCreatePlayer.RegisterResponse(OnCreatePlayer);
        }

        void OnDisable()
        {
            PlayerCreationWriter.CommandReceiver.OnCreatePlayer.DeregisterResponse();
        }

        private CreatePlayerResponse OnCreatePlayer(CreatePlayerRequest request, ICommandCallerInfo callerinfo)
        {
            CreatePlayerWithReservedId(callerinfo.CallerWorkerId);
            return new CreatePlayerResponse();
        }

        private void CreatePlayerWithReservedId(string clientWorkerId)
        {
            SpatialOS.Commands.ReserveEntityId(PlayerCreationWriter)
                              .OnSuccess(reservedEntityId => CreatePlayer(clientWorkerId, reservedEntityId))
                              .OnFailure(failure => OnFailedReservation(failure, clientWorkerId));
        }

        private void OnFailedReservation(ICommandErrorDetails response, string clientWorkerId)
        {
            Debug.LogError("Failed to Reserve EntityId for Player: " + response.ErrorMessage + ". Retrying...");
            CreatePlayerWithReservedId(clientWorkerId);
        }

        private void CreatePlayer(string clientWorkerId, EntityId entityId)
        {
            var playerEntityTemplate = EntityTemplateFactory.CreatePlayerTemplate(clientWorkerId);
            SpatialOS.Commands.CreateEntity(PlayerCreationWriter, entityId, SimulationSettings.PlayerPrefabName, playerEntityTemplate)
                              .OnFailure(failure => OnFailedPlayerCreation(failure, clientWorkerId, entityId));
        }

        private void OnFailedPlayerCreation(ICommandErrorDetails response, string clientWorkerId, EntityId entityId)
        {
            Debug.LogError("Failed to Create Player Entity: " + response.ErrorMessage + ". Retrying...");
            CreatePlayer(clientWorkerId, entityId);
        }
    }
}