namespace TestsGenerator.Application
{
    public struct PipeInputArgument
    {
        public string path;
        public string code;

        public PipeInputArgument(string path)
        {
            this.path = path;
            this.code = null;
        }

        public PipeInputArgument(string path, string code)
        {
            this.path = path;
            this.code = code;
        }
    }
}
