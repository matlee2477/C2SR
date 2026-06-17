using System.IO;

namespace C2SR.Services.JsonServices
{
    class C2JsonService : IJsonService
    {
        public string LoadJson(string fileName)
        {
            using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(fs);
            string code = reader.ReadToEnd();
            return code;
        }

        public void SaveJson(string fileName, string code)
        {
            using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new(fs);
            writer.Write(code);
        }
    }
}
