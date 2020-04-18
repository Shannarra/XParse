using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    class FieldGenerator : ICodeGenerator
    {

        readonly string name;

        readonly string? protectonLevel;

        readonly string? type;

        readonly string? get;

        readonly string? set;

        readonly bool? isVolatile;

        readonly string[]? additional;

        readonly string summary;

        public FieldGenerator(Attributes attributes)
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

            if (attributes["isVolatile"] != null)
                isVolatile = attributes["isVolatile"].Value.ToLower() == "true";

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
                $"{(isVolatile == null ? string.Empty : isVolatile.Value ? "volatile " : string.Empty)}" +
                $"{(type == null ? "void " : type + " ")}" +
                $"{name} " + " { " + $"{(get ?? string.Empty)} get; " + $"{(set ?? string.Empty)}" + " set; }";
    }
}
