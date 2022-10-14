using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;
using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.DataOperations
{
	public class MyDataOperations<T> : DataOperations<T> where T : IItem
	{
		private readonly Configuration configuration;

		protected Func<IQueryable<T>, IQueryable<T>> Searching { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Filtering { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Sorting { get; init; } = null!;
		protected Func<IQueryable<T>, IQueryable<T>> Paging { get; init; } = null!;

		public MyDataOperations(Configuration configuration)
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

				//var propertiesToCheck = new List<PropertyInfo>();
				//foreach (var field in this.configuration.FieldsForSearching)
				//{
				//	var property = itemsType.GetProperty(field);
				//	if (property == null)
				//	{
				//		throw new Exception($"Searching failed... Type '{itemsType}' does not have property '{field}'.");
				//	}

				//	propertiesToCheck.Add(property);
				//}
				//var searchingValue = this.configuration.SearchingValue;
				//items = items.Where(item =>
				//	/* We should be able to call .ToString() as we search only fields that are in the grid, 
				//	 * which are of "to-string-able" types such as string, int... 
				//	 * Also we don't use nulls in grid so to call .Contains() shoud be OK too. */
				//	propertiesToCheck.Any(property => property.GetValue(item)!.ToString()!.Contains(searchingValue)));

				//return items;
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

				//foreach (var pair in this.configuration.FieldValuePairsForFiltering)
				//{
				//	var property = itemsType.GetProperty(pair.Field);
				//	if (property == null)
				//	{
				//		throw new Exception($"Filtering failed... Type '{itemsType}' does not have property '{pair.Field}'.");
				//	}

				//	/* We should be able to call .ToString() as we filter only fields that are in the grid, 
				//	 * which are of "to-string-able" types such as string, int... 
				//	 * Also we don't use nulls in grid so to call .Contains() shoud be OK too. */
				//	items = items.Where(item => property.GetValue(item)!.ToString()!.Contains(pair.Value));
				//}

				//return items;
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

		public override IQueryable<T> PerformDataOperations(IQueryable<T> data)
		{
			var specialOperations = configuration.SpecialOperations;
			if (specialOperations != null)
			{
				data = specialOperations(data);
			}

			data = Searching(data);
			data = Filtering(data);
			data = Sorting(data);
			data = Paging(data);

			return data;
		}

		public class Configuration
		{
			public int? Skip { get; set; }
			public int? Take { get; set; }
			public string SearchingValue { get; set; } = string.Empty;
			public (string Field, string Value)[] FieldValuePairsForFiltering { get; set; }
			public (string Field, SortOrder Value)[] FieldValuePairsForSorting { get; set; }
			public string[] FieldsForSearching { get; set; }
			public Func<IQueryable<T>, IQueryable<T>>? SpecialOperations { get; set; }

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
