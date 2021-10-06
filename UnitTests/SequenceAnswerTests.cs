using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
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
            public void ReturnsStringsWhichContainsAllElementsOfSentenceInRightOrder()
            {

            }
        }
    }
}
