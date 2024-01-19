using ControllerBaseLib;
using JsonDataProviderLibDNC.Interfaces;
using Newtonsoft.Json;

namespace JsonDataProviderLibDNC    
{
    public class JsonDataProvider<DataProviderOperationTypes>: ControllerBaseClass<DataProviderOperationTypes>, IDataProvider<DataProviderOperationTypes>
        where DataProviderOperationTypes : struct, Enum
    {
        #region Fields
        private readonly string m_FileExtension;
        #endregion

        #region Properties

        public string FileExtension { get => m_FileExtension; }

        #endregion

        #region Ctor
        public JsonDataProvider()
        {
            m_FileExtension = "json";
        }
        #endregion

        #region Methods


        public async Task SaveFileAsync(string path, object serObject,
            DataProviderOperationTypes jDataProviderOperation)           
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                jDataProviderOperation, 
                (state, cts)=>
                {
                    IfFileNotExistsCreateIt(path);

                    string str = JsonConvert.SerializeObject(serObject, Formatting.None);

                    File.WriteAllText(path, str);

                    return null;
                }
                
                );
        }


        public async Task LoadFileAsync(string path, DataProviderOperationTypes jDataProviderOperation)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                jDataProviderOperation,
                (state, cts)=>
                {
                    string str = String.Empty;

                    object res = null;

                    if (File.Exists(path))
                    {
                        str = File.ReadAllText(path);

                        res = JsonConvert.DeserializeObject(str);
                    }

                    return res;
                }
                );
        }


        public async Task LoadFileAsync<ObjectType>(string path, ObjectType obj, 
            DataProviderOperationTypes jDataProviderOperation)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                jDataProviderOperation,
                (state, cts) =>
                {
                    string str = String.Empty;

                    object res = null;

                    if (File.Exists(path))
                    {
                        str = File.ReadAllText(path);

                        res = JsonConvert.DeserializeAnonymousType<ObjectType>(str, obj);
                    }

                    return res;
                }
                );
        }

        public void SaveFile(string path, object serObject,
            DataProviderOperationTypes jDataProviderOperation)
        {
             ExecuteFunctionAndGetResultThroughEvent(
                jDataProviderOperation,
                (state) =>
                {
                    IfFileNotExistsCreateIt(path);

                    string str = JsonConvert.SerializeObject(serObject, Formatting.None);

                    File.WriteAllText(path, str);

                    return null;
                }

                );
        }

        public void LoadFile(string path, DataProviderOperationTypes jDataProviderOperation)
        {
            ExecuteFunctionAndGetResultThroughEvent(
                jDataProviderOperation,
                (state) =>
                {
                    string str = String.Empty;

                    object res = null;

                    if (File.Exists(path))
                    {
                        str = File.ReadAllText(path);

                        res = JsonConvert.DeserializeObject(str);
                    }

                    return res;
                }
                );
        }

        public void LoadFile<ObjectType>(string path, ObjectType obj,
            DataProviderOperationTypes jDataProviderOperation)
        {
            ExecuteFunctionAndGetResultThroughEvent(
                jDataProviderOperation,
                (state) =>
                {
                    string str = String.Empty;

                    object res = null;

                    if (File.Exists(path))
                    {
                        str = File.ReadAllText(path);

                        res = JsonConvert.DeserializeAnonymousType<ObjectType>(str, obj);
                    }

                    return res;
                }
                );
        }

        public void IfFileNotExistsCreateIt(string path)
        {
            if (!File.Exists(path))
            {
                var fs = File.Create(path);

                fs.Close();

                fs.Dispose();
            }
        }
       
        public void IfDirectoryNotExistsCreateIt(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}