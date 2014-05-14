using CommandLine;

namespace StressClient.App
{
    class Options
    {
        [Option('d', "duration", DefaultValue = 60000, HelpText = "Duration of the run")]
        public int Duration { get; set; }

        [Option('u', "userload", DefaultValue = 100, HelpText = "number of concurrent virtual users")]
        public int UsersLoad { get; set; }

        [Option('c', "constant", DefaultValue = true, HelpText = "Constant User Load")]
        public bool Constant { get; set; }
    }
}
