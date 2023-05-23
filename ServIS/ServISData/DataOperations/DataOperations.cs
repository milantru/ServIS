using ServISData.Interfaces;
using System.Reflection;

namespace ServISData.DataOperations
{
    /// <summary>
    /// Represents a custom implementation of data operations for objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects on which the data operations are performed.</typeparam>
    public class DataOperations<T> : IDataOperations<T> where T : IItem
	{
		private readonly Configuration configuration;

        /// <summary>
        /// Gets or sets the searching operation function applied on the provided queryable data of type <typeparamref name="T"/> 
		/// when <c>PerformDataOperations</c> method is called.
        /// </summary>
        protected Func<IQueryable<T>, IQueryable<T>> Searching { get; init; } = null!;

        /// <summary>
        /// Gets or sets the filtering operation function applied on the provided queryable data of type <typeparamref name="T"/> 
		/// when <c>PerformDataOperations</c> method is called.
        /// </summary>
        protected Func<IQueryable<T>, IQueryable<T>> Filtering { get; init; } = null!;

        /// <summary>
        /// Gets or sets the sorting operation function applied on the provided queryable data of type <typeparamref name="T"/> 
		/// when <c>PerformDataOperations</c> method is called.
        /// </summary>
        protected Func<IQueryable<T>, IQueryable<T>> Sorting { get; init; } = null!;

        /// <summary>
        /// Gets or sets the paging operation function applied on the provided queryable data of type <typeparamref name="T"/> 
		/// when <c>PerformDataOperations</c> method is called.
        /// </summary>
        protected Func<IQueryable<T>, IQueryable<T>> Paging { get; init; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOperations{T}"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration object for data operations.</param>
        public DataOperations(Configuration configuration)
		{
			this.configuration = configuration;

			var itemsType = typeof(T);			

			Searching = (items) =>
			{
				var searchingValue = this.configuration.SearchingValue;
				if (string.IsNullOrEmpty(searchingValue))
				{
					return items;
				}

				var propertiesToCheck = new List<PropertyInfo>();
				foreach (var field in this.configuration.FieldsForSearching)
				{
					var property = itemsType.GetProperty(field);
					if (property == null)
					{
						throw new Exception($"Searching failed... Type '{itemsType}' does not have property '{field}'.");
					}

					propertiesToCheck.Add(property);
				}

				var resItems = new List<T>();
				foreach (var item in items)
				{
					foreach (var property in propertiesToCheck)
					{
						var propertyValue = (property.GetValue(item) ?? new object()).ToString() ?? "";
						if (propertyValue.Contains(searchingValue))
						{
							resItems.Add(item);
							break;
						}
					}
				}

				items = resItems.AsQueryable();

				return items;
			};

			Filtering = (items) =>
			{
				var propertyValuePairsToCheck = new List<(PropertyInfo Property, string Value)>();
				foreach (var pair in this.configuration.FieldValuePairsForFiltering)
				{
					var property = itemsType.GetProperty(pair.Field);
					if (property == null)
					{
						throw new Exception($"Searching failed... Type '{itemsType}' does not have property '{pair.Field}'.");
					}

					propertyValuePairsToCheck.Add((property, pair.Value));
				}

				var resItems = new List<T>();
				foreach (var item in items)
				{
					foreach (var pair in propertyValuePairsToCheck)
					{
						var propertyValue = (pair.Property.GetValue(item) ?? new object()).ToString() ?? "";
						if (propertyValue.Contains(pair.Value))
						{
							resItems.Add(item);
							break;
						}
					}
				}

				items = resItems.AsQueryable();

				return items;
			};

			Sorting = (items) =>
			{
				foreach (var pair in this.configuration.FieldValuePairsForSorting)
				{
					var property = itemsType.GetProperty(pair.Field);
					if (property == null)
					{
						throw new Exception($"Sorting failed... Type '{itemsType}' does not have property '{pair.Field}'.");
					}

					var pairValue = pair.Value;
					if (pairValue == SortOrder.Ascending)
					{
						items = items.OrderBy(item => property.GetValue(item));
					}
					else if (pairValue == SortOrder.Descending)
					{
						items = items.OrderByDescending(item => property.GetValue(item));
					}
					else
					{
						items = items.OrderBy(item => item.Id);
					}
				}

				return items;
			};

			Paging = (items) =>
			{
				var skip = this.configuration.Skip;
				if (skip.HasValue)
				{
					items = items.Skip(skip.Value);
				}

				var take = this.configuration.Take;
				if (take.HasValue)
				{
					items = items.Take(take.Value);
				}

				return items;
			};
		}

        /// <inheritdoc/>
        /// <remarks>Order of the operations is:
        /// <list type="number">
        /// <item>SpecialOperations (if defined)</item>
        /// <item>Searching</item>
        /// <item>Filtering</item>
        /// <item>Sorting</item>
        /// <item>Paging</item>
        /// </list>
        /// </remarks>
        public IQueryable<T> PerformDataOperations(IQueryable<T> data)
		{
			var specialOperations = configuration.SpecialOperations;
			if (specialOperations is not null)
			{
				data = specialOperations(data);
			}

			data = Searching(data);
			data = Filtering(data);
			data = Sorting(data);
			data = Paging(data);

			return data;
		}

        /// <summary>
        /// Represents the configuration options for data operations.
        /// </summary>
        public class Configuration
		{
            /// <summary>
            /// Gets or sets the number of items to skip during paging operation.
            /// </summary>
            public int? Skip { get; set; }

            /// <summary>
            /// Gets or sets the number of items to take during paging operation.
            /// </summary>
            public int? Take { get; set; }

            /// <summary>
            /// Gets or sets the value to search during searching operation.
            /// </summary>
            public string SearchingValue { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets an array of field-value pairs used during filtering operation.
            /// </summary>
            public (string Field, string Value)[] FieldValuePairsForFiltering { get; set; }

            /// <summary>
            /// Gets or sets an array of field-value pairs used during sorting operation.
            /// </summary>
            public (string Field, SortOrder Value)[] FieldValuePairsForSorting { get; set; }

            /// <summary>
            /// Gets or sets an array of fields that should be used for searching during searching operation.
            /// </summary>
            public string[] FieldsForSearching { get; set; }

            /// <summary>
            /// Gets or sets a special operations function applied during performing data operations.
            /// </summary>
			/// <remarks>
			/// This can be used for defining some specific custom operations. 
			/// It is executed before searching, filtering, sorting, paging operations so it can be used 
			/// e.g. for reducing data on which the searching, filtering, sorting, paging operations will be performed.
			/// </remarks>
            public Func<IQueryable<T>, IQueryable<T>>? SpecialOperations { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Configuration"/> class.
            /// </summary>
            public Configuration()
			{
				var allFields = typeof(T).GetProperties().Select(p => p.Name);

				FieldValuePairsForFiltering = allFields.Select(f => (Field: f, Value: "")).ToArray();
				FieldValuePairsForSorting = allFields.Select(f => (Field: f, SortOrder: SortOrder.None)).ToArray();
				FieldsForSearching = allFields.ToArray();
			}
		}
	}
}
