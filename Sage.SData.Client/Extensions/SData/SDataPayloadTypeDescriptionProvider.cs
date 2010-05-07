using System;
using System.ComponentModel;
using System.Linq;

namespace Sage.SData.Client.Extensions
{
    ///<summary>
    /// A <see cref="TypeDescriptionProvider"/> that allows the <see cref="SDataPayload"/>
    /// class to be data bound.
    ///</summary>
    public class SDataPayloadTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly ICustomTypeDescriptor _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataPayloadTypeDescriptionProvider"/> class.
        /// </summary>
        public SDataPayloadTypeDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof (object)))
        {
            _inner = TypeDescriptor.GetProvider(typeof (SDataPayload)).GetTypeDescriptor(typeof (SDataPayload));
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new PayloadCustomTypeDescriptor(_inner, (SDataPayload) instance);
        }

        private class PayloadCustomTypeDescriptor : CustomTypeDescriptor
        {
            private readonly ICustomTypeDescriptor _inner;
            private readonly SDataPayload _instance;

            public PayloadCustomTypeDescriptor(ICustomTypeDescriptor inner, SDataPayload instance)
            {
                _inner = inner;
                _instance = instance;
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                return GetProperties(null);
            }

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                var properties = _inner.GetProperties()
                    .Cast<PropertyDescriptor>()
                    .Where(prop => prop.Name != "Values")
                    .Select(prop => (PropertyDescriptor) new ExistingPropertyDescriptor(prop))
                    .ToList();

                if (_instance != null)
                {
                    foreach (var pair in _instance.Values)
                    {
                        PropertyDescriptor prop;

                        if (pair.Value is SDataPayload)
                        {
                            prop = new VirtualPropertyDescriptor(pair.Key,
                                                                 typeof (SDataPayload),
                                                                 typeof (ExpandableObjectConverter));
                        }
                        else if (pair.Value is SDataPayloadCollection)
                        {
                            prop = new VirtualPropertyDescriptor(pair.Key,
                                                                 typeof (SDataPayloadCollection),
                                                                 typeof (ExpandablePayloadCollectionConverter));
                        }
                        else
                        {
                            prop = new VirtualPropertyDescriptor(pair.Key, typeof (string), null);
                        }

                        properties.Add(prop);
                    }
                }

                return new PropertyDescriptorCollection(properties.ToArray());
            }

            private class ExistingPropertyDescriptor : PropertyDescriptor
            {
                private readonly PropertyDescriptor _inner;

                public ExistingPropertyDescriptor(PropertyDescriptor inner)
                    : base(inner, new[] {new CategoryAttribute("SData")})
                {
                    _inner = inner;
                }

                #region PropertyDescriptor Members

                public override bool CanResetValue(object component)
                {
                    return _inner.CanResetValue(component);
                }

                public override object GetValue(object component)
                {
                    return _inner.GetValue(component);
                }

                public override void ResetValue(object component)
                {
                    _inner.ResetValue(component);
                }

                public override void SetValue(object component, object value)
                {
                    _inner.SetValue(component, value);
                }

                public override bool ShouldSerializeValue(object component)
                {
                    return _inner.ShouldSerializeValue(component);
                }

                public override Type ComponentType
                {
                    get { return _inner.ComponentType; }
                }

                public override bool IsReadOnly
                {
                    get { return _inner.IsReadOnly; }
                }

                public override Type PropertyType
                {
                    get { return _inner.PropertyType; }
                }

                #endregion
            }

            private class VirtualPropertyDescriptor : PropertyDescriptor
            {
                private readonly Type _type;

                public VirtualPropertyDescriptor(string name, Type type, Type typeConverterType)
                    : base(name, typeConverterType != null
                                     ? new Attribute[] {new CategoryAttribute("Values"), new TypeConverterAttribute(typeConverterType)}
                                     : new Attribute[] {new CategoryAttribute("Values")})
                {
                    _type = type;
                }

                #region PropertyDescriptor Members

                public override bool CanResetValue(object component)
                {
                    return true;
                }

                public override object GetValue(object component)
                {
                    return ((SDataPayload) component).Values[Name];
                }

                public override void ResetValue(object component)
                {
                    SetValue(component, null);
                }

                public override void SetValue(object component, object value)
                {
                    ((SDataPayload) component).Values[Name] = value;
                }

                public override bool ShouldSerializeValue(object component)
                {
                    return GetValue(component) != null;
                }

                public override Type ComponentType
                {
                    get { return typeof (SDataPayload); }
                }

                public override bool IsReadOnly
                {
                    get { return false; }
                }

                public override Type PropertyType
                {
                    get { return _type; }
                }

                #endregion
            }

            private class ExpandablePayloadCollectionConverter : TypeConverter
            {
                public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
                {
                    return new PropertyDescriptorCollection(
                        TypeDescriptor.GetProperties(value, attributes)
                            .Cast<PropertyDescriptor>()
                            .Where(prop => prop.Name != "Capacity" && prop.Name != "Count")
                            .Concat(((SDataPayloadCollection) value)
                                        .Select((item, i) => (PropertyDescriptor) new PayloadCollectionItemPropertyDescriptor(i)))
                            .ToArray());
                }

                public override bool GetPropertiesSupported(ITypeDescriptorContext context)
                {
                    return true;
                }

                private class PayloadCollectionItemPropertyDescriptor : PropertyDescriptor
                {
                    private readonly int _index;

                    public PayloadCollectionItemPropertyDescriptor(int index)
                        : base(string.Format("[{0}]", index), new[] {new TypeConverterAttribute(typeof (ExpandableObjectConverter))})
                    {
                        _index = index;
                    }

                    #region PropertyDescriptor Members

                    public override bool CanResetValue(object component)
                    {
                        return false;
                    }

                    public override object GetValue(object component)
                    {
                        return ((SDataPayloadCollection) component)[_index];
                    }

                    public override void ResetValue(object component)
                    {
                    }

                    public override void SetValue(object component, object value)
                    {
                    }

                    public override bool ShouldSerializeValue(object component)
                    {
                        return true;
                    }

                    public override Type ComponentType
                    {
                        get { return typeof (SDataPayloadCollection); }
                    }

                    public override bool IsReadOnly
                    {
                        get { return true; }
                    }

                    public override Type PropertyType
                    {
                        get { return typeof (SDataPayload); }
                    }

                    #endregion
                }
            }
        }
    }
}