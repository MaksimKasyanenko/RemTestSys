using System;
using RemTestSys.Domain.Models;
using System.Text.Json;
using Xunit;

namespace UnitTests
{
    public class OneVariantAnswerTests
    {
        public class GetAdditiveDataTests
        {
            [Fact]
            public void ReturnsStringArrayWith4Variants_WhenSpecified3FakeVariants()
            {
                var answer = new OneOfFourVariantsAnswer
                {
                    RightText = "rightVariant",
                    SerializedFakes = JsonSerializer.Serialize(new string[] { "fake1", "fake2", "fake3" })
                };

                var res = answer.GetAddition();

                Assert.IsType<string[]>(res);
                Assert.Equal(4, res.Length);
            }
            [Fact]
            public void ResultContainsAllOfFakesAndRightText()
            {
                var answer = new OneOfFourVariantsAnswer
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
        }
        public class SettingAdditiveDataTests
        {
            [Theory]
            [InlineData("fake1",null,null)]
            [InlineData("fake1","fake2",null)]
            public void SetFakesMethodThrowsException_WhenPassedTheFakesLessThen3(params string[] fakes)
            {
                var answer = new OneOfFourVariantsAnswer();

                Assert.Throws<InvalidOperationException>(()=>answer.SetFakes(fakes[0],fakes[1],fakes[2]));
            }

            [Theory]
            [InlineData("fake1",null,null)]
            [InlineData("fake1", "fake2")]
            [InlineData("fake1", "fake2","fake3","fake4")]
            public void SerializedDataPropertyThrowsException_WhenPassedTheFakesLessOrMoreThen3(params string[] fakes)
            {
                var answer = new OneOfFourVariantsAnswer();

                Assert.Throws<InvalidOperationException>(() => answer.SerializedFakes = JsonSerializer.Serialize(fakes));
            }

            [Fact]
            public void SetsSerializedFakes()
            {
                var answer = new OneOfFourVariantsAnswer();

                answer.SetFakes("fake1", "fake2", "fake3");

                Assert.NotNull(answer.SerializedFakes);
                Assert.Equal("[\"fake1\",\"fake2\",\"fake3\"]", answer.SerializedFakes);
            }
        }

        public class IsMatchMethodTests
        {
            [Fact]
            public void ReturnsTrue_WhenPassedRightText()
            {
                var answer = new OneOfFourVariantsAnswer { RightText = "rightText" };
                answer.SetFakes("fake1", "fake2", "fake3");

                var res = answer.IsMatch(new string[] { "rightText" });

                Assert.True(res);
            }

            [Fact]
            public void ReturnsFalse_WhenPassedWrongText()
            {
                var answer = new OneOfFourVariantsAnswer { RightText = "rightText" };
                answer.SetFakes("fake1", "fake2", "fake3");

                var res = answer.IsMatch(new string[] { "fake2" });

                Assert.False(res);
            }
        }
    }
}
