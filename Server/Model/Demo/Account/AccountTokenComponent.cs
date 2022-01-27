using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountTokenComponent :Entity,IAwake,IDestroy
    {
        /// <summary>
        /// <accoutnId,token>
        /// </summary>
        public Dictionary<long, string> tokenDic = new Dictionary<long, string>();
    }
}
