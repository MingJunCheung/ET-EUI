using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ET.Demo.Account
{
    public class AccountLoginHandler : AMRpcHandler<C2A_AccountLogin, A2C_AccountLogin>
    {
        protected override async ETTask Run(Session session, C2A_AccountLogin request, A2C_AccountLogin response, Action reply)
        {
            //第一步 验证是否是账号服务器
            if(session.DomainScene().SceneType != SceneType.Account)
            {
                response.Error = ErrorCode.ERR_AccountScene;
                reply();
                session.Dispose();
                return;
            }
            //第二部 验证账号密码的合法性 
            //1）空值判断

            if (string.IsNullOrEmpty(request.Account)|| string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_AccountNull;
                reply();
                session.Dispose();
                return;
            }

            //2）验证格式的合法性
            if (!Regex.IsMatch(request.Account.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountRegx;
                reply();
                session.Dispose();
                return;
            }

            //第三部 验证服务器是不是有记录 没有记录要注册一个账号
            AccountData accountData = null;
            List<AccountData> accountList =  await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).
                Query<AccountData>(d=>d.accountName.Equals (request.Account.Trim()));

            if (accountList .Count > 0)
            {
                accountData = accountList[0];
            }
            else
            {
                accountData = session.AddChild<AccountData>();
                accountData .accountName = request.Account;
            }

            await ETTask.CompletedTask;
        }
    }
}
