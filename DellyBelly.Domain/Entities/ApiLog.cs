using System;

namespace DellyBelly.Domain.Entities
{
    public class ApiLog
    {
        public long Id { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public string? Duration { get; set; }  // Human-readable (e.g., "22 ms", "1.2 s")
        public string Method { get; set; }
        public string Path { get; set; }
        public string? QueryString { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public string? IpAddress { get; set; }
    }
}
