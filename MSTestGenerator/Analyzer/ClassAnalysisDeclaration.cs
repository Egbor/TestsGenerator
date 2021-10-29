using System.Collections.Generic;

namespace MSTestGenerator.Analyzer
{
    public class ClassAnalysisDeclaration
    {
        public string Name { get; }
        public List<MethodAnalysisDeclaration> Methods { get; }

        public ClassAnalysisDeclaration(string name, IEnumerable<MethodAnalysisDeclaration> methods)
        {
            Name = name;
            Methods = new List<MethodAnalysisDeclaration>(methods);
        }
    }
}
