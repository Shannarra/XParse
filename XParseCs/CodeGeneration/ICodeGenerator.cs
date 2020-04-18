namespace XParseCs.CodeGeneration
{
    /// <summary>
    /// Represents a code generation interface.
    /// </summary>
    interface ICodeGenerator
    {
        /// <summary>
        /// Parses the currently given <see cref="System.Xml.XmlNode"/> and converts it to a <see cref="string"/>.
        /// </summary>
        /// <returns>string</returns>
        public string Parse();
    }
}
