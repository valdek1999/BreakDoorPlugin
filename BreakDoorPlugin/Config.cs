using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakDoorPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float PercentDoor { get; set; } = 1f;
        public int[] RoleException { get; set; } = { 0 };
    }
}
