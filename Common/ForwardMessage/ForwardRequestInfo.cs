

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Common.ForwardMessage
{
    public class ForwardRequestInfo
    {
        public ForwardRequestInfo(string scheme, QueryString queryString, PathString pathString, string method, HostString hostString, Dictionary<string, string> httpRequestHeader, JObject body)
        {
            this.scheme = scheme;
            this.queryString = queryString;
            this.pathString = pathString;
            this.method = method;
            this.hostString = hostString;
            this.httpRequestHeader = httpRequestHeader;
            this.body = body;
        }

        public string scheme { get; set; }
        public QueryString queryString { get; set; }
        public PathString pathString { get; set; }
        public string method { get; set; }
        public HostString hostString{ get; set; }
        public Dictionary<string, string> httpRequestHeader { get; set; }
        public JObject? body { get; set; }
        public string? userId { get; set; }


    }
}
