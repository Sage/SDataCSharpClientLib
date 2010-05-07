#region License, Terms and Author(s)
//
// LINQBridge
// Copyright (c) 2007-9 Atif Aziz, Joseph Albahari. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// This library is free software; you can redistribute it and/or modify it 
// under the terms of the New BSD License, a copy of which should have 
// been delivered along with this distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
#endregion

// $Id: OrderedEnumerable.cs 237 2010-01-31 12:20:24Z azizatif $

namespace LinqBridge
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal sealed class OrderedEnumerable<T, K> : IOrderedEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly List<Comparison<T>> _comparisons;

        public OrderedEnumerable(IEnumerable<T> source, 
            Func<T, K> keySelector, IComparer<K> comparer, bool descending) :
            this(source, null, keySelector, comparer, descending) {}

        private OrderedEnumerable(IEnumerable<T> source, List<Comparison<T>> comparisons,
            Func<T, K> keySelector, IComparer<K> comparer, bool descending)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            _source = source;
            
            comparer = comparer ?? Comparer<K>.Default;

            if (comparisons == null)
                comparisons = new List<Comparison<T>>(/* capacity */ 4);

            comparisons.Add((x, y) 
                => (descending ? -1 : 1) * comparer.Compare(keySelector(x), keySelector(y)));

            _comparisons = comparisons;
        }

        public IOrderedEnumerable<T> CreateOrderedEnumerable<KK>(
            Func<T, KK> keySelector, IComparer<KK> comparer, bool descending)
        {
            return new OrderedEnumerable<T, KK>(_source, _comparisons, keySelector, comparer, descending);
        }

        public IEnumerator<T> GetEnumerator()
        {
            //
            // We sort using List<T>.Sort, but docs say that it performs an 
            // unstable sort. LINQ, on the other hand, says OrderBy performs 
            // a stable sort. So convert the source sequence into a sequence 
            // of tuples where the second element tags the position of the 
            // element from the source sequence (First). The position is 
            // then used as a tie breaker when all keys compare equal,
            // thus making the sort stable.
            //

			var list = _source.Select(new Func<T, int, Tuple<T, int>>(TagPosition)).ToList();
            
            list.Sort((x, y) => 
            {
                //
                // Compare keys from left to right.
                //

                var comparisons = _comparisons;
                for (var i = 0; i < comparisons.Count; i++)
                {
                    var result = comparisons[i](x.First, y.First);
                    if (result != 0)
                        return result;
                }

                //
                // All keys compared equal so now break the tie by their
                // position in the original sequence, making the sort stable.
                //

                return x.Second.CompareTo(y.Second);
            });

			return list.Select(new Func<Tuple<T, int>, T>(GetFirst)).GetEnumerator();

        }

        /// <remarks>
        /// See <a href="http://code.google.com/p/linqbridge/issues/detail?id=11">issue #11</a>
        /// for why this method is needed and cannot be expressed as a 
        /// lambda at the call site.
        /// </remarks>

		private static Tuple<T, int> TagPosition(T e, int i)
		{
			return new Tuple<T, int>(e, i);
		}

        /// <remarks>
        /// See <a href="http://code.google.com/p/linqbridge/issues/detail?id=11">issue #11</a>
        /// for why this method is needed and cannot be expressed as a 
        /// lambda at the call site.
        /// </remarks>

        private static T GetFirst(Tuple<T, int> pv)
		{
			return pv.First;
		}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
