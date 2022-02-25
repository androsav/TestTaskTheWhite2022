using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	class JsonData : IComparable
	{
		public JsonData(string replacement, string source)
		{
			Replacement = replacement;
			Source = source;
		}

		public JsonData()
		{
		}

		public string Replacement { get; set; }
		public string Source { get; set; }

		public int CompareTo(object obj)
		{
			JsonData data = obj as JsonData;
			if (this.Replacement.Length < data.Replacement.Length)
				return 1;
			else if (this.Replacement == data.Replacement)
			{
				data.Replacement = "Removed";
				data.Source = "Removed";
				return 0;
			}
			else
				return -1;

		}
	}
}
