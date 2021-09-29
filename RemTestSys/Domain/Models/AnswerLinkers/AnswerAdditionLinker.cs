using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models.AnswerLinkers
{
    public abstract class AnswerAdditionLinker
    {
        public static AnswerAdditionLinker Create(Answer.Types type)
        {
            AnswerAdditionLinker res = null;
            switch (type)
            {
                case Answer.Types.Text: res = new TextAdditionLinker(); break;
                case Answer.Types.OneVariant: res = new OneVariantAdditionLinker(); break;
                case Answer.Types.MultipleVariant: res = new MultipleVariantAdditionLinker(); break;
                case Answer.Types.Conformity: res = new ConformityAdditionLinker(); break;
                case Answer.Types.Chain: res = new ChainAdditionLinker(); break;
                default: throw new NotImplementedException($"The AddiotionLinker for answer type {type} is not implemented");
            }
            return res;
        }
        public abstract string[] Link(Answer answer);
    }
}
