using MG.CamCtrl.Common.Enum;

namespace MG.CamCtrl.Common.Model
{
    public class CamConfig
    {
        public TriggerMode triggerMode { get; set; }

        public TriggerSource triggeSource { get; set; } 

        public TriggerPolarity triggerPolarity { get; set; }

        public ushort ExpouseTime { get; set; }

        public ushort TriggerFilter { get; set; }

        public ushort TriggerDelay { get; set; }

        public short Gain { get; set; }
    }
}


