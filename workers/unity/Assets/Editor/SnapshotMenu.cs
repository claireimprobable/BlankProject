using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Worker;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using Assets.Gamelogic.EntityTemplates;
using Improbable.Math;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor 
{
	public class SnapshotMenu : MonoBehaviour
	{
		[MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
		[UsedImplicitly]
		private static void GenerateDefaultSnapshot()
		{
			var snapshotEntities = new Dictionary<EntityId, SnapshotEntity>();

		    var currentEntityId = 1;

		    snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreatePlayerCreatorTemplate());
		    snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateCubeTemplate());

            //new Coordinates(0, 0, 5),

		    var rows = 10;
		    var columns = 10;
		    for (int i = 0; i < rows; i++)
		    {
		        for (int j = 0; j < columns; j++)
		        {
		            snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateTreeTemplate(new Coordinates(5*i, 0, 5*j)));
                }
		    }

            SaveSnapshot(snapshotEntities);
		}

		private static void SaveSnapshot(IDictionary<EntityId, SnapshotEntity> snapshotEntities)
		{
			File.Delete(SimulationSettings.DefaultSnapshotPath);
			var maybeError = Snapshot.Save(SimulationSettings.DefaultSnapshotPath, snapshotEntities);

			if (maybeError.HasValue)
			{
				Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", maybeError.Value);
			}
			else
			{
				Debug.LogFormat("Successfully generated initial world snapshot at {0}", SimulationSettings.DefaultSnapshotPath);
			}
		}
	}
}
