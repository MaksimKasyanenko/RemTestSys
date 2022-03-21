using RemTestSys.Domain.Models;
using System;
using System.Text.Json;
using Xunit;

#pragma warning disable

namespace UnitTests.AnswerModelTests
{
    public class SequenceAnswerTests
    {
        public class SettingAdditiveData
        {
            [Theory]
            [InlineData(new string[] { "1", "2", "3" }, "[\"1\",\"2\",\"3\"]")]
            [InlineData(new string[] { "5", "df" }, "[\"5\",\"df\"]")]
            [InlineData(new string[] { "1", "2", "3","ghj" }, "[\"1\",\"2\",\"3\",\"ghj\"]")]
            public void SetsSequenceIntoSerializedDataPropertyOverSetMethod(string[] sequence, string result)
            {
                var answer = new SequenceAnswer();

                answer.SetSequence(sequence);

                Assert.Equal(result, answer.SerializedSequence);
            }
            [Fact]
            public void ThrowsInvalidOperationException_WhenPassedNullOverSetMethodAndOverProperty()
            {
                var answer = new SequenceAnswer();

                Assert.Throws<InvalidOperationException>(()=>answer.SetSequence(new string[] {"1",null,"3"}));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedSequence = "[\"1\",null,\"3\"]");
                Assert.Throws<InvalidOperationException>(()=>answer.SetSequence(null));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedSequence=null);
            }
            [Theory]
            [InlineData(new string[] {}, "[]")]
            [InlineData(new string[] {"test"}, "[\"test\"]")]
            public void ThrowsInvalidOperationException_WhenPassedLessThen2Elements(string[] sequence, string serialized)
            {
                var answer = new SequenceAnswer();

                Assert.Throws<InvalidOperationException>(()=>answer.SetSequence(sequence));
                Assert.Throws<InvalidOperationException>(() => answer.SerializedSequence = serialized);
            }
        }

        public class GettingAdditiveData
        {
            [Theory]
            [InlineData("tiiiii","teeest")]
            [InlineData("test","test2","test98","t")]
            public void GettedDataContainsAllOfSequenceElements_WhenSentenceHadBeenSettedOverMethod(params string[] sequence)
            {
                var answer = new SequenceAnswer();
                answer.SetSequence(sequence);

                var res = answer.GetAdditiveData();

                foreach(var el in sequence)
                {
                    Assert.Contains(el, res);
                }
            }
            [Theory]
            [InlineData("test1","test2")]
            [InlineData("test1", "2","5")]
            public void GettedDataContainsAllOfSequenceElements_WhenSentenceHadBeenSettedOverProperty(params string[] sequence)
            {
                var answer = new SequenceAnswer();
                answer.SerializedSequence = JsonSerializer.Serialize(sequence);

                var res = answer.GetAdditiveData();

                foreach(var el in sequence)
                {
                    Assert.Contains(el, res);
                }
            }
        }

        public class GettingRightText
        {
            [Fact]
            public void ReturnsStringWhichContainsAllElementsOfSentenceInRightOrder()
            {
                var answer = new SequenceAnswer();
                string[] source = new string[] { "1", "2", "3" };
                answer.SetSequence(source);

                string[] res = answer.RightText.Split(", ");

                for(int i = 0; i < source.Length; i++)
                {
                    Assert.Equal(source[i], res[i]);
                }
            }
        }

        public class Matching
        {
            [Theory]
            [InlineData("1","2")]
            [InlineData("12","13","14")]
            [InlineData("34","44","54","64")]
            public void ReturnsTrue_WhenPassedRightAnswer(params string[] sequence)
            {
                var answer = new SequenceAnswer();
                answer.SetSequence(sequence);
                var answerSequence = new string[sequence.Length];
                Array.Copy(sequence,answerSequence,sequence.Length);

                var res = answer.IsMatch(answerSequence);

                Assert.True(res);
            }

            [Theory]
            [InlineData("1", "22")]
            [InlineData("1", "2", "3")]
            [InlineData("1", "22", "3", "4")]
            public void ReturnsTrue_WhenPassedWrongAnswer(params string[] answerSequence)
            {
                var answer = new SequenceAnswer();
                answer.SetSequence(new string[] {"1","22","3"});

                var res = answer.IsMatch(answerSequence);

                Assert.False(res);
            }
        }
    }
}
