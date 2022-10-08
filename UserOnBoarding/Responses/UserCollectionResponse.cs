namespace UserOnBoarding.Responses
{
    public class UserCollectionResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
