using Yomicchi.Core;
using Newtonsoft.Json;
using System.IO;

namespace Yomicchi.Desktop
{
    public class State
    {
        private static readonly string ApplicationDirectory
            = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yomicchi");

        private static readonly string StateFilepath
            = Path.Combine(ApplicationDirectory, "state.dat");

        public List<Source>? Sources { get; set; }

        public State()
        {
        }

        public void Load()
        {
            Directory.CreateDirectory(ApplicationDirectory);

            if (!File.Exists(StateFilepath))
            {
                return;
            }

            var serialized = File.ReadAllText(StateFilepath);
            JsonConvert.PopulateObject(serialized, this);
        }

        public void Save()
        {
            Directory.CreateDirectory(ApplicationDirectory);

            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(StateFilepath, serialized);
        }
    }
}
