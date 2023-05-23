using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Interfaces
{
    /// <summary>
    /// An interface for objects that represent photos.
    /// </summary>
    public interface IPhoto
	{
        /// <summary>
        /// Gets the photo data as a byte array.
        /// </summary>
        public byte[] Photo { get; }

        /// <summary>
        /// Gets a value indicating whether the photo is a title photo.
        /// </summary>
        public bool IsTitle { get; }
	}
}
