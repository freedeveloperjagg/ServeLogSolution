using Moq;
using ServeLog.InternalLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class MockHelper
    {
        public Mock<IDataInternal> MockDataInternal { get; set; }

        public MockHelper()
        {
            MockDataInternal = new Mock<IDataInternal>(MockBehavior.Strict);

        }
    }
}
