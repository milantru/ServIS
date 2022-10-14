using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServISData.DataOperations
{
	public abstract class DataOperations<T>
	{
		public abstract IQueryable<T> PerformDataOperations(IQueryable<T> data);
	}
}
