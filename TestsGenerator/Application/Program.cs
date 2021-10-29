using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;
using MSTestGenerator.Generator;

namespace TestsGenerator.Application
{
    public class Program
    {
        private static TestGenerator _generator = new TestGenerator();

        private static async Task<PipeInputArgument[]> ReadFiles(PipeInputArgument[] args)
        {
            return await Task<PipeInputArgument[]>.Factory.StartNew(() => args.Select(x => new PipeInputArgument(x.path, File.ReadAllText(x.path))).ToArray());
        }

        private static async Task<PipeInputArgument[]> GenerateTests(PipeInputArgument[] args)
        {
            return await Task<PipeInputArgument[]>.Factory.StartNew(() => args.Select(x => new PipeInputArgument(x.path, _generator.Generate(x.code).Result)).ToArray());
        }

        private static void WriteFiles(PipeInputArgument[] args)
        {
            foreach (PipeInputArgument arg in args)
            {
                File.WriteAllTextAsync(Path.GetFileNameWithoutExtension(arg.path) + ".test" + Path.GetExtension(arg.path), arg.code);
            }
        }

        private static IEnumerable<IEnumerable<PipeInputArgument>> CreateArguments(string[] args)
        {
            int maxFilesCount = Convert.ToInt32(args[0]);
            List<string> paths = new List<string>();

            for (int i = 1; i < args.Length; i++)
            {
                paths.Add(args[i]);
            }

            List<List<PipeInputArgument>> inputs = new List<List<PipeInputArgument>>();

            for (var i = 0; i < (float)paths.Count / maxFilesCount; i++)
            {
                yield return paths.Skip(i * maxFilesCount).Take(maxFilesCount).Select(x => new PipeInputArgument(x));
            }
        }

        public static void Main(string[] args)
        {
            IEnumerable<IEnumerable<PipeInputArgument>> inputs = CreateArguments(args);
            List<ActionBlock<PipeInputArgument[]>> waits = new List<ActionBlock<PipeInputArgument[]>>();

            foreach (IEnumerable<PipeInputArgument> input in inputs)
            {

                var readedFiles = new TransformBlock<PipeInputArgument[], PipeInputArgument[]>(ReadFiles);
                var generatedTests = new TransformBlock<PipeInputArgument[], PipeInputArgument[]>(GenerateTests);
                var savedTests = new ActionBlock<PipeInputArgument[]>(WriteFiles);

                var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

                readedFiles.LinkTo(generatedTests, linkOptions);
                generatedTests.LinkTo(savedTests, linkOptions);

                readedFiles.Post(input.ToArray());
                readedFiles.Complete();

                waits.Add(savedTests);
            }

            foreach (ActionBlock<PipeInputArgument[]> action in waits)
            {
                action.Completion.Wait();
            }
        }
    }
}
