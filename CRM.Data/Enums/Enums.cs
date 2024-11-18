using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Enums
{
    public enum JobTaskStatus
    {
        Running = 1,
        Exception = 2,
        Completed = 3
    }
    public enum LogType
    {
        Info = 1,
        Fetal = 2, 
    }
}
