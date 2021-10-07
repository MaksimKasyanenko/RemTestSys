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

                answer.SetAdditiveData(new string[] {"a","b","c"}, new string[] {"A","B","C"});

                Assert.Equal("[\"a\",\"A\",\"b\",\"B\",\"c\",\"C\"]", answer.SerializedPairs);
            }

            [Theory]
            [InlineData(new string[] {}, new string[] {})]
            [InlineData(new string[] {"a",null}, new string[] {"A","B"})]
            [InlineData(new string[] {"a","b"}, new string[] {"A",null})]
            [InlineData(new string[] {"a","b","c"}, new string[] {"A","B"})]
            [InlineData(new string[] {"a","b"}, new string[] {"A","B","C"})]
            [InlineData(new string[] {"a"}, null)]
            [InlineData(null, new string[] { })]
            public void SetMethodAndSerializedDataPropertyThrowInvalidOperationException_WhenPassedWrongData(string[]leftList, string[]rightList)
            {
                var answer = new ConnectedPairsAnswer();
                string[] temp = new string[leftList.Length+rightList.Length];
                int tempIndex = 0;
                for(int i = 0; i < leftList.Length; i++)
                {
                    temp[tempIndex++] = leftList[i];
                    temp[tempIndex++] = rightList[i];
                }

                Assert.Throws<InvalidOperationException>(()=>answer.SetAdditiveData(leftList,rightList));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedPairs=JsonSerializer.Serialize(temp));
            }
        }

        public class GettingAdditiveData
        {
            [Theory]
            [InlineData(new string[] {"t"},new string[] {"T"})]
            [InlineData(new string[] {"tt","dd"}, new string[] {"TT","DD"})]
            [InlineData(new string[] {"1","2","3"}, new string[] {"a","b","c"})]
            public void ReturnsArrayAsLongAsLeftAndRightListBoth(string[] leftList, string[] rightList)
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetAdditiveData(leftList, rightList);
                int expectLength = leftList.Length + rightList.Length;

                string[] res = answer.GetAdditiveData();

                Assert.Equal(expectLength, res.Length);
            }

            [Fact]
            public void ReturnsStringArrayWhichContainsAllElementsFromSourceArraysAndinRightOrder()
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetAdditiveData(new string[] {"a","b","c"},new string[] {"A","B","C"});

                var res = answer.GetAdditiveData();

                Assert.Equal("a", res[0]);
                Assert.Equal("A", res[1]);
                Assert.Equal("b", res[2]);
                Assert.Equal("B", res[3]);
                Assert.Equal("c", res[4]);
                Assert.Equal("C", res[5]);
            }
        }

        public class RightTextTests
        {
            [Fact]
            public void ReturnsRightText()
            {
                var answer = new ConnectedPairsAnswer();
                answer.SetAdditiveData(new string[] {"a","b","c"},new string[] {"A","B","C"});

                string res = answer.RightText;

                Assert.Equal("a - A\nb - B\nc - C\n",res);
            }
        }
    }
}
