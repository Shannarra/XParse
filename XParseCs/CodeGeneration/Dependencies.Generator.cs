using Attribute = System.Xml.XmlAttribute;

namespace XParseCs.CodeGeneration
{
    class DepedenciesGenerator: ICodeGenerator
    {
        private readonly string dependencyName;

        private readonly string? aliasName;

        public DepedenciesGenerator(Attribute dependency, Attribute hasAlias, Attribute alias)
        {
            if (dependency == null)
                throw new XParseException("Empty dependency given!");
            else
                dependencyName = dependency.Value;

            if (hasAlias != null && hasAlias.Value.ToLower() == "true")
                if (alias != null && !string.IsNullOrEmpty(alias.Value))
                    this.aliasName = alias.Value;
                else
                    throw new XParseException("The system was not given an alias name!");
        }

        public string Parse()
            => aliasName == null ? $"\tusing {dependencyName};" : $"\tusing {aliasName} = {dependencyName};";
    }
}
