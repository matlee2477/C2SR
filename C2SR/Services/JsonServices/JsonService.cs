using System.IO;

namespace C2SR.Services.JsonServices
{
    class JsonService : IJsonService
    {
        public string LoadJson(string fileName)
        {
            using StreamReader reader = File.OpenText(fileName);
            string code = reader.ReadToEnd();
            return code;
        }

        public void SaveJson(string fileName, string code)
        {
            using StreamWriter writer = File.CreateText(fileName);
            writer.Write(code);
        }
    }
}
