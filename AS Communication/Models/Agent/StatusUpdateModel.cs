namespace AdministrationStation.Communication.Models.Agent
{
    public class StatusUpdateModel
    {
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public int CpuUsage { get; set; }
        public int MaximumRam { get; set; }
        public int CurrentRam { get; set; }
        public byte[] Screenshot { get; set; }
    }
}