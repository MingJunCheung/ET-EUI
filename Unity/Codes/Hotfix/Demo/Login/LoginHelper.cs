using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_AccountLogin a2CLogin = null;
            Session session = null;
            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));

                C2A_AccountLogin c2aLogin = new C2A_AccountLogin();
                c2aLogin.Account = account;
                c2aLogin.Password = MD5Helper.StringMD5(password); //密码进行加密

                a2CLogin = (A2C_AccountLogin)await session.Call(c2aLogin);
            }
            catch (Exception e)
            {
                session.Dispose();
                Log.Error(e);
                return ErrorCode.ERR_Net;
            }

            if (a2CLogin.Error != ErrorCode.ERR_Success)
            {
                session.Dispose();
                return a2CLogin.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = session;
            //zoneScene.AddComponent<AccounInfoComponent>();

            AccountInfoComponent infoCom = zoneScene.GetComponent<AccountInfoComponent>();
            infoCom.accountId = a2CLogin.AccountId;
            infoCom.token = a2CLogin.Token;

            return ErrorCode.ERR_Success;
        }
    }
}