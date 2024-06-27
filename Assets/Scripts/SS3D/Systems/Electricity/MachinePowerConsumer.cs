using FishNet.Object.Synchronizing;
using SS3D.Systems.Tile.Connections;
using UnityEngine;
namespace System.Electricity
{
    public class MachinePowerConsumer : BasicElectricDevice, IPowerConsumer
    {
        [SerializeField]
        private float _powerConsumptionIdle = 1f;
        [SerializeField]
        private float _powerConsumptionInUse = 1f;

        public bool isIdle = true;

        private bool _machineUsedOnce;

        [SyncVar(OnChange = nameof(SyncPowerStatus))]
        private PowerStatus _powerStatus;
        public float PowerNeeded 
        {
            get 
            {
                if (_machineUsedOnce)
                {
                    _machineUsedOnce = false;
                    return _powerConsumptionInUse;
                }
                
                return isIdle ? _powerConsumptionIdle : _powerConsumptionInUse;
            }
        }
        
        public event EventHandler<PowerStatus> OnPowerStatusUpdated;
        public PowerStatus PowerStatus { get => _powerStatus; set => _powerStatus = value; }

        private void SyncPowerStatus(PowerStatus oldValue, PowerStatus newValue, bool asServer)
        {
            OnPowerStatusUpdated?.Invoke(this, newValue);
        }

        public void UseMachineOnce()
        {
            _machineUsedOnce = true;
        }
    }
}