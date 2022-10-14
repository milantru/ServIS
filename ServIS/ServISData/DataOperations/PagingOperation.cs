using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.DataOperations
{
	public class PagingOperation<T> : DataOperations<T>
	{
		private Func<IQueryable<T>, IQueryable<T>> Paging { get; set; }

		public PagingOperation(int? skip = null, int ? take = null)
		{
			Paging = (items) =>
			{
				if (skip.HasValue)
				{
					items = items.Skip(skip.Value);
				}
				if (take.HasValue)
				{
					items = items.Take(take.Value);
				}

				return items;
			};
		}

		public override IQueryable<T> PerformDataOperations(IQueryable<T> data)
		{
			data = Paging(data);

			return data;
		}
	}
}
