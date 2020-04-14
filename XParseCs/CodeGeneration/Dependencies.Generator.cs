namespace XParseCs.CodeGeneration
{
    class DepedenciesGenerator: ICodeGenerator
    {
        private string dependencyName;

        private string? aliasName;

        public DepedenciesGenerator(string dependency, string? alias)
        {
            this.dependencyName = dependency;
            this.aliasName = alias;
        }

        public string Parse()
            => aliasName == null ? $"\tusing {dependencyName};" : $"\tusing {aliasName} = {dependencyName};";
    }
}
