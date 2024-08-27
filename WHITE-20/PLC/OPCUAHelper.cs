using Opc.Ua.Client;
using Opc.Ua;
using OpcUaHelper;
using System.Security.Cryptography.X509Certificates;

namespace WHITE_20.PLC
{
    public class OPCUAHelper
    {
        private OpcUaClient opcUaClient;
        /// <summary>
        /// 构造函数
        /// </summary>
        public OPCUAHelper()
        {
            opcUaClient = new OpcUaClient();
        }

        /// <summary>
        /// 添加数据到字典中（相同键的则采用最后一个键对应的值）
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="key">键</param>
        /// <param name="dataValue">值</param>
        private void AddInfoToDic(Dictionary<string, DataValue> dic, string key, DataValue dataValue)
        {
            if (dic != null)
            {
                if (!dic.ContainsKey(key))
                {

                    dic.Add(key, dataValue);
                }
                else
                {
                    dic[key] = dataValue;
                }
            }

        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool ConnectStatus
        {
            get { return opcUaClient.Connected; }
        }

        /// <summary>
        /// 打开连接[匿名方式]
        /// </summary>
        /// <param name="serverUrl">服务器URL [格式: opc.tcp://服务器IP地址/服务名称]</param>
        public async void OpenConnectOfAnonymous(string serverUrl)
        {
            if (!string.IsNullOrEmpty(serverUrl))
            {
                try
                {
                    opcUaClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
                    await opcUaClient.ConnectServer(serverUrl);
                }
                catch (Exception ex)
                {
                    throw new Exception("连接失败: " + ex);
                }
            }
        }

        /// <summary>
        /// 打开连接[用户名密码方式]
        /// </summary>
        /// <param name="serverUrl">服务器</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        public async void OpenConnectOfAccount(string serverUrl, string userName, string userPwd)
        {
            if (!string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPwd))
            {
                try
                {
                    opcUaClient.UserIdentity = new UserIdentity(userName, userPwd);
                    await opcUaClient.ConnectServer(serverUrl);
                }
                catch (Exception ex)
                {
                    throw new Exception("连接失败: " + ex);
                }
            }
        }

