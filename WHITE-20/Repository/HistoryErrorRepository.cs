using MongoDB.Driver;

namespace WHITE_20.Repository
{
    public class HistoryErrorRepository : IHistoryErrorRepository
    {
        private List<HistoryError> _historyErrors =
        [
            new()
            {
                HistoryDate = "2024/01/01 10:00:00",
                RestoreDate = "2024/01/01 10:30:00",
                Content = "机器人报错",
            },
            new()
            {
                HistoryDate = "2024/01/01 17:30:00",
                RestoreDate = "2024/01/01 18:00:00",
                Content = "夹爪报错",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:00:00",
                RestoreDate = "2024/01/02 09:02:00",
                Content = "扫码枪通讯失败",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:01:33",
                RestoreDate = "2024/01/02 09:01:45",
                Content = "抓取失败",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:03:00",
                RestoreDate = "2024/01/02 09:04:00",
                Content = "扫码枪通讯失败",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:26:00",
                RestoreDate = "2024/01/02 09:26:30",
                Content = "抓取失败",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:27:00",
                RestoreDate = "2024/01/02 09:27:30",
                Content = "夹爪报错",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:28:00",
                RestoreDate = "2024/01/02 09:28:30",
                Content = "机器人报错",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:29:00",
                RestoreDate = "2024/01/02 09:29:30",
                Content = "夹爪报错",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:30:00",
                RestoreDate = "2024/01/02 09:30:30",
                Content = "移液X轴故障",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:31:00",
                RestoreDate = "2024/01/02 09:31:30",
                Content = "移液Y轴故障",
            },
            new()
            {
                HistoryDate = "2024/01/02 09:32:00",
                RestoreDate = "2024/01/02 09:32:30",
                Content = "移液Z轴故障",
            },
        ];

        public List<HistoryError> GetAllHistoryErrors()
        {
            return _historyErrors;
        }

        public List<HistoryError> SearchErrors(string searchTerm)
        {
            searchTerm = searchTerm.ToLower(); // 将搜索关键词转换为小写，便于不区分大小写地比较

            return _historyErrors.Where(e => e.Content.ToLower().Contains(searchTerm)).ToList();
        }
    }
}
