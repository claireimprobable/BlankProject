using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Unity.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gamelogic.UI
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField] private Button ConnectButton;

        public void AttemptSpatialOsConnection()
        {
            DisableConnectButton();
            AttemptConnection();
        }

        private void DisableConnectButton()
        {
            ConnectButton.interactable = false;
        }

        private void AttemptConnection()
        {
            FindObjectOfType<Bootstrap>().ConnectToClient();
            StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClientConnectionTimeoutSecs,
                ConnectionTimeout));
        }

        private void ConnectionTimeout()
        {
            if (SpatialOS.IsConnected)
            {
                SpatialOS.Disconnect();
            }
            ConnectButton.interactable = true;
        }
    }
}
