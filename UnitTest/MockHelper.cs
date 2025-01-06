using Moq;
using ServeLog.InternalLogger;

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
