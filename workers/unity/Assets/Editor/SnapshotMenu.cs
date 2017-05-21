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
using Quaternion = Improbable.Core.Quaternion;

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

            const int area = 10;        

		    for (var i = 0; i < area; i++)
		    {
		        for (var j = 0; j < area; j++)
		        {
                    snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreateTreeTemplate(JitterTreePosition(area, i, j), JitterTreeRotation()));
                }
		    }
            SaveSnapshot(snapshotEntities);
		}

	    private static Coordinates JitterTreePosition(int area, int initialX, int initialZ)
	    {
	        var distanceStep = 1f / SimulationSettings.TreeDensity;
	        var width = area * distanceStep;
	        var halfWidth = width * 0.5f;

	        var jitterX = Random.Range(-0.3f, 0.3f) * distanceStep;
	        var jitterY = Random.Range(-0.3f, 0.3f) * distanceStep;

	        var xCoord = (initialX * distanceStep) - halfWidth + jitterX;
	        var zCoord = (initialZ * distanceStep) - halfWidth + jitterY;

            var coords = new Coordinates(xCoord, 0, zCoord);

	        return coords;
	    }

	    private static Quaternion JitterTreeRotation()
	    {
            //Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            //var rotation = new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f);

            // NOT first, THIRD
	        var rotation = new Quaternion(0f, Random.Range(0f, 360f), 0f, Random.Range(0f, 360f));
            return rotation;
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