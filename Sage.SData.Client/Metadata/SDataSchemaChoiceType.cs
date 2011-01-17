using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaChoiceType : SDataSchemaType
    {
        private IList<SDataSchemaChoiceItem> _types;

        public IList<SDataSchemaChoiceItem> Types
        {
            get { return _types ?? (_types = new List<SDataSchemaChoiceItem>()); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var type = (XmlSchemaComplexType) obj;
            var choice = type.Particle as XmlSchemaChoice;

            if (choice == null)
            {
                throw new NotSupportedException();
            }

            foreach (var item in choice.Items)
            {
                var element = item as XmlSchemaElement;

                if (element == null)
                {
                    throw new NotSupportedException();
                }

                var choiceType = new SDataSchemaChoiceItem();
                choiceType.Read(element);
                Types.Add(choiceType);
            }

            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var type = (XmlSchemaComplexType) obj;
            var choice = new XmlSchemaChoice {MinOccurs = 0};

            foreach (var choiceType in Types)
            {
                var element = new XmlSchemaElement();
                choiceType.Write(element);
                choice.Items.Add(element);
            }

            type.Particle = choice;
            base.Write(obj);
        }
    }
}