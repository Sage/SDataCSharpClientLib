// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Provides helper methods for discovering information about a <see cref="Type"/> .
    /// </summary>
    internal static class TypeInfoHelper
    {
        #region Fields

        private static readonly Dictionary<string, ConstructorInfo> _oXSDataTypeToSMEType;
        private static readonly Dictionary<Type, string> _oSMETypeToXSDataType;

        static TypeInfoHelper()
        {
            _oXSDataTypeToSMEType = new Dictionary<string, ConstructorInfo>(StringComparer.InvariantCultureIgnoreCase);
            _oXSDataTypeToSMEType[XSDataTypes.String] = typeof (SMEStringProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.NormalizedString] = typeof (SMENormalizedStringProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Token] = typeof (SMETokenProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Boolean] = typeof (SMEBoolProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Decimal] = typeof (SMEDecimalProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Float] = typeof (SMEFloatProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.DateTime] = typeof (SMEDateTimeProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Time] = typeof (SMETimeProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Date] = typeof (SMEDateProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.AnyURI] = typeof (SMEUriProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Double] = typeof (SMEDoubleProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Int] = typeof (SMEIntegerProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Integer] = typeof (SMEIntegerProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Long] = typeof (SMELongProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Short] = typeof (SMEShortProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Byte] = typeof (SMEByteProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.UnsignedInt] = typeof (SMEUnsignedIntegerProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.UnsignedLong] = typeof (SMEUnsignedLongProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.UnsignedShort] = typeof (SMEUnsignedShortProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.UnsignedByte] = typeof (SMEUnsignedByteProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.Base64Binary] = typeof (SMEBase64BinaryProperty).GetConstructor(Type.EmptyTypes);
            _oXSDataTypeToSMEType[XSDataTypes.HexBinary] = typeof (SMEHexBinaryProperty).GetConstructor(Type.EmptyTypes);

            _oSMETypeToXSDataType = new Dictionary<Type, string>();
            _oSMETypeToXSDataType[typeof (SMEStringProperty)] = XSDataTypes.String;
            _oSMETypeToXSDataType[typeof (SMEMultiLineStringProperty)] = XSDataTypes.String;
            _oSMETypeToXSDataType[typeof (SMENormalizedStringProperty)] = XSDataTypes.NormalizedString;
            _oSMETypeToXSDataType[typeof (SMETokenProperty)] = XSDataTypes.Token;
            _oSMETypeToXSDataType[typeof (SMEBoolProperty)] = XSDataTypes.Boolean;
            _oSMETypeToXSDataType[typeof (SMEDecimalProperty)] = XSDataTypes.Decimal;
            _oSMETypeToXSDataType[typeof (SMEFloatProperty)] = XSDataTypes.Float;
            _oSMETypeToXSDataType[typeof (SMEDateTimeProperty)] = XSDataTypes.DateTime;
            _oSMETypeToXSDataType[typeof (SMETimeProperty)] = XSDataTypes.Time;
            _oSMETypeToXSDataType[typeof (SMEDateProperty)] = XSDataTypes.Date;
            _oSMETypeToXSDataType[typeof (SMEUriProperty)] = XSDataTypes.AnyURI;
            _oSMETypeToXSDataType[typeof (SMEDoubleProperty)] = XSDataTypes.Double;
            _oSMETypeToXSDataType[typeof (SMEIntegerProperty)] = XSDataTypes.Int;
            _oSMETypeToXSDataType[typeof (SMELongProperty)] = XSDataTypes.Long;
            _oSMETypeToXSDataType[typeof (SMEShortProperty)] = XSDataTypes.Short;
            _oSMETypeToXSDataType[typeof (SMEByteProperty)] = XSDataTypes.Byte;
            _oSMETypeToXSDataType[typeof (SMEUnsignedIntegerProperty)] = XSDataTypes.UnsignedInt;
            _oSMETypeToXSDataType[typeof (SMEUnsignedLongProperty)] = XSDataTypes.UnsignedLong;
            _oSMETypeToXSDataType[typeof (SMEUnsignedShortProperty)] = XSDataTypes.UnsignedShort;
            _oSMETypeToXSDataType[typeof (SMEUnsignedByteProperty)] = XSDataTypes.UnsignedByte;
            _oSMETypeToXSDataType[typeof (SMEBase64BinaryProperty)] = XSDataTypes.Base64Binary;
            _oSMETypeToXSDataType[typeof (SMEHexBinaryProperty)] = XSDataTypes.HexBinary;
            _oSMETypeToXSDataType[typeof (SMEEnumProperty)] = XSDataTypes.String;
            _oSMETypeToXSDataType[typeof (SMEGuidProperty)] = XSDataTypes.String;
        }

        #endregion

        #region Methods

        private static readonly char[] EscapeCharacaters = new[] {'&', '<', '>', '\"'};

        internal static string Escape(string text)
        {
            if (text.IndexOfAny(EscapeCharacaters) == -1)
                return text;

            return SecurityElement.Escape(text);
        }

        internal static string FormatXS(string name)
        {
            return String.Format("{0}:{1}",
                                 Framework.Common.XS.Prefix,
                                 name);
        }

        /// <summary>
        /// Returns the xs data type for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to retrieve the xs data type for.</param>
        /// <returns>The xs data type for the specified type if one exists; otherwise, <b>null</b>.</returns>
        public static string GetXSDataType(Type type)
        {
            string xsdType = null;

            while (type != typeof (SMEProperty))
            {
                if (_oSMETypeToXSDataType.TryGetValue(type, out xsdType))
                    break;

                type = type.BaseType;
            }

            if (xsdType == null)
                return null;

            return FormatXS(xsdType);
        }

        /// <summary>
        /// Returns the constructor for the SME attribute class that handles the specified xs data type.
        /// </summary>
        /// <param name="type">The xs data type to retrieve the SME attribute constructor for.</param>
        /// <returns>The constructor for SME attribute class that handles the specified xs data type if one exists; otherwise, <b>null</b>.</returns>
        public static ConstructorInfo GetMetaDataConstructorFromXSDType(string type)
        {
            ConstructorInfo constructor;

            if (String.IsNullOrEmpty(type) || !_oXSDataTypeToSMEType.TryGetValue(type, out constructor))
                return null;

            return constructor;
        }

        /// <summary>
        /// Returns a value indicating if the specified qualified name is valid.
        /// </summary>
        /// <param name="name">The qualified name to check as being valid.</param>
        /// <returns><b>true</b> if the qualified name is valid; otherwise, <b>false</b>.</returns>
        public static bool IsValidQualifiedName(XmlQualifiedName name)
        {
            return name != null && !String.IsNullOrEmpty(name.Name);
        }

        #endregion
    }
}