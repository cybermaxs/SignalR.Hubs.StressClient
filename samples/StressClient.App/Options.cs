using CommandLine;

namespace StressClient.App
{
    class Options
    {
        [Option('d', "duration", DefaultValue = 15000, HelpText = "Duration of the run")]
        public int Duration { get; set; }

        [Option('u', "userload", DefaultValue = 500, HelpText = "number of concurrent virtual users")]
        public int UsersLoad { get; set; }

        [Option('c', "constant", DefaultValue = true, HelpText = "Constant User Load")]
        public bool Constant { get; set; }
    }
}
