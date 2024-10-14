namespace Common.ForwardMessage
{
    public class ForwardMessage
    {
        public string requestId;
        public ForwardRequestInfo httpRequestInfo;
        public ForwardCallBackPath callBackPathInfo;

        public ForwardMessage(string requestId, ForwardRequestInfo httpRequestInfo, ForwardCallBackPath callBackPathInfo)
        {
            this.requestId = requestId;
            this.httpRequestInfo = httpRequestInfo;
            this.callBackPathInfo = callBackPathInfo;
        }
    }
}
