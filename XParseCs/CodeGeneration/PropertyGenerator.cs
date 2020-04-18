using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    class PropertyGenerator : ICodeGenerator
    {
        /// <summary>
        /// The property's name.
        /// </summary>
        readonly string name;

#nullable enable
        /// <summary>
        /// The protection level
        /// </summary>
        readonly string? protectonLevel;

        /// <summary>
        /// The return type of the property.
        /// </summary>
        readonly string? type;

        /// <summary>
        /// The 'get' protection level of the property.
        /// </summary>
        readonly string? get;

        /// <summary>
        /// The 'set' protection level of the property.
        /// </summary>
        readonly string? set;

        /// <summary>
        /// Additional modifiers, such as 'unsafe' or 'static';
        /// </summary>
        readonly string[]? additional;

        /// <summary>
        /// A summary for the property.
        /// </summary>
        readonly string? summary;

        public PropertyGenerator(Attributes attributes)
        {
            if (attributes["name"] == null || string.IsNullOrEmpty(attributes["name"].Value))
                throw new XParseAttributeException("name");
            else
                name = attributes["name"].Value;

            if (attributes["protection"] != null)
                protectonLevel = attributes["protection"].Value;

            if (attributes["type"] != null)
                type = attributes["type"].Value;

            if (attributes["get"] != null)
                get = attributes["get"].Value;

            if (attributes["set"] != null)
                set = attributes["set"].Value;

            if (attributes["additional"] != null)
                additional = attributes["additional"].Value.Contains(',')
                    ? attributes["additional"].Value.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { attributes["additional"].Value };

            if (attributes["summary"] != null)
                summary = attributes["summary"].Value;
        }

        public string Parse()
            => "\n\t" +
                $"{(summary == null ? string.Empty : $"\t///<summary>\n\t\t/// {summary}\n\t\t///</summary>\n\t\t")}" +
                $"{(protectonLevel == null ? string.Empty : protectonLevel + " ")}" +
                $"{(additional == null ? string.Empty : additional.Reduce() + " ")}" +
                $"{(type == null ? "void " : type + " ")}" +
                $"{name} " + "{" + $"{(get == null ? " " : $" {get} ")}get; " + $"{(set == null ? string.Empty : set + " ")}" + "set; }";
    }
}
