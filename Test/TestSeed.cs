using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    public class TestSeed {
        public long Value { get; protected set; }

        public TestSeed() {
            Value = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }


    }
}
