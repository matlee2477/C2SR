namespace C2SR.Services.JsonServices
{
    interface IJsonService
    {
        public string LoadJson(string fileName);
        public void SaveJson(string fileName, string code);
    }
}
