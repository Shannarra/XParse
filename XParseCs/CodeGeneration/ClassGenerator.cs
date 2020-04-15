using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    class ClassGenerator : ICodeGenerator
    {
        readonly string name;

        readonly string? protectionLevel;

        readonly bool? isPartial;

        readonly bool? isUnsafe;

        readonly string[]? others;

        readonly string? summary;

        readonly string[]? inherits;

        public ClassGenerator(Attributes attributes)
        {
            if (attributes["name"] == null)
                throw new XParseAttributeException("name");
            else
                name = attributes["name"].Value;
        }

        public string Parse()
        {
            return "";
        }
    }
}
