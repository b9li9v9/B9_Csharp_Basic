

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Common.ForwardMessage
{
    public class ForwardRequestInfo
    {
        public ForwardRequestInfo(PathString url, string method, Dictionary<string, string> headers, JObject body)
        {
            this.url = url;
            this.method = method;
            this.headers = headers;
            this.body = body;
        }

        public PathString url { get; set; }
        public string method { get; set; }
        public Dictionary<string, string> headers { get; set; }
        public JObject body { get; set; }
    }
}
