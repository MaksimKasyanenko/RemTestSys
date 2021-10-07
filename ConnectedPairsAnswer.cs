using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemTestSys.Domain.Models
{
	public class ConnectedPairsAnswer : AnswerBase
	{
		public string SerializedPairs
		{
			get
			{
				return _serializedPairs;
			}
			set
			{
				if (value == null) throw new InvalidOperationException("Property cannot be setted as NULL");
				var pairs = JsonSerializer.Deserialize<Pair[]>(value);
				SetPairs(pairs);
			}
		}
		private string _serializedPairs;

		public void SetPairs(Pair[] pairs)
		{
			if (pairs == null) throw new InvalidOperationException("Pairs cannot be NULL");
			if (pairs.Length < 1) throw new InvalidOperationException("Pairs count must be over then 1");
			_serializedPairs = JsonSerializer.Serialize(pairs);
		}

		public override string[] GetAdditiveData()
		{
			Pair[] pairs = JsonSerializer.Deserialize<Pair[]>(_serializedPairs);
			string[] res = new string[pairs.Length * 2];
			RandomSequence rnd = new RandomSequence(0, pairs.Length);
			for (int i = 0; i < pairs.Length; i++)
			{
				res[i * 2] = pairs[i].Value1;
				res[i * 2 + 1] = pairs[i].Value2;
			}
			return res;
		}

		public override bool IsMatch(string[] data)
		{
			if (data == null) return false;
			if (data.Length % 2 > 0) return false;
			Pair[] pairs = JsonSerializer.Deserialize<Pair[]>(_serializedPairs);
			if (data.Length / 2 != pairs.Length) return false;

			Pair[] inPairs = new Pair[pairs.Length];
			for (int i = 0; i < inPairs.Length; i++)
			{
				inPairs[i] = new Pair(data[i * 2], data[i * 2 + 1]);
			}
			foreach (var pair in pairs)
			{
				if (!inPairs.Any(p => p.Value1 == pair.Value1 && p.Value2 == pair.Value2))
					return false;
			}
			return true;
		}

		public struct Pair
		{
			public string Value1
			{
				get { return value1; }
				set
				{
					if (value == null) throw new InvalidOperationException("The property cannot be setted as NULL");
					value1 = value;
				}
			}
			public string Value2
			{
				get { return value2; }
				set
				{
					(value == null) throw new InvalidOperationException("The property cannot be setted as NULL");
					value2 = value;
				}
			}
			private string value1;
			private string value2;
			public Pair(string v1, string v2)
			{
				Value1 = v1;
				Value2 = v2;
			}
		}
	}
}
