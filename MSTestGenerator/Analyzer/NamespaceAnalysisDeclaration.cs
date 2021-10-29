namespace MSTestGenerator.Analyzer
{
    public class NamespaceAnalysisDeclaration
    {
        public string Name { get; }
        public ClassAnalysisDeclaration Class { get; }

        public NamespaceAnalysisDeclaration(string name, ClassAnalysisDeclaration declaration)
        {
            Name = name;
            Class = declaration;
        }
    }
}
