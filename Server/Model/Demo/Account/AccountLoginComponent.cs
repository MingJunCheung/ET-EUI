using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountLoginComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<long,string> accountDic = new Dictionary<long,string>();
    }
}
