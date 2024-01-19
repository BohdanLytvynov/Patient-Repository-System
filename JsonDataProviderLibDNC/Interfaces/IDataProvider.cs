
namespace JsonDataProviderLibDNC.Interfaces
{
    public interface IDataProvider<DataProviderOperationTypes>
        where DataProviderOperationTypes : struct, Enum
    {
        Task SaveFileAsync(string path, object serObject,
            DataProviderOperationTypes jDataProviderOperation);

        Task LoadFileAsync(string path, DataProviderOperationTypes iDataProviderOperation);

        Task LoadFileAsync<ObjectType>(string path, ObjectType obj, DataProviderOperationTypes iDataProviderOperation);

        void IfFileNotExistsCreateIt(string path);

        void IfDirectoryNotExistsCreateIt(string path);

        void SaveFile(string path, object serObject,
            DataProviderOperationTypes jDataProviderOperation);

        void LoadFile(string path, DataProviderOperationTypes jDataProviderOperation);

        public void LoadFile<ObjectType>(string path, ObjectType obj,
            DataProviderOperationTypes jDataProviderOperation);
        
        string FileExtension { get; }
    }
}
