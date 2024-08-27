namespace WHITE_20.Repository.Interfaces
{
    public interface ICurrentErrorRepository
    {
        public List<CurrentError> GetAllHistoryErrors();
        public void AddHistoryError(CurrentError historyError);
        public void DeleteHistoryError(int id);
        public List<CurrentError> SearchErrors(string searchTerm);
    }
}
