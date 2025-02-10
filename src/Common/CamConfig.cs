namespace MG.CamCtrl
{
    public class CamConfig
    {
        public TriggerMode triggerMode { get; set; }

        public TriggerSource triggeSource { get; set; }

        public TriggerPolarity triggerPolarity { get; set; }

        public ulong ExpouseTime { get; set; }

        public ushort TriggerFilter { get; set; }

        public ushort TriggerDelay { get; set; }

        public float Gain { get; set; }
    }
}


