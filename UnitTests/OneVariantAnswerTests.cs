using RemTestSys.Domain.Models;
using System.Text.Json;
using Xunit;

namespace UnitTests
{
    public class GettingAndSettingAdditiveDataOfOneVariantAnswerTests
    {
        [Fact]
        public void ReturnsStringArrayWith4Variants_WhenSpecified3FakeVariants()
        {
            var answer = new OneVariantAnswer {
                RightText="rightVariant",
                SerializedFakes = JsonSerializer.Serialize(new string[] {"fake1","fake2","fake3"})
            };

            var res = answer.GetAddition();

            Assert.IsType<string[]>(res);
            Assert.Equal(4, res.Length);
        }

        [Fact]
        public void ResultContainsAllOfFakesAndRightText()
        {
            var answer = new OneVariantAnswer
            {
                RightText = "rightVariant",
                SerializedFakes = JsonSerializer.Serialize(new string[] { "fake1", "fake2", "fake3" })
            };

            var res = answer.GetAddition();

            Assert.Contains("rightVariant", res);
            Assert.Contains("fake1", res);
            Assert.Contains("fake2", res);
            Assert.Contains("fake3", res);
        }

        [Fact]
        public void SetsSerializedFakes()
        {
            var answer = new OneVariantAnswer();

            answer.SetFakes("fake1","fake2","fake3");

            Assert.NotNull(answer.SerializedFakes);
            Assert.Equal("[\"fake1\",\"fake2\",\"fake3\"]", answer.SerializedFakes);
        }
    }

    public class MatchingOneVariantAnswerTests
    {
        [Fact]
        public void ReturnsTrue_WhenPassedRightText()
        {
            var answer = new OneVariantAnswer {RightText="rightText"};
            answer.SetFakes("fake1", "fake2", "fake3");

            var res = answer.IsMatch(new string[] { "rightText" });

            Assert.True(res);
        }

        [Fact]
        public void ReturnsFalse_WhenPassedWrongText()
        {
            var answer = new OneVariantAnswer { RightText = "rightText" };
            answer.SetFakes(new string[] { "fake1", "fake2", "fake3" });

            var res = answer.IsMatch(new string[] { "fake2" });

            Assert.False(res);
        }
    }
}
