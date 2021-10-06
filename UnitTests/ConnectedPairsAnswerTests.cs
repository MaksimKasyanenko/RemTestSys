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
            public void SetMethodThrowsInvalidOperationException_WhenPassedWrongData(string[]leftList, string[]rightList)
            {
                var answer = new ConnectedPairsAnswer();
                string[] temp = new string[leftList.Length+rightList.Length];
                for(int i = 0; i < temp.Length-1; i+=2)
                {
                    temp[i] = leftList[i/2];
                    temp[i+1] = rightList[i/2];
                }

                Assert.Throws<InvalidOperationException>(()=>answer.SetAdditiveData(leftList,rightList));
                Assert.Throws<InvalidOperationException>(()=>answer.SerializedPairs=JsonSerializer.Serialize(temp));
            }
        }
    }
}
