namespace Common.ForwardMessage
{
    public class ForwardMessage
    {
        public string requestId;
        public string bodyClassType;
        public ForwardRequestInfo httpRequestInfo;
        public ForwardCallBackPath callBackPathInfo;

        public ForwardMessage(string requestId, string bodyClassType, ForwardRequestInfo httpRequestInfo, ForwardCallBackPath callBackPathInfo)
        {
            this.requestId = requestId;
            this.bodyClassType = bodyClassType;
            this.httpRequestInfo = httpRequestInfo;
            this.callBackPathInfo = callBackPathInfo;
        }
    }
}
