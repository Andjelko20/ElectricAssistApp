namespace Server.DTOs
{

    /// <summary>
    /// Data get from database using pagination ( only one page )
    /// </summary>
    public class DataPage<T> where T : class
    {
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<T> Data { get; set; }
    }
}
