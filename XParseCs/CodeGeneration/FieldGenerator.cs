using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    class FieldGenerator : ICodeGenerator
    {
        /// <summary>
        /// The property's name.
        /// </summary>
        readonly string name;
        
        /// <summary>
        /// The return type of the field.
        /// </summary>
        readonly string type;

#nullable enable
        /// <summary>
        /// The protection level
        /// </summary>
        readonly string? protectonLevel;

        /// <summary>
        /// Is the field <see cref="volatile"/>?
        /// </summary>
        readonly bool? isVolatile;

        /// <summary>
        /// Additional modifiers, such as 'unsafe' or 'static';
        /// </summary>
        readonly string[]? additional;

        /// <summary>
        /// A summary for the field.
        /// </summary>
        readonly string? summary;

        /// <summary>
        /// The field's given value.
        /// </summary>
        readonly string? value;

        public FieldGenerator(Attributes attributes)
        {
            if (attributes["name"] == null || string.IsNullOrEmpty(attributes["name"].Value))
                throw new XParseAttributeException("name");
            else
                name = attributes["name"].Value;

            if (attributes["type"] == null || string.IsNullOrEmpty(attributes["type"].Value))
                throw new XParseAttributeException("type");
            else
                type = attributes["type"].Value;
            
            if (attributes["protection"] != null)
                protectonLevel = attributes["protection"].Value;

            if (attributes["isVolatile"] != null)
                isVolatile = attributes["isVolatile"].Value.ToLower() == "true";

            if (attributes["additional"] != null)
                additional = attributes["additional"].Value.Contains(',')
                    ? attributes["additional"].Value.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { attributes["additional"].Value };

            if (attributes["summary"] != null)
                summary = attributes["summary"].Value;

            if (attributes["value"] != null)
                value = attributes["value"].Value;
        }

        public string Parse()
             => "\n\t\t" +
                $"{(summary == null ? string.Empty : $"\t///<summary>\n\t\t/// {summary}\n\t\t///</summary>\n\t\t")}" +
                $"{(protectonLevel == null ? string.Empty : protectonLevel + " ")}" +
                $"{(additional == null ? string.Empty : additional.Reduce() + " ")}" +
                $"{(isVolatile.HasValue && isVolatile.Value ? "volatile " : string.Empty)}" +
                $"{type} {name} {(value == null? string.Empty : $" = {(type == "string" ? $"\"{value}\"" : value)}")};";
    }
}
