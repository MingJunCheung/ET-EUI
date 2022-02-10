namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        public const int ERR_AccountNull = 200002;//账号密码为空
        public const int ERR_AccountRegx = 200003;//账号密码格式不正确
        public const int ERR_AccountScene = 200004;//当前服务器不是账号服务器
        public const int ERR_AccountRequestRepeatly = 200005;//重复请求
        public const int ERR_AccountBlack = 200006; //黑名单
        public const int ERR_AccountPassword = 200007; //密码错误
        public const int ERR_Net = 200008; //网络错误
    }
}