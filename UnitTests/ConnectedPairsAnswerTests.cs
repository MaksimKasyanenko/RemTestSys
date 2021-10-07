using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ConnectedPairsAnswerTests
    {
        public class SettingAdditiveData
        {
            [Fact]
            public void SetMethodSetsSerilizedDataProperty()
            {
                var answer = new ConnectedPairsAnswer();
                var testData = GetTestAdditiveData();

                answer.SetPairs(testData);

                Assert.Equal(JsonSerializer.Serialize(testData), answer.SerializedPairs);
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
            }
            [Fact]
            public void SetMethodAndSerializedDataPropertyThrowInvalidOperationException_WhenPassedLessThen1Pair()
            {
                var answer = new ConnectedPairsAnswer();

                Assert.Throws<InvalidOperationException>(() => answer.SetPairs(new ConnectedPairsAnswer.Pair[0]));
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
