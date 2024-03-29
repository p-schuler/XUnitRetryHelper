using Xunit;
using XunitRetryHelper.Lib;

namespace XunitRetryHelper.Tests
{
    public class XunitRetryHelperTests
    {
        private static int runsA;
        private static int runsB;

        [RetryTheory]
        [InlineData(1)]
        [InlineData(2)]
        public void AllPass_Theory(int n)
        {
            Assert.True(n > 0);
        }

        [RetryTheory]
        [InlineData(1)]
        [InlineData(2)]
        public void OneFail_Theory(int n)
        {
            Assert.True(n == 2 || runsA++ >= 5);
        }

        [RetryTheory]
        [InlineData(1)]
        [InlineData(2)]
        public void TwoFail_Theory(int n)
        {
            Assert.True(n == 2 || runsB++ >= 2);
        }
    }
}
