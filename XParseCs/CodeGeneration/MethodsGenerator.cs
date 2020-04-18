using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    class MethodsGenerator : ICodeGenerator
    {
        readonly string name;

        readonly string? protection;

        readonly string? type;

        readonly string? summary;

        readonly string? inheritsAs;

        readonly string[]? additional;

        readonly string[]? parameters;

        readonly string[]? cdata;

        public MethodsGenerator(Attributes attributes)
        {
            if (attributes["name"] == null || string.IsNullOrEmpty(attributes["name"].Value))
                throw new XParseAttributeException("name");
            else
                name = attributes["name"].Value;

            if (attributes["protection"] != null)
                protection = attributes["protection"].Value;

            if (attributes["type"] != null)
                type = attributes["type"].Value;

            if (attributes["summary"] != null)
                summary = attributes["summary"].Value;

            if (attributes["inheritsAs"] != null)
                if (attributes["inheritsAs"].Value.ToLower() != "abstract" &&
                    attributes["inheritsAs"].Value.ToLower() != "virtual")
                    throw new XParseException("Attribute \"inheritsAs\" was given unacceptable value!");
                else
                    inheritsAs = attributes["inheritsAs"].Value;

            if (attributes["additional"] != null)
                additional = attributes["additional"].Value.Contains(',')
                    ? attributes["additional"].Value.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { attributes["additional"].Value };

            if (attributes["params"]!= null)
                parameters = attributes["params"].Value.Contains(',')
                    ? attributes["params"].Value.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { attributes["params"].Value };
        }

        public MethodsGenerator(Attributes attributes, System.Xml.XmlNode firstChild)
            : this(attributes)
        {
            if (firstChild is System.Xml.XmlCDataSection)
            {
                var data = firstChild as System.Xml.XmlCDataSection;

                if (data.Value.Contains(';'))
                {
                    cdata = data.Value.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < cdata.Length; i++)
                        if (i < cdata.Length - 1)
                            cdata[i] += ";\n\t\t   ";
                        else cdata[i] += ';';
                }
                else cdata = new string[] { data.Value };
            }
        }

        public string Parse()
            => "\n\t" +
                $"{(summary == null ? string.Empty : $"\t///<summary>\n\t\t/// {summary}\n\t\t///</summary>\n\t\t")}" +
                $"{(protection == null ? string.Empty : protection + " ")}" +
                $"{(inheritsAs == null ? string.Empty : inheritsAs + " ")}" +
                $"{(additional == null ? string.Empty : additional.Reduce(' ') + " ")}" +
                $"{(type == null ? "void " : type + " ")}" +
                $"{name}({(parameters == null ? string.Empty : parameters.Reduce(','))})\n\t\t" + 
                "{\n\t\t\t"+$"{(cdata == null ?"//write some quality code here :)" : cdata.Reduce())}"+"\n\t\t}";
    }
}
