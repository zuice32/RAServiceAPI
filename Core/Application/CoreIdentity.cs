using System;
using System.IO;
using Newtonsoft.Json;

namespace Core.Application
{
    public class CoreIdentity : ICoreIdentity
    {
        public static string LoadedCoreDataDirectory { get; private set; }

        public static ICoreIdentity Load(string pathToCoreDataDirectory)
        {
            CoreIdentity coreIdentity;

            string pathToIdentityFile = Path.Combine(pathToCoreDataDirectory, "core.id");

            Directory.CreateDirectory(pathToCoreDataDirectory);

            if (!File.Exists(pathToIdentityFile))
            {
                coreIdentity = new CoreIdentity
                {
                    PathToCoreDataDirectory = pathToCoreDataDirectory,
                    ID = Guid.NewGuid().ToString("N")
                };

                string serializedIdentity = JsonConvert.SerializeObject(coreIdentity);

                using (TextWriter textWriter = new StreamWriter(pathToIdentityFile))
                {
                    textWriter.Write(serializedIdentity);
                }
            }
            else
            {
                string serializedIdentity;
                using (TextReader textReader = new StreamReader(pathToIdentityFile))
                {
                    serializedIdentity = textReader.ReadToEnd();
                }

                coreIdentity = JsonConvert.DeserializeObject<CoreIdentity>(serializedIdentity);
            }

            LoadedCoreDataDirectory = pathToCoreDataDirectory;

            return coreIdentity;
        }

        public string ID { get; set; }
        public string PathToCoreDataDirectory { get; set; }
    }
}