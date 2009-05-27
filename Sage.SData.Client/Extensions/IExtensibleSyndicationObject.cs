using System;
using System.Collections.Generic;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Defines generalized extension properties, methods, indexers and events that a value type or class 
    /// implements to create a type-specific implementation of extension properties, methods, indexers and events.
    /// </summary>
    public interface IExtensibleSyndicationObject
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Extensions
        /// <summary>
        /// Gets or sets the syndication extensions applied to the syndication entity.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}"/> collection of <see cref="ISyndicationExtension"/> objects that represent syndication extensions applied to the syndication entity.</value>
        /// <seealso cref="SyndicationExtension"/>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        IEnumerable<ISyndicationExtension> Extensions
        {
            get;
            set;
        }
        #endregion

        #region HasExtensions
        /// <summary>
        /// Gets a value indicating if the syndication entity has one or more syndication extensions applied to it.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Extensions"/> collection contains one or more <see cref="ISyndicationExtension"/> objects, otherwise returns <b>false</b>.</value>
        bool HasExtensions
        {
            get;
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region AddExtension(ISyndicationExtension extension)
        /// <summary>
        /// Adds the supplied <see cref="ISyndicationExtension"/> to the current instance's <see cref="IExtensibleSyndicationObject.Extensions"/> collection.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be added.</param>
        /// <returns><b>true</b> if the <see cref="ISyndicationExtension"/> was added to the <see cref="IExtensibleSyndicationObject.Extensions"/> collection, otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        bool AddExtension(ISyndicationExtension extension);
        #endregion

        #region FindExtension(Predicate<ISyndicationExtension> match)
        /// <summary>
        /// Searches for a syndication extension that matches the conditions defined by the specified predicate, and returns the first occurrence within the <see cref="Extensions"/> collection.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{ISyndicationExtension}"/> delegate that defines the conditions of the <see cref="ISyndicationExtension"/> to search for.</param>
        /// <returns>
        ///     The first syndication extension that matches the conditions defined by the specified predicate, if found; otherwise, the default value for <see cref="ISyndicationExtension"/>.
        /// </returns>
        /// <remarks>
        ///     The <see cref="Predicate{ISyndicationExtension}"/> is a delegate to a method that returns <b>true</b> if the object passed to it matches the conditions defined in the delegate. 
        ///     The elements of the current <see cref="Extensions"/> are individually passed to the <see cref="Predicate{ISyndicationExtension}"/> delegate, moving forward in 
        ///     the <see cref="Extensions"/>, starting with the first element and ending with the last element. Processing is stopped when a match is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="match"/> is a null reference (Nothing in Visual Basic).</exception>
        ISyndicationExtension FindExtension(Predicate<ISyndicationExtension> match);
        #endregion

        #region RemoveExtension(ISyndicationExtension extension)
        /// <summary>
        /// Removes the supplied <see cref="ISyndicationExtension"/> from the current instance's <see cref="IExtensibleSyndicationObject.Extensions"/> collection.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be removed.</param>
        /// <returns><b>true</b> if the <see cref="ISyndicationExtension"/> was removed from the <see cref="IExtensibleSyndicationObject.Extensions"/> collection, otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        bool RemoveExtension(ISyndicationExtension extension);
        #endregion
    }
}
