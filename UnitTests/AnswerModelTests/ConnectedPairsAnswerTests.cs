using RemTestSys.Domain.Models;
using System;
using Xunit;

namespace UnitTests
{
    public class ConnectedPairsAnswerTests
    {
        public class SettingAdditiveData
        {
            [Fact]
            public void SetMethodAndSerializedPairsPropertySetsSerilizedDataProperty()
            {
                var answer1 = new ConnectedPairsAnswer();
                var answer2 = new ConnectedPairsAnswer();
                var testData = GetTestAdditiveData();

                answer1.SetPairs(testData);
                answer2.SerializedPairs = "[{\"Value1\":\"a\",\"Value2\":\"A\"},{\"Value1\":\"b\",\"Value2\":\"B\"},{\"Value1\":\"c\",\"Value2\":\"C\"}]";

                Assert.Equal("[{\"Value1\":\"a\",\"Value2\":\"A\"},{\"Value1\":\"b\",\"Value2\":\"B\"},{\"Value1\":\"c\",\"Value2\":\"C\"}]", answer1.SerializedPairs);
                Assert.Equal("[{\"Value1\":\"a\",\"Value2\":\"A\"},{\"Value1\":\"b\",\"Value2\":\"B\"},{\"Value1\":\"c\",\"Value2\":\"C\"}]", answer2.SerializedPairs);
            }

            [Fact]
            public void ThrowsInvalidOperationException_WhenPassedNullToConstructorOfPairStruct()
            {
                Assert.Throws<InvalidOperationException>(()=>new ConnectedPairsAnswer.Pair("a",null));
                Assert.Throws<InvalidOperationException>(()=>new ConnectedPairsAnswer.Pair(null,"b"));
            }

            [Fact]
            public void SetMethodAndSerializedDataPropertyThrowInvalidOperationException_WhenPassedNull()
            {
                var answer = new ConnectedPairsAnswer();

                Assert.Throws<InvalidOperationException>(()=>answer.SetPairs(null));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedPairs=null);
            }
            [Fact]
            public void SetMethodAndSerializedDataPropertyThrowInvalidOperationException_WhenPassedLessThen1Pair()
            {
                var answer = new ConnectedPairsAnswer();

                Assert.Throws<InvalidOperationException>(() => answer.SetPairs(new ConnectedPairsAnswer.Pair[0]));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedPairs="[]");
            }
        }

        public class GettingAdditiveData
        {
            [Fact]
            public void ReturnsStringArrayWhichContainsAllElementsFromSourceArraysInRightOrder()
            {
                var answer = new ConnectedPairsAnswer();
                var testData = GetTestAdditiveData();
                answer.SetPairs(testData);

                var res = answer.GetAdditiveData();

                Assert.Equal(0, Array.IndexOf(res, "a") % 2);
                Assert.Equal(0, Array.IndexOf(res, "b") % 2);
                Assert.Equal(0, Array.IndexOf(res, "c") % 2);
                Assert.Equal(1, Array.IndexOf(res, "A") % 2);
                Assert.Equal(1, Array.IndexOf(res, "B") % 2);
                Assert.Equal(1, Array.IndexOf(res, "C") % 2);
            }
        }

        public class RightTextTests
        {
            [Fact]
            public void ReturnsRightText()
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetPairs(GetTestAdditiveData());

                string res = answer.RightText;

                Assert.Equal("a - A\nb - B\nc - C\n",res);
            }
        }

        public class Matching
        {
            [Theory]
            [InlineData("a", "A", "b", "C", "c", "B")]
            [InlineData("a","B","c","A","b","C")]
            [InlineData("a", "A", "b", "B", "c")]
            [InlineData("a", "A", "b", "B", "c", "C","d")]
            [InlineData("a", "A", "c", "C")]
            public void ReturnsFalse_WhenPassedWrongAnswerArray(params string[] answerArray)
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetPairs(GetTestAdditiveData());

                var res = answer.IsMatch(answerArray);

                Assert.False(res);
            }
            [Fact]
            public void ReturnsFalse_WhenPassedNull()
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetPairs(GetTestAdditiveData());

                var res = answer.IsMatch(null);

                Assert.False(res);
            }
            [Fact]
            public void ReturnsTrue_WhenPassedRightAnswerArray()
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetPairs(GetTestAdditiveData());
                string[] answerArray = new string[] {"a","A","b","B","c","C"};

                bool res = answer.IsMatch(answerArray);

                Assert.True(res);
            }
        }

        private static ConnectedPairsAnswer.Pair[] GetTestAdditiveData()
        {
            return new ConnectedPairsAnswer.Pair[] {
                    new ConnectedPairsAnswer.Pair("a","A"),
                    new ConnectedPairsAnswer.Pair("b","B"),
                    new ConnectedPairsAnswer.Pair("c","C"),
                };
        }
    }
}