        /// <summary>
        /// 打开连接【证书方式】
        /// </summary>
        /// <param name="serverUrl">服务器URL【格式：opc.tcp://服务器IP地址/服务名称】</param>
        /// <param name="certificatePath">证书路径</param>
        /// <param name="secreKey">密钥</param>
        public async void OpenConnectOfCertificate(string serverUrl, string certificatePath, string secreKey)
        {
            if (!string.IsNullOrEmpty(serverUrl) &&
                !string.IsNullOrEmpty(certificatePath) && !string.IsNullOrEmpty(secreKey))
            {
                try
                {
                    X509Certificate2 certificate = new X509Certificate2(certificatePath, secreKey, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                    opcUaClient.UserIdentity = new UserIdentity(certificate);

                    await opcUaClient.ConnectServer(serverUrl);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 连接失败: " + ex);
                }
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            if (opcUaClient != null)
            {
                try
                {
                    opcUaClient.Disconnect();
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 关闭连接失败: " + ex);
                }

            }
        }

        /// <summary>
        /// 获取到当前节点的值【同步读取】
        /// </summary>
        /// <typeparam name="T">节点对应的数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <returns>返回当前节点的值</returns>
        public T GetCurrentNodeValue<T>(string nodeId)
        {
            T value = default(T);
            if (!string.IsNullOrEmpty(nodeId) && ConnectStatus)
            {
                try
                {
                    value = opcUaClient.ReadNode<T>(nodeId);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 读取节点失败: " + ex);
                }
            }

            return value;
        }

        /// <summary>
        /// 获取到当前节点数据【同步读取】
        /// </summary>
        /// <typeparam name="DataValue">节点对应的数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <returns>返回当前节点的值</returns>
        public DataValue GetCurrentNodeValue(string nodeId)
        {
            DataValue dataValue = null;
            if (!string.IsNullOrEmpty(nodeId) && ConnectStatus)
            {
                try
                {
                    dataValue = opcUaClient.ReadNode(nodeId);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 读取节点失败: " + ex);
                }
            }
            return dataValue;
        }

        /// <summary>
        /// 获取到批量节点数据【同步读取】
        /// </summary>
        /// <param name="nodeIds">节点列表</param>
        /// <returns>返回节点数据字典</returns>
        public Dictionary<string, DataValue> GetBatchNodeDatasOfSync(List<NodeId> nodeIdList)
        {
            Dictionary<string, DataValue> dicNodeInfo = new Dictionary<string, DataValue>();
            if (nodeIdList != null && nodeIdList.Count > 0 && ConnectStatus)
            {
                try
                {
                    List<DataValue> dataValues = opcUaClient.ReadNodes(nodeIdList.ToArray());

                    int count = nodeIdList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        AddInfoToDic(dicNodeInfo, nodeIdList[i].ToString(), dataValues[i]);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 读取节点失败: " + ex);
                }
            }

            return dicNodeInfo;
        }

        // <summary>
        /// 获取到当前节点的值【异步读取】
        /// </summary>
        /// <typeparam name="T">节点对应的数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <returns>返回当前节点的值</returns>
        public async Task<T> GetCurrentNodeValueOfAsync<T>(string nodeId)
        {
            T value = default(T);
            if (!string.IsNullOrEmpty(nodeId) && ConnectStatus)
            {
                try
                {
                    value = await opcUaClient.ReadNodeAsync<T>(nodeId);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 读取节点失败: " + ex);
                }
            }
            return value;
        }

        /// <summary>
        /// 获取到批量节点数据【异步读取】
        /// </summary>
        /// <param name="nodeIds">节点列表</param>
        /// <returns>返回节点数据字典</returns>
        public async Task<Dictionary<string, DataValue>> GetBatchNodeDatasOfAsync(List<NodeId> nodeIdList)
        {
            Dictionary<string, DataValue> dicNodeInfo = new Dictionary<string, DataValue>();
            if (nodeIdList != null && nodeIdList.Count > 0 && ConnectStatus)
            {
                try
                {
                    List<DataValue> dataValues = await opcUaClient.ReadNodesAsync(nodeIdList.ToArray());

                    int count = nodeIdList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        AddInfoToDic(dicNodeInfo, nodeIdList[i].ToString(), dataValues[i]);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 读取节点失败: " + ex);
                }
            }

            return dicNodeInfo;
        }

        /// <summary>
        /// 获取到当前节点的关联节点
        /// </summary>
        /// <param name="nodeId">当前节点</param>
        /// <returns>返回当前节点的关联节点</returns>
        public ReferenceDescription[] GetAllRelationNodeOfNodeId(string nodeId)
        {
            ReferenceDescription[] referenceDescriptions = null;

            if (!string.IsNullOrEmpty(nodeId) && ConnectStatus)
            {
                try
                {
                    referenceDescriptions = opcUaClient.BrowseNodeReference(nodeId);
                }
                catch (Exception ex)
                {
                    string str = "PLC 获取当前： " + nodeId + "  节点的相关节点失败！！！ ";
                    throw new Exception(str + ex);
                }
            }

            return referenceDescriptions;
        }

        /// <summary>
        /// 获取到当前节点的所有属性
        /// </summary>
        /// <param name="nodeId">当前节点</param>
        /// <returns>返回当前节点对应的所有属性</returns>
        public OpcNodeAttribute[] GetCurrentNodeAttributes(string nodeId)
        {
            OpcNodeAttribute[] opcNodeAttributes = null;
            if (!string.IsNullOrEmpty(nodeId) && ConnectStatus)
            {
                try
                {
                    opcNodeAttributes = opcUaClient.ReadNoteAttributes(nodeId);
                }
                catch (Exception ex)
                {
                    string str = "PLC 读取节点: " + nodeId + "  的所有属性失败！！！";
                    throw new Exception(str + ex);
                }
            }
            return opcNodeAttributes;
        }

        /// <summary>
        /// 写入单个节点【同步方式】
        /// </summary>
        /// <typeparam name="T">写入节点值得数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <param name="value">节点对应的数据值(比如:(short)123）)</param>
        /// <returns>返回写入结果（true:表示写入成功）</returns>
        public bool WriteSingleNodeIdOfSync<T>(string nodeId, T value)
        {
            bool success = false;

            if (opcUaClient != null && ConnectStatus)
            {
                if (!string.IsNullOrEmpty(nodeId))
                {
                    try
                    {
                        success = opcUaClient.WriteNode(nodeId, value);
                    }
                    catch (Exception ex)
                    {
                        string str = "PLC 当前节点：" + nodeId + "  写入失败";
                        throw new Exception(str, ex);
                    }
                }

            }

            return success;
        }

        /// <summary>
        /// 批量写入节点
        /// </summary>
        /// <param name="nodeIdArray">节点数组</param>
        /// <param name="nodeIdValueArray">节点对应数据数组</param>
        /// <returns>返回写入结果（true:表示写入成功）</returns>
        public bool BatchWriteNodeIds(string[] nodeIdArray, object[] nodeIdValueArray)
        {
            bool success = false;
            if (nodeIdArray != null && nodeIdArray.Length > 0 &&
                nodeIdValueArray != null && nodeIdValueArray.Length > 0)

            {
                try
                {
                    success = opcUaClient.WriteNodes(nodeIdArray, nodeIdValueArray);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 批量写入节点失败！！！", ex);
                }
            }
            return success;
        }

        /// <summary>
        /// 写入单个节点【异步方式】
        /// </summary>
        /// <typeparam name="T">写入节点值得数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <param name="value">节点对应的数据值</param>
        /// <returns>返回写入结果（true:表示写入成功）</returns>
        public async Task<bool> WriteSingleNodeIdOfAsync<T>(string nodeId, T value)
        {
            bool success = false;

            if (opcUaClient != null && ConnectStatus)
            {
                if (!string.IsNullOrEmpty(nodeId))
                {
                    try
                    {
                        success = await opcUaClient.WriteNodeAsync(nodeId, value);
                    }
                    catch (Exception ex)
                    {
                        string str = "PLC 当前节点：" + nodeId + "  写入失败";
                        throw new Exception(str, ex);
                    }
                }

            }

            return success;
        }


        /// <summary>
        /// 读取单个节点的历史数据记录
        /// </summary>
        /// <typeparam name="T">节点的数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回该节点对应的历史数据记录</returns>
        public List<T> ReadSingleNodeIdHistoryDatas<T>(string nodeId, DateTime startTime, DateTime endTime)
        {
            List<T> nodeIdDatas = null;
            if (!string.IsNullOrEmpty(nodeId) && startTime != null && endTime != null && endTime > startTime)
            {
                try
                {
                    nodeIdDatas = opcUaClient.ReadHistoryRawDataValues<T>(nodeId, startTime, endTime).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC读取节点历史数据失败", ex);
                }
            }

            return nodeIdDatas;
        }

        /// <summary>
        /// 读取单个节点的历史数据记录
        /// </summary>
        /// <typeparam name="T">节点的数据类型</typeparam>
        /// <param name="nodeId">节点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回该节点对应的历史数据记录</returns>
        public List<DataValue> ReadSingleNodeIdHistoryDatas(string nodeId, DateTime startTime, DateTime endTime)
        {
            List<DataValue> nodeIdDatas = null;
            if (!string.IsNullOrEmpty(nodeId) && startTime != null && endTime != null && endTime > startTime)
            {
                if (ConnectStatus)
                {
                    try
                    {
                        nodeIdDatas = opcUaClient.ReadHistoryRawDataValues(nodeId, startTime, endTime).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("PLC读取节点历史数据失败", ex);
                    }
                }

            }
            return nodeIdDatas;
        }


        /// <summary>
        /// 单节点数据订阅
        /// </summary>
        /// <param name="key">订阅的关键字（必须唯一）</param>
        /// <param name="nodeId">节点</param>
        /// <param name="callback">数据订阅的回调方法</param>
        public void SingleNodeIdDatasSubscription(string key, string nodeId, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
        {
            if (ConnectStatus)
            {
                try
                {
                    opcUaClient.AddSubscription(key, nodeId, callback);
                }
                catch (Exception ex)
                {
                    string str = "PLC 订阅节点：" + nodeId + " 数据失败！！！";
                    throw new Exception(str, ex);
                }
            }
        }

        /// <summary>
        /// 取消单节点数据订阅
        /// </summary>
        /// <param name="key">订阅的关键字</param>
        public bool CancelSingleNodeIdDatasSubscription(string key)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(key))
            {
                if (ConnectStatus)
                {
                    try
                    {
                        opcUaClient.RemoveSubscription(key);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        string str = "PLC 取消 " + key + " 的订阅失败";
                        throw new Exception(str, ex);
                    }

                }
            }
            return success;
        }


        /// <summary>
        /// 批量节点数据订阅
        /// </summary>
        /// <param name="key">订阅的关键字（必须唯一）</param>
        /// <param name="nodeIds">节点数组</param>
        /// <param name="callback">数据订阅的回调方法</param>
        public void BatchNodeIdDatasSubscription(string key, string[] nodeIds, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
        {
            if (!string.IsNullOrEmpty(key) && nodeIds != null && nodeIds.Length > 0)
            {
                if (ConnectStatus)
                {
                    try
                    {
                        opcUaClient.AddSubscription(key, nodeIds, callback);
                    }
                    catch (Exception ex)
                    {
                        string str = "PLC 批量订阅节点数据失败！！！";
                        throw new Exception(str, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 取消所有节点的数据订阅
        /// </summary>
        /// <returns></returns>
        public bool CancelAllNodeIdDatasSubscription()
        {
            bool success = false;

            if (ConnectStatus)
            {
                try
                {
                    opcUaClient.RemoveAllSubscription();
                    success = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC 取消所有的节点数据订阅失败！！！", ex);
                }

            }
            return success;
        }



    }
}
