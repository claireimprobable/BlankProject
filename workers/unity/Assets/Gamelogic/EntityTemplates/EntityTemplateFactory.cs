using Assets.Gamelogic.Core;
using Improbable.Core;
using Improbable.Math;
using Improbable.Player;
using Improbable.Tree;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;
using UnityEngine;
using Quaternion = Improbable.Core.Quaternion;

namespace Assets.Gamelogic.EntityTemplates
{
    public class EntityTemplateFactory : MonoBehaviour
    {
        // Creates a template for the PlayerCreator entity
        public static SnapshotEntity CreatePlayerCreatorTemplate()
        {
            var playerCreatorEntityTemplate = new SnapshotEntity { Prefab = SimulationSettings.PlayerCreatorPrefabName };

            playerCreatorEntityTemplate.Add(new WorldTransform.Data(Coordinates.ZERO, new Quaternion(0, 0, 0, 0)));
            playerCreatorEntityTemplate.Add(new PlayerCreation.Data());

            var acl = Acl.GenerateServerAuthoritativeAcl(playerCreatorEntityTemplate);
            playerCreatorEntityTemplate.SetAcl(acl);

            return playerCreatorEntityTemplate;
        }

        // Creates a template for the Player entity
        public static Entity CreatePlayerTemplate(string clientId)
        {
            var playerTemplate = new SnapshotEntity { Prefab = SimulationSettings.PlayerPrefabName };

            playerTemplate.Add(new WorldTransform.Data(new Coordinates(0, 4, 0), new Quaternion(0, 0, 0, 0)));
            playerTemplate.Add(new ClientConnection.Data());
            playerTemplate.Add(new HeartbeatCounter.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout));

            var acl = Acl.Build()
                .SetReadAccess(CommonRequirementSets.PhysicsOrVisual)
                .SetWriteAccess<WorldTransform>(CommonRequirementSets.SpecificClientOnly(clientId))
                .SetWriteAccess<ClientConnection>(CommonRequirementSets.SpecificClientOnly(clientId))
                .SetWriteAccess<HeartbeatCounter>(CommonRequirementSets.PhysicsOnly);
            playerTemplate.SetAcl(acl);

            return playerTemplate;
        }

        public static SnapshotEntity CreateCubeTemplate()
        {
            var cubeTemplate = new SnapshotEntity { Prefab = SimulationSettings.CubePrefabName };

            cubeTemplate.Add(new WorldTransform.Data(new Coordinates(0, 0, 5), new Quaternion(0, 0, 0, 0)));

            var acl = Acl.Build()
                .SetReadAccess(CommonRequirementSets.PhysicsOrVisual)
                .SetWriteAccess<WorldTransform>(CommonRequirementSets.PhysicsOnly);
            cubeTemplate.SetAcl(acl);

            return cubeTemplate;
        }

        public static SnapshotEntity CreateTreeTemplate(Coordinates coordinates, Quaternion rotation)
        {
            var treeTemplate = new SnapshotEntity { Prefab = SimulationSettings.TreePrefabName };

            treeTemplate.Add(new WorldTransform.Data(coordinates, rotation));
            treeTemplate.Add(new TreeState.Data(TreeStatus.HEALTHY));

            var acl = Acl.Build()
                .SetReadAccess(CommonRequirementSets.PhysicsOrVisual)
                .SetWriteAccess<WorldTransform>(CommonRequirementSets.PhysicsOnly)
                .SetWriteAccess<TreeState>(CommonRequirementSets.PhysicsOnly);
            treeTemplate.SetAcl(acl);

            return treeTemplate;
        }
    }
}