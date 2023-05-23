using ServISData.DataOperations;
using Syncfusion.Blazor;

namespace ServISWebApp.Shared
{
    /// <summary>
    /// Represents a generic implementation of data operations using Syncfusion's <see cref="DataManagerRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of items the operations are performed on.</typeparam>
    public class SyncfusionDataOperations<T> : IDataOperations<T>
	{
		private readonly DataManagerRequest dataManagerRequest;

        /// <summary>
        /// Gets or sets the function used for searching data.
        /// </summary>
		/// <para>
		/// <remarks>
		/// This function is used when <see cref="PerformDataOperations(IQueryable{T})"/> is called.
		/// </remarks>
		/// </para>
        protected Func<IQueryable<T>, IQueryable<T>> Searching { get; init; } = null!;

        /// <summary>
        /// Gets or sets the function used for filtering data.
        /// </summary>
		/// <para>
		/// <remarks>
		/// This function is used when <see cref="PerformDataOperations(IQueryable{T})"/> is called.
		/// </remarks>
		/// </para>
        protected Func<IQueryable<T>, IQueryable<T>> Filtering { get; init; } = null!;

        /// <summary>
        /// Gets or sets the function used for sorting data.
        /// </summary>
		/// <para>
		/// <remarks>
		/// This function is used when <see cref="PerformDataOperations(IQueryable{T})"/> is called.
		/// </remarks>
		/// </para>
        protected Func<IQueryable<T>, IQueryable<T>> Sorting { get; init; } = null!;

        /// <summary>
        /// Gets or sets the function used for paging data.
        /// </summary>
		/// <para>
		/// <remarks>
		/// This function is used when <see cref="PerformDataOperations(IQueryable{T})"/> is called.
		/// </remarks>
		/// </para>
        protected Func<IQueryable<T>, IQueryable<T>> Paging { get; init; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncfusionDataOperations{T}"/> class with the specified 
		/// instance of the <see cref="DataManagerRequest"/>.
        /// </summary>
        /// <param name="dataManagerRequest">The <see cref="DataManagerRequest"/> containing the data operation parameters.</param>
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

        /// <inheritdoc/>
        /// <remarks>Order of the operations is: 
        /// <list type="number">
        /// <item>Searching</item>
        /// <item>Filtering</item>
        /// <item>Sorting</item>
        /// <item>Paging</item>
        /// </list>
        /// </remarks>
        public IQueryable<T> PerformDataOperations(IQueryable<T> data)
		{
			data = Searching(data);
			data = Filtering(data);
			data = Sorting(data);
			data = Paging(data);

			return data;
		}
	}
}
