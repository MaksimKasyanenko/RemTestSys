using System;

namespace RemTestSys.Domain.Models
{
    public class Session
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Test Test { get; set; }
        public DateTime StartTime { get; set; }
        private bool _finished = false;
        public bool Finished
        {
            get 
            {
                return _finished || TimeLeft < 1 || _questionCursor >= _questions.Length;
            }
            set
            {
                if (value == false && _finished == true) throw new InvalidCastException("Finished session cannot be continued");
                _finished = value;
            }
        }
        public Question CurrentQuestion
        {
            get
            {
                return _questions[_questionCursor];
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
                if((_questions != null && value > _questions.Length) || value-1 < 0)
                    throw new IndexOutOfRangeException("question cursor");
                _questionCursor = value-1;
            }
        }
        private Question[] _questions;
        public Question[] Questions
        {
            get
            {
                if (_questions == null) throw new NullReferenceException("The Questions property hasn't been setted");
                return _questions;
            }
            set
            {
                if (_questions != null) throw new Exception("The Questions property mustn't be overwritten");
                if (value.Length <= _questionCursor) throw new IndexOutOfRangeException("Length of quesions array is less then cursor");
                _questions = value;
            }
        }
        public int TimeLeft
        {
            get
            {
                return (DateTime.Now - StartTime).Seconds;
            }
        }
        public bool NextQuestion()
        {
            if (!Finished) _questionCursor++;
            return !Finished;
        }
    }
}
