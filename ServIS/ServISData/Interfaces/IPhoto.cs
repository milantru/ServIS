using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Interfaces
{
	public interface IPhoto
	{
		public byte[] Photo { get; }
		public bool IsTitle { get; }
	}
}
