﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class DisconnectHelper 
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null||self.IsDisposed)
            {
                return;
            }
            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(5000);

            if (instanceId == self.InstanceId)
            {
                self.Dispose();
            }

        }
    }
}
