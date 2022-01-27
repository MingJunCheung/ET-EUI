using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountData :Entity,IAwake
    {
        public string accountName;
        public string passWord;
        public int accountType;
    }


    public enum AccountType
    {
        /// <summary>
        /// 正常账号
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 机器人账号
        /// </summary>
        Robot = 1,
        /// <summary>
        /// 黑名单账号
        /// </summary>
        Black = 2,
    }
}
