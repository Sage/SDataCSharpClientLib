using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaChoiceType : SDataSchemaType
    {
        private IList<SDataSchemaChoiceItem> _types;

        public SDataSchemaChoiceType()
        {
        }

        public SDataSchemaChoiceType(string baseName)
            : base(baseName, "choice")
        {
        }

        public IList<SDataSchemaChoiceItem> Types
        {
            get { return _types ?? (_types = new List<SDataSchemaChoiceItem>()); }
        }

        public decimal? MaxOccurs { get; set; }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var type = (XmlSchemaComplexType) obj;

            if (type.Particle == null)
            {
                throw new InvalidOperationException(string.Format("Missing particle on choice type '{0}'", type.Name));
            }

            var choice = type.Particle as XmlSchemaChoice;

            if (choice == null)
            {
                throw new InvalidOperationException(string.Format("Unexpected particle type '{0}' on choice type '{1}'", type.Particle.GetType(), type.Name));
            }

            MaxOccurs = !string.IsNullOrEmpty(choice.MaxOccursString) ? choice.MaxOccurs : (decimal?) null;

            foreach (var item in choice.Items)
            {
                var element = item as XmlSchemaElement;

                if (element == null)
                {
                    throw new InvalidOperationException(string.Format("Unexpected item type '{0}' on choice type '{1}'", item.GetType(), type.Name));
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

            if (MaxOccurs != null)
            {
                choice.MaxOccurs = MaxOccurs.Value;
            }

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