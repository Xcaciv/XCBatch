using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core.Source
{
    /// <summary>
    /// generic default Source Block with Enumerable access
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SourceBlock<T> : ISourceBlock<ISource>, ICollection<T> where T : ISource
    {
        /// <summary>
        /// internal list
        /// </summary>
        protected ConcurrentQueue<T> internalList = new ConcurrentQueue<T>();

        public System.Type SourceType{ get => typeof(T); }

        /// <summary>
        /// construct empty collection
        /// </summary>
        public SourceBlock() { }

        /// <summary>
        /// constructor to set 
        /// </summary>
        /// <param name="list"></param>
        public SourceBlock(IEnumerable<T> list)
        {
            foreach (T item in list)
            {
                internalList.Enqueue(item);
            }
        }

        /// <summary>
        /// implementation list
        /// <see cref="ISourceBlock{T}"/>
        /// </summary>
        public IEnumerable<ISource> SourceList => (IEnumerable<ISource>)internalList;

        public int Count => internalList.Count;

        public bool IsReadOnly => true;

        public int DistributionId { get; set; } = -1;

        /// <summary>
        /// append source to the bock
        /// </summary>
        /// <param name="source"></param>
        public void Add(T item)
        {
            internalList.Enqueue(item);
        }

        /// <summary>
        /// <see cref="IEnumerable"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        /// <summary>
        /// <see cref="ICollection<T>"/>
        /// </summary>
        public void Clear()
        {
            internalList = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// <see cref="ICollection<T>"/>
        /// </summary>
        public bool Contains(T item)
        {
            return new List<T>(internalList).Contains(item);
        }

        /// <summary>
        /// <see cref="ICollection<T>"/>
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// <see cref="ICollection<T>"/>
        /// </summary>
        public bool Remove(T item)
        {
            return false;
        }

        /// <summary>
        /// <see cref="ICollection<T>"/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalList.GetEnumerator();
        }
    }
}
