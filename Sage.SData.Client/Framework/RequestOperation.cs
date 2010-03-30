// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Defines an operation to peform during a request.
    /// </summary>
    public class RequestOperation
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="RequestOperation"/> class.
        /// </summary>
        public RequestOperation()
            : this(HttpMethod.Get)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="RequestOperation"/> class.
        /// </summary>
        /// <param name="method"></param>
        public RequestOperation(HttpMethod method)
            : this(method, null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="RequestOperation"/> class with
        /// the specified <see cref="AtomEntry"/> and method.
        /// </summary>
        /// <param name="method">One of the <see cref="HttpMethod"/> values</param>
        /// <param name="resource">The input resource involved in the operation.</param>
        public RequestOperation(HttpMethod method, ISyndicationResource resource)
        {
            if (resource == null)
            {
                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    throw new InvalidOperationException("Content must be specified for POST and PUT requests");
                }
            }
            else
            {
                if (method != HttpMethod.Post && method != HttpMethod.Put)
                {
                    throw new InvalidOperationException("Content must only be specified for POST and PUT requests");
                }
            }

            Method = method;
            Resource = resource;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the method for the request.
        /// </summary>
        /// <value>One of the <see cref="HttpMethod"/> values.</value>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the predicate for the request.
        /// </summary>
        public string Predicate { get; set; }

        /// <summary>
        /// Gets or sets the input resource for the request.
        /// </summary>
        public ISyndicationResource Resource { get; set; }

        /// <summary>
        /// Gets or sets the ETag value for the request.
        /// </summary>
        /// <value>The ETag value for the request.</value>
        public string ETag { get; set; }

        #endregion
    }
}