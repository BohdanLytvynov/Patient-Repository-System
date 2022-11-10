using ControllerBaseLib;
using JsonDataProviderLibDNC.Enums;
using Newtonsoft.Json;

namespace JsonDataProviderLibDNC
    
{
    public class JsonDataProvider: ControllerBaseClass
    {
        #region Ctor
        public JsonDataProvider()
        {

        }
        #endregion

        #region Methods


        public async Task SaveDBAsync(string path, object serObject,
            JDataProviderOperation jDataProviderOperation)           
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<JDataProviderOperation>(
                jDataProviderOperation, 
                (state, cts)=>
                {
                    IfFIleNotExistsCreateIt(path);

                    string str = JsonConvert.SerializeObject(serObject, Formatting.None);

                    File.WriteAllText(path, str);

                    return null;
                }
                
                );
        }


        public async Task LoadDBAsync(string path, JDataProviderOperation jDataProviderOperation)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<JDataProviderOperation>(
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


        public async Task LoadDBAsync<ObjectType>(string path, ObjectType obj, JDataProviderOperation jDataProviderOperation)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<JDataProviderOperation>(
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

        public void IfFIleNotExistsCreateIt(string path)
        {
            if (!File.Exists(path))
            {
                var fs = File.Create(path);

                fs.Close();

                fs.Dispose();
            }
        }

        #endregion
    }
}