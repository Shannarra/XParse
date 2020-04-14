using System.Collections.Generic;
using System.Text;

namespace XParseCs
{
    static class ParseExtensions
    {
        private static string lastNodeVisited;

        public static readonly string TAB = "\t";

        public static string? ParseTrigger(System.Xml.XmlNode root)
        {
            string accumulatedText = "";

            if (root.Name == "namespace")
                if (root.Attributes["name"] == null)
                    throw new XParseException("The given namespace has no name!");
                else accumulatedText = $"namespace {root.Attributes["name"].Value}" + "\n{";

            if (lastNodeVisited == "namespace") // start a #region
                accumulatedText = $"{TAB}#region Dependencies";
            if (root.Name != lastNodeVisited && lastNodeVisited == "ref") // we went trough the last "using" directive
                accumulatedText = $"{TAB}#endregion Dependencies";

            if (root.Name == "ref")
                if (root.Attributes["name"] != null)
                    if (root.Attributes["alias"] != null) // it's alias 'using'
                        if (root.Attributes["aliasName"] == null) //we are using alias with no provided name, so throw an exception
                            throw new XParseException($"The alias to {root.Attributes["name"]} was NOT given a name!");
                        else //valid alias
                            accumulatedText = new CodeGeneration.DepedenciesGenerator(root.Attributes["name"].Value, root.Attributes["aliasName"].Value).Parse();
                    else // no alias
                        accumulatedText = new CodeGeneration.DepedenciesGenerator(root.Attributes["name"].Value, null).Parse();
                else //we're given a "using" with no name
                    throw new XParseException("The given \"using\" has no name!");


            lastNodeVisited = root.Name;

            return accumulatedText; // not found or recognized?
        }
    }
}
