using Attribute = System.Xml.XmlAttribute;

namespace XParseCs.CodeGeneration
{
    /// <summary>
    /// Generates a 'using XYZ;' dependency in a specified region.
    /// </summary>
    class DepedenciesGenerator: ICodeGenerator
    {
        /// <summary>
        /// The namespace we depend on.
        /// </summary>
        private readonly string dependencyName;

#nullable enable
        /// <summary>
        /// The alias name, if one exists.
        /// </summary>
        private readonly string? aliasName;
#nullable disable

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
