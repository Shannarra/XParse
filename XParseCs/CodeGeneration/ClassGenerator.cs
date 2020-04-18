using Attributes = System.Xml.XmlAttributeCollection;

namespace XParseCs.CodeGeneration
{
    /// <summary>
    /// Generates a class' header line + all it's inheritance list
    /// </summary>
    class ClassGenerator : ICodeGenerator
    {
        /// <summary>
        /// The class' name
        /// </summary>
        readonly string name;

        /// <summary>
        /// The class' protection level
        /// </summary>
        readonly string? protectionLevel;

        /// <summary>
        /// Is this class partial?
        /// </summary>
        readonly bool? isPartial;

        /// <summary>
        /// Is this class unsafe?
        /// </summary>
        readonly bool? isUnsafe;

        /// <summary>
        /// Any additional modifiers, such as "static", "sealed", et cetera
        /// </summary>
        readonly string[]? additional;

        /// <summary>
        /// The class' summary, if it has one
        /// </summary>
        readonly string? summary;

        /// <summary>
        /// The list of inheritance : IInterface, BaseClass
        /// </summary>
        readonly string[]? inherits;


        public ClassGenerator(Attributes attributes)
        {
            if (attributes["name"] == null || string.IsNullOrEmpty(attributes["name"].Value))
                throw new XParseAttributeException("name");
            else
                name = attributes["name"].Value;

            if (attributes["protection"] != null) // protection level
                protectionLevel = attributes["protection"].Value;

            if (attributes["isPartial"] != null) // partial?
                isPartial = attributes["isPartial"].Value == "true";

            if (attributes["isUnsafe"] != null) // unsafe?
                isUnsafe = attributes["isUnsafe"].Value == "true";

            if (attributes["additional"] != null) // splitting the 'additional' list
                additional = attributes["additional"].Value.Contains(',') ? attributes["additional"].Value.Split(",", 
                    System.StringSplitOptions.RemoveEmptyEntries) : new string[] { attributes["additional"].Value };

            if (attributes["summary"] != null) // getting the summary
                summary = attributes["summary"].Value;

            if (attributes["inherits"] != null) // splitting the 'inherits' list
                inherits = attributes["inherits"].Value.Contains(',') ? attributes["inherits"].Value.Split(',',
                    System.StringSplitOptions.RemoveEmptyEntries) : new string[] { attributes["inherits"].Value };
        }

        public string Parse()
            => $"\n\t" +
                $"{(summary == null ? string.Empty : $"///<summary>\n\t/// {summary}\n\t///</summary>\n\t")}" +
                $"{(protectionLevel == null ? string.Empty : protectionLevel + " ")}" +
                $"{(additional == null ? string.Empty : additional.Reduce() + " ")}" +
                $"{(isUnsafe == null ? string.Empty : isUnsafe.Value ? "unsafe " : string.Empty)}" +
                $"{(isPartial == null ? string.Empty : isPartial.Value ? "partial " : string.Empty)}" +
                $"class {name} {(inherits == null ? "\n\t{" : ": " + inherits.Reduce(',') + "\n\t{")}";
    }
}
