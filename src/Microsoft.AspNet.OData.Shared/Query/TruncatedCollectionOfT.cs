﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Common;

namespace Microsoft.AspNet.OData.Query
{
    /// <summary>
    /// Represents a class that truncates a collection to a given page size.
    /// </summary>
    /// <typeparam name="T">The collection element type.</typeparam>
    public class TruncatedCollection<T> : List<T>, ITruncatedCollection, IEnumerable<T>, ICountOptionCollection
    {
        /// <summary>
        /// Whether to return only the count and nothing else
        /// </summary>
        public bool OnlyCount { get; }

        private const int MinPageSize = 1;

        private bool _isTruncated;
        private int _pageSize;
        private long? _totalCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The collection to be truncated.</param>
        /// <param name="pageSize">The page size.</param>
        public TruncatedCollection(IEnumerable<T> source, int pageSize)
            : base(PrepareBase(source?.AsQueryable(), pageSize))
        {
            Initialize(pageSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The queryable collection to be truncated.</param>
        /// <param name="pageSize">The page size.</param>
        // NOTE: The queryable version calls Queryable.Take which actually gets translated to the backend query where as 
        // the enumerable version just enumerates and is inefficient.
        public TruncatedCollection(IQueryable<T> source, int pageSize)
            : base(PrepareBase(source, pageSize))
        {
            Initialize(pageSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The queryable collection to be truncated.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="onlyCount">Whether to only return the count and nothing else.</param>
        public TruncatedCollection(IEnumerable<T> source, int pageSize, long? totalCount, bool onlyCount)
            : base(PrepareBase(source?.AsQueryable(), pageSize))
        {
            OnlyCount = onlyCount;
            if (pageSize > 0)
            {
                Initialize(pageSize);
            }

            _totalCount = totalCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruncatedCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The queryable collection to be truncated.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="onlyCount">Whether to only return the count and nothing else.</param>
        // NOTE: The queryable version calls Queryable.Take which actually gets translated to the backend query where as 
        // the enumerable version just enumerates and is inefficient.
        public TruncatedCollection(IQueryable<T> source, int pageSize, long? totalCount, bool onlyCount)
            : base(PrepareBase(source, pageSize))
        {
            OnlyCount = onlyCount;
            if (pageSize > 0)
            {
                Initialize(pageSize);
            }

            _totalCount = totalCount;
        }

        private static IQueryable<T> PrepareBase(IQueryable<T> source, int pageSize)
        {
            return pageSize > 0 ? source?.Take(checked(pageSize + 1)) : source ?? new T[] { }.AsQueryable();
        }

        private void Initialize(int pageSize)
        {
            if (pageSize == 0)
            {
                return;
            }
            if (pageSize < MinPageSize)
            {
                throw Error.ArgumentMustBeGreaterThanOrEqualTo("pageSize", pageSize, MinPageSize);
            }

            _pageSize = pageSize;

            if (Count > pageSize)
            {
                _isTruncated = true;
                RemoveAt(Count - 1);
            }
        }

        /// <inheritdoc />
        public int PageSize
        {
            get { return _pageSize; }
        }

        /// <inheritdoc />
        public bool IsTruncated
        {
            get { return _isTruncated; }
        }

        /// <inheritdoc />
        public long? TotalCount
        {
            get { return _totalCount; }
        }
    }
}
