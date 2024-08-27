namespace WHITE_20.PLC
{
    public class OPCScope
    {
        public  OPCUAHelper OPC { get; }

        public OPCScope(string connectionString)
        {
            OPC = new();
            OPC.OpenConnectOfAnonymous(connectionString);
        }
    }
}
