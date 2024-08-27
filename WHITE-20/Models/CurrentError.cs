namespace WHITE_20.Models
{
    public class CurrentError
    {
        public int Id { get; set; } = 0;
        public string Date { get; set; } = "";
        public string Content { get; set; } = "";

        public CurrentError()
        {
            Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); // 设置Date为当前时间的特定格式
        }
    }
}
