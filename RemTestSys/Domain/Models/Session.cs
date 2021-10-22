using System;
using System.Collections.Generic;
using System.Linq;

namespace RemTestSys.Domain.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TestId{get;set;}
        public Test Test { get; set; }
        public DateTime StartTime { get; set; }
        public int RightAnswersCount { get; set; }
        public int? IdOfResult { get; set; }

        private bool _finished = false;
        public bool Finished
        {
            get 
            {
                return _finished || _questionCursor >= Test.QuestionsCount || TimeLeft < 1;
            }
            set
            {
                if (value == false && _finished == true) throw new InvalidCastException($"Finished session cannot be continued. (Session:{Id})");
                _finished = value;
            }
        }
        public Question CurrentQuestion
        {
            get
            {
                return Questions.First(q => q.SerialNumber == QuestionNum).Question;
            }
        }
        private int _questionCursor = 0;
        public int QuestionNum
        {
            get
            {
                return _questionCursor + 1;
            }
            set
            {
                if(value-1 < 0)
                    throw new IndexOutOfRangeException("question cursor");
                _questionCursor = value-1;
            }
        }
        public List<QuestionInSession> Questions { get; set; } = new List<QuestionInSession>();
        public int TimeLeft
        {
            get
            {
                return Test.Duration - (DateTime.Now - StartTime).Seconds;
            }
        }

        public bool NextQuestion()
        {
            if (!Finished) _questionCursor++;
            return !Finished;
        }
        public int GetMark()
        {
            if (!Finished) throw new InvalidOperationException("You cannot find out the mark before the session ends");
            return (int)(Test.ScoresPerRightAnswer * RightAnswersCount);
        }
    }
}
