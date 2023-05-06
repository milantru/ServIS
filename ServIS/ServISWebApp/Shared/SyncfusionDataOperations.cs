using ServISData.DataOperations;
using Syncfusion.Blazor;

namespace ServISWebApp.Shared
{
	public class SyncfusionDataOperations<T> : DataOperations<T>
	{
		private readonly DataManagerRequest dataManagerRequest;

		protected Func<IQueryable<T>, IQueryable<T>> Searching { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Filtering { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Sorting { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Paging { get; init; } = null!;

		public SyncfusionDataOperations(DataManagerRequest dataManagerRequest)
		{
			this.dataManagerRequest = dataManagerRequest;

			Searching = (items) =>
			{
				if (this.dataManagerRequest.Search != null && this.dataManagerRequest.Search.Count > 0)
				{
					items = DataOperations.PerformSearching(items, this.dataManagerRequest.Search);
				}

				return items;
			};

			Sorting = (items) =>
			{
				if (this.dataManagerRequest.Sorted != null && this.dataManagerRequest.Sorted.Count > 0)
				{
					items = DataOperations.PerformSorting(items, this.dataManagerRequest.Sorted);
				}

				return items;
			};

			Filtering = (items) =>
			{
				if (this.dataManagerRequest.Where != null && this.dataManagerRequest.Where.Count > 0)
				{
					items = DataOperations.PerformFiltering(
						items, this.dataManagerRequest.Where, this.dataManagerRequest.Where[0].Operator);
				}

				return items;
			};

			Paging = (items) =>
			{
				if (this.dataManagerRequest.Skip != 0)
				{
					items = DataOperations.PerformSkip(items, this.dataManagerRequest.Skip);
				}
				if (this.dataManagerRequest.Take != 0)
				{
					items = DataOperations.PerformTake(items, this.dataManagerRequest.Take);
				}

				return items;
			};
		}

		public override IQueryable<T> PerformDataOperations(IQueryable<T> data)
		{
			data = Searching(data);
			data = Filtering(data);
			data = Sorting(data);
			data = Paging(data);

			return data;
		}
	}
}
