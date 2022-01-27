using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountTokenComponentSystemDestroy : DestroySystem<AccountTokenComponent>
    {
        public override void Destroy(AccountTokenComponent self)
        {
            self.tokenDic.Clear();
        }
    }


    public static class AccountTokenComponentSystem
    {
        public static void AddToken(this AccountTokenComponent self, long accountId, string token)
        {
            if (self.tokenDic.ContainsKey(accountId))
            {
                self.tokenDic[accountId] = token;
            }
            else
            {
                self.tokenDic.Add(accountId, token);
            }
            TimeoutRemoveToken(self,accountId).Coroutine();
        }

        public static void RemoveRoken(this AccountTokenComponent self, long accountId)
        {
            if (self.tokenDic.ContainsKey(accountId))
            {
                self.tokenDic.Remove(accountId);
            }
        }
        
        public static string GetToken(this AccountTokenComponent self ,long accountId)
        {
            string token = null; 
            self.tokenDic.TryGetValue(accountId,out token);
            return token;
        }

        public static async ETTask TimeoutRemoveToken(this AccountTokenComponent self,long accountId)
        {
            await TimerComponent.Instance.WaitAsync(36000);
            RemoveRoken(self,accountId);
        }
    }
}
