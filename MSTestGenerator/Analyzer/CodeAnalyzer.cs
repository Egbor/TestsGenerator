using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MSTestGenerator.Analyzer
{
    public class CodeAnalyzer
    {
        public CodeAnalyzer()
        {

        }

        public NamespaceAnalysisDeclaration Analyze(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var namespaceDecalaration = root.Members.OfType<NamespaceDeclarationSyntax>().First();
            var classDeclaration = namespaceDecalaration.Members.OfType<ClassDeclarationSyntax>().First();
            var methodDeclaration = classDeclaration.Members.OfType<MethodDeclarationSyntax>().Where(method => method.Modifiers.Where(modifier => modifier.Kind() == SyntaxKind.PublicKeyword).Any());

            IEnumerable<MethodAnalysisDeclaration> anlsMethods = methodDeclaration.Select(method => new MethodAnalysisDeclaration(method.Identifier.ToString()));
            ClassAnalysisDeclaration anlsClass = new ClassAnalysisDeclaration(classDeclaration.Identifier.ToString(), anlsMethods);
            NamespaceAnalysisDeclaration anlsNamespace = new NamespaceAnalysisDeclaration(namespaceDecalaration.Name.ToString(), anlsClass);

            return anlsNamespace;
        }
    }
}
