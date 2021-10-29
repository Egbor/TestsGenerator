using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisApplication
{
    public class Program
    {
        private static readonly string _code = @"
            using System;

            namespace Application 
            {
                public class Program 
                {
                    public static void Main(string[] argv)
                    {
                        Console.WriteLine(""Hello, world!"");
                    }
                }
            }
";

        public static async Task Main(string[] argv)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(_code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            Console.WriteLine($"The tree is a {root.Kind()} node.");
            Console.WriteLine($"The tree has {root.Members.Count} elements in it.");
            Console.WriteLine($"The tree has {root.Usings.Count} using statements. They are:");
            foreach (MemberDeclarationSyntax element in root.Members)
            {
                Console.WriteLine($"\t{element.ToString()}");
            }
        }
    }
}
