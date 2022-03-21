using RemTestSys.Domain.Models;
using System;
using System.Text.Json;
using Xunit;

namespace UnitTests.AnswerModelTests
{
    public class SomeVariantsAnswerTests
    {
        public class GettingAdditiveDataTests
        {
            [Theory]
            [InlineData("[\"right1\",\"right2\",\"right3\"]", "[\"fake1\",\"fake2\",\"fake3\"]")]
            [InlineData("[\"right1\"]", "[\"fake1\",\"fake2\"]")]
            public void ReturnsCountOfStringsWhichEqualsSumOfRightAnswersAndFakes(string rightSet, string fakes)
            {
                var answer = new SomeVariantsAnswer {
                    SerializedRightAnswers = rightSet,
                    SerializedFakes = fakes
                };

                var res = answer.GetAdditiveData();
                int sum = JsonSerializer.Deserialize<string[]>(rightSet).Length + JsonSerializer.Deserialize<string[]>(fakes).Length;

                Assert.Equal(sum, res.Length);
            }

            [Fact]
            public void ResultContainsAllFakesAndRightAnswers()
            {
                string[] fakes = {"fake1","fake2"};
                string[] rightAnswers = {"right1","right2"};
                var answer = new SomeVariantsAnswer();
                answer.SetFakes(fakes);
                answer.SetRightAnswers(rightAnswers);

                var res = answer.GetAdditiveData();

                for(int i = 0; i < fakes.Length; i++)
                {
                    Assert.Contains(fakes[i], res);
                }
                for (int i = 0; i < rightAnswers.Length; i++)
                {
                    Assert.Contains(rightAnswers[i], res);
                }
            }
        }

        public class SettingAdditiveDataTests
        {
            [Theory]
            [InlineData("[null,\"gh\"]", new string[] {"data",null})]
            [InlineData(null, null)]
            public void ThrowsExceptions_WhenPassedNull(string serializedData, string[] arr)
            {
                var answer = new SomeVariantsAnswer();

                Assert.Throws<InvalidOperationException>(()=>answer.SetRightAnswers(arr));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedRightAnswers=serializedData);
            }

            [Fact]
            public void SetsSerializedData()
            {
                var answer = new SomeVariantsAnswer();

                answer.SetRightAnswers("1","2");
                answer.SetFakes("3","4");

                Assert.Equal("[\"1\",\"2\"]", answer.SerializedRightAnswers);
                Assert.Equal("[\"3\",\"4\"]", answer.SerializedFakes);
            }
        }

        public class MatchingTests
        {
            [Theory]
            [InlineData(new string[] {"1","2"}, new string[] {"3","4"}, new string[] {"1","3"})]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] { "1", "2", "3" })]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] { "4", "3" })]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] { "1" })]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] {  })]
            public void ReturnsFalse_WhenAnswerIsWrong(string[] rightAnswers, string[] fakes, string[] answerData)
            {
                var answer = new SomeVariantsAnswer();
                answer.SetRightAnswers(rightAnswers);
                answer.SetFakes(fakes);

                var res = answer.IsMatch(answerData);

                Assert.False(res);
            }

            [Theory]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] {"1","2"})]
            [InlineData(new string[] { "1", "2" }, new string[] { "3", "4" }, new string[] { "2", "1" })]
            public void ReturnsTrue_WhenAnswerIsRight(string[] rightAnswers, string[] fakes, string[] answerData)
            {
                var answer = new SomeVariantsAnswer();
                answer.SetRightAnswers(rightAnswers);
                answer.SetFakes(fakes);

                var res = answer.IsMatch(answerData);

                Assert.True(res);
            }
        }
        public class GettingRightText
        {
            [Theory]
            [InlineData(new string[] { "rightText1", "rightText2" }, new string[] { "fake1", "fake2" })]
            [InlineData(new string[] { "rightText1", "rightText2","rightText3" }, new string[] { "fake1", "fake2","fake3" })]
            public void RightTextPropertyGetsStringContainsAllRightAnswersAndNoFakes_WhenDataSetedOverSettersAndProperty(string[] rightAnswers, string[] fakes)
            {
                var answer1 = new SomeVariantsAnswer();
                answer1.SetRightAnswers(rightAnswers);
                answer1.SetFakes(fakes);
                var answer2 = new SomeVariantsAnswer {
                    SerializedRightAnswers = JsonSerializer.Serialize(rightAnswers),
                    SerializedFakes = JsonSerializer.Serialize(fakes)
                };

                string rightText1 = answer1.RightText;
                string rightText2 = answer2.RightText;

                foreach(var text in rightAnswers)
                {
                    Assert.Contains(text, rightText1);
                    Assert.Contains(text, rightText2);
                }
                foreach (var text in fakes)
                {
                    Assert.DoesNotContain(text, rightText1);
                    Assert.DoesNotContain(text, rightText2);
                }
            }
        }
    }
}
