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
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                response.Error = ErrorCode.ERR_AccountScene;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (session.GetComponent<SessionLockComponent>() != null)
            {
                response.Error = ErrorCode.ERR_AccountRequestRepeatly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }
            //第二部 验证账号密码的合法性 
            //1）空值判断

            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_AccountNull;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //2）验证格式的合法性
            if (!Regex.IsMatch(request.Account.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountRegx;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.AccountLogin, request.Account.Trim().GetHashCode()))
                {
                    //第三部 验证服务器是不是有记录 没有记录要注册一个账号
                    AccountData accountData = null;
                    List<AccountData> accountList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).
                        Query<AccountData>(d => d.accountName.Equals(request.Account.Trim()));

                    if (accountList.Count > 0)
                    {
                        accountData = accountList[0];
                        session.AddChild(accountData);
                        //黑名单判断
                        if (accountData.accountType == (int)AccountType.Black)
                        {
                            response.Error = ErrorCode.ERR_AccountRegx;
                            reply();
                            accountData?.Dispose();
                            session.Disconnect().Coroutine();
                            return;
                        }
                        ///验证密码正确性
                        if (!accountData.passWord.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_AccountPassword;
                            reply();
                            accountData?.Dispose();
                            session.Disconnect().Coroutine();
                            return;
                        }
                    }
                    else
                    {
                        accountData = session.AddChild<AccountData>();
                        accountData.accountName = request.Account;
                        accountData.accountType = (int)AccountType.Normal;
                        accountData.creatTime = TimeHelper.ServerNow().ToString();
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<AccountData>(accountData);
                    }
                    //第四步 创建一条登录token
                    string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);
                    session.DomainScene().GetComponent<AccountTokenComponent>().RemoveRoken(accountData.Id);
                    session.DomainScene().GetComponent<AccountTokenComponent>().AddToken(accountData.Id, token);

                    response.AccountId = accountData.accountName;
                    response.Token = token;
                    reply();
                    accountData?.Dispose();
                }

            }
        }
    }
}
