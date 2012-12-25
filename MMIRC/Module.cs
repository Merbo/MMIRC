using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIRC
{
    public abstract class Module
    {
        public const int MODULE_OKAY = 0;
        public const int MODULE_FAIL = 1;
        public const int MODULE_FATAL = 2;

        public abstract int Init();
        public abstract int OnTick(int Tick);

        public abstract string Name();
        public abstract string Author();
        public abstract string Description();
        public abstract string Version();

        public Module()
        {
        }
    }
}
