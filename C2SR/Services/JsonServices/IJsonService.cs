namespace C2SR.Services.JsonServices
{
    interface IJsonService<T>
    {
        public T Load(string fileName);
        public void Save(string fileName, T records);
    }
}
