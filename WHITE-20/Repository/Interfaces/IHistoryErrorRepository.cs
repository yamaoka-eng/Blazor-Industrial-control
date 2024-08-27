namespace WHITE_20.Repository.Interfaces
{
    public interface IHistoryErrorRepository
    {
        List<HistoryError> GetAllHistoryErrors();
        List<HistoryError> SearchErrors(string searchTerm);
    }
}
