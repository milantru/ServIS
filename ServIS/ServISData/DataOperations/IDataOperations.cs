using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServISData.DataOperations
{
    /// <summary>
    /// Represents an interface for performing data operations on a collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects in the data collection.</typeparam>
    public interface IDataOperations<T>
	{
        /// <summary>
        /// Performs data operations on the provided queryable data of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="data">The queryable data on which the operations are performed.</param>
        /// <returns>A queryable object of type <typeparamref name="T"/> representing 
        /// the result of the data operations.</returns>
        public IQueryable<T> PerformDataOperations(IQueryable<T> data);
	}
}
