namespace AdministrationStation.Communication.Models.Shared
{
    public class ServerInfo
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; } = 5500;
        public int ResponsePort { get; set; } = 5501;
    }
}