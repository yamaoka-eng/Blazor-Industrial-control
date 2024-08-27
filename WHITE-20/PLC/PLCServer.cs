using Opc.Ua;
using Opc.Ua.Client;

namespace WHITE_20.PLC
{
    public class PLCServer : IHostedService, IPLCOperate
    {
        private readonly OPCUAHelper _OPC;

        public PLCServer(OPCScope _OPCScope)
        {
            _OPC = _OPCScope.OPC;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 进行PLCOPC订阅
            //_OPC.BatchNodeIdDatasSubscription(" ", callback, readPlcData);

            return Task.CompletedTask; // 返回一个已完成的Task
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _OPC.CancelAllNodeIdDatasSubscription();
            // 断开PLC连接
            _OPC.CloseConnect();
            return Task.CompletedTask; // 返回一个已完成的Task
        }

        // 写入PLC数据 - 设备操控
        public async Task WriteControlState<T>(string nodeId, T data)
        {
        }

        // 读取PLC数据
        private async void readPlcData(
            string key,
            MonitoredItem monitoredItem,
            MonitoredItemNotificationEventArgs args
        )
        {
            // 需要区分出来每个不同的节点信息
            MonitoredItemNotification notification =
                args.NotificationValue as MonitoredItemNotification;
            var stringValue = monitoredItem.StartNodeId.ToString();
            
        }
    }
}
