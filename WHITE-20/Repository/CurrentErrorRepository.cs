namespace WHITE_20.Repository
{
    public class CurrentErrorRepository : ICurrentErrorRepository
    {
        private List<CurrentError> _currentErrors = [
            new() { Id = 0, Date = "2024/01/01 10:00:00", Content = "机器人报错" },
            new() { Id = 1, Date = "2024/01/01 17:30:00", Content = "夹爪报错" },
            new() { Id = 2, Date = "2024/01/02 09:00:00", Content = "扫码枪通讯失败" },
            new() { Id = 3, Date = "2024/01/02 09:01:33", Content = "抓取失败" },
            new() { Id = 4, Date = "2024/01/02 09:03:00", Content = "扫码枪通讯失败" },
            new() { Id = 5, Date = "2024/01/02 09:26:00", Content = "抓取失败" },
            ];

        public void AddHistoryError(CurrentError historyError)
        {
            _currentErrors.Add(historyError);
        }

        public void DeleteHistoryError(int id)
        {
            CurrentError currentError = _currentErrors.FirstOrDefault(e => e.Id == id);
            _currentErrors.Remove(currentError);
        }

        public List<CurrentError> GetAllHistoryErrors()
        {
            return _currentErrors;
        }

        public List<CurrentError> SearchErrors(string searchTerm)
        {
            searchTerm = searchTerm.ToLower(); // 将搜索关键词转换为小写，便于不区分大小写地比较

            return _currentErrors.Where(e => e.Content.ToLower().Contains(searchTerm)).ToList();
        }
    }
}
