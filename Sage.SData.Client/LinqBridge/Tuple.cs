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

// $Id: Tuple.cs 215 2009-10-03 13:31:49Z azizatif $

namespace LinqBridge
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    [ Serializable ]
    internal struct Tuple<TFirst, TSecond> : IEquatable<Tuple<TFirst, TSecond>>
    {
        public TFirst First { get; private set; }
        public TSecond Second { get; private set; }

        public Tuple(TFirst first, TSecond second) : this()
        {
            First = first;
            Second = second;
        }

        public override bool Equals(object obj)
        {
            return obj != null 
                   && obj is Tuple<TFirst, TSecond> 
                   && base.Equals((Tuple<TFirst, TSecond>) obj);
        }

        public bool Equals(Tuple<TFirst, TSecond> other)
        {
            return EqualityComparer<TFirst>.Default.Equals(other.First, First) 
                   && EqualityComparer<TSecond>.Default.Equals(other.Second, Second);
        }

        public override int GetHashCode()
        {
            var num = 0x7a2f0b42;
            num = (-1521134295 * num) + EqualityComparer<TFirst>.Default.GetHashCode(First);
            return (-1521134295 * num) + EqualityComparer<TSecond>.Default.GetHashCode(Second);
        }

        public override string ToString()
        {
            return string.Format(@"{{ First = {0}, Second = {1} }}", First, Second);
        }
    }
}
