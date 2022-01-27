using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountComponentSystemDestroy : DestroySystem<AccountLoginComponent>
    {
        public override void Destroy(AccountLoginComponent self)
        {
            self.accountDic.Clear();
        }
    }

    public class AccountLoginComponentAwakeSystem : AwakeSystem<AccountLoginComponent>
    {
        public override void Awake(AccountLoginComponent self)
        {
            
        }
    }

    public static class  AccountComponentSystem 
    {
        public static void AddOneAccount( this AccountLoginComponent self,long accountId,string account)
        {

        }

        public static void GetOneAccount()
        {

        }

    }
}
