namespace WHITE_20.PLC
{
    public interface IPLCOperate
    {
        Task WriteControlState<T>(string nodeId, T data);
    }
}
