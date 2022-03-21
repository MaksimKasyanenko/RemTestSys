﻿using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.AnswerModelTests
{
    public class TextAnswerTests
    {
        public class IsMatchMethodTests
        {
            [Fact]
            public void ReturnsFalse_WhenPassedNull()
            {
                var answer = new TextAnswer();

                var res = answer.IsMatch(null);

                Assert.False(res);
            }
            [Fact]
            public void ReturnsFalse_WhenPassedStringsMoreThen1()
            {
                var answer = new TextAnswer();

                var res = answer.IsMatch(new string[] { "", "" });

                Assert.False(res);
            }
            [Theory]
            [InlineData("string1", "string2")]
            [InlineData("str", "stri")]
            [InlineData("String", "string")]
            public void ReturnsFalse_WhenCaseMattersAndPassedStringAreDifferent(string rightText, string answerText)
            {
                var answer = new TextAnswer
                {
                    CaseMatters = true,
                    RightText = rightText
                };

                var res = answer.IsMatch(new string[] { answerText });

                Assert.False(res);
            }

            [Theory]
            [InlineData("string1", "string 1")]
            [InlineData("String1", "string 1")]
            [InlineData("     text", "     text")]
            public void ReturnsTrue_WhenStringsOnlyDifferSpacesAndCaseEachOther(string rightText, string answerText)
            {
                var answer = new TextAnswer
                {
                    RightText = rightText,
                    CaseMatters = false
                };

                var res = answer.IsMatch(new string[] { answerText });

                Assert.True(res);
            }

            [Theory]
            [InlineData("this text has ukrainian language letters", "thІs tЕxt hАs ukrАІnІАn languАge lЕtters")]
            [InlineData("Цей текст має символи англійскої мови", "ЦEй тEкст має симвOли англIйскої мOви")]
            public void ReturnsTrue_WhenStringsHaveLettersWhichLooksLikeEachOther(string rightText, string answerText)
            {
                var answer = new TextAnswer
                {
                    CaseMatters = false,
                    RightText = rightText
                };

                var res = answer.IsMatch(new string[] { answerText });

                Assert.True(res);
            }
        }
    }
}
