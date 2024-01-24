﻿using Models.Configuration.ReasonModels.ReasonStorageModel;
using static Models.Configuration.IntegratedData.Reasons;
using static Models.Configuration.IntegratedData.Physicians;
using static Models.Configuration.IntegratedData.Investigations;

namespace Models.Configuration
{
    public class ConfigStorage
    {        
        #region Event

        public event Func<bool, Task> OnConfigChanged;
        
        #endregion

        #region Properties

        public List<string> Physicians { get; set; }

        public List<ReasonStorageModel> Reasons { get; set; }

        public List<string> Investigations { get; set; }

        public string NotesReportOutput { get; set; }

        public string HistoryNotesReportOutput { get; set; }

        public string PathToViberPhoto { get; set; }

        public string PathToFailToReadPhotos { get; set; }

        public bool IsViberParserActive { get; set; }

        #endregion

        #region Ctor
        public ConfigStorage()
        {
            Physicians = new List<string>();
            
            Reasons = new List<ReasonStorageModel>();

            Investigations = new List<string>();

            NotesReportOutput = String.Empty;

            HistoryNotesReportOutput = String.Empty;

            PathToFailToReadPhotos = String.Empty;

            PathToViberPhoto = String.Empty;

            IsViberParserActive = false;
        }
        #endregion

        #region Methods

        public void ConfirmChanging(bool needRestart = true)
        {
            OnConfigChanged?.Invoke(needRestart);
        }

        public void UpdateIntegratedData()
        {
            ClearCollections(InvestProperty);

            ClearCollections(ReasonsProp);

            ClearCollections(DoctorsProp);

            foreach (var item in Investigations)
            {
                InvestProperty.Add(item);
            }

            foreach (var item in Physicians)
            {
                DoctorsProp.Add(item);
            }

            foreach (var item in Reasons)
            {
                ReasonsProp.Add(item.TextValue + " [" + item.Code + "]");

                if (item.DocDependent)
                {
                    CodeInsertionToPropriateList(item.Code, "DocDep");
                }

                if (item.DateDependent)
                {
                    CodeInsertionToPropriateList(item.Code, "DateDep");
                }               
            }            
        }

       
        private void CodeInsertionToPropriateList(int code, string keyname)
        {
            if (ConfigCodeUsageDictionary.ContainsKey(keyname)) //List with doc dependent indexes already exists
            {
                ConfigCodeUsageDictionary[keyname].Add(code);
            }
            else //Shoul be created
            {
                ConfigCodeUsageDictionary.Add(keyname, new List<int>() { code });
            }
        }

        private void ClearCollections<Type>(IList<Type> col)
        {
            if (col.Count > 0)
            {
                col.Clear();
            }
        }

        #endregion
    }
}
