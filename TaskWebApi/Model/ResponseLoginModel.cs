namespace TaskWebApi.Model
{
    public class ResponseLoginModel
    {
        public string FullName { get; set; }
        public string UserId { get; set; }
        public Object Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
