#region Usings

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLib.VM;
using PatientRep.ViewModelBase.Commands;
using DataValidation;
using CRUDControllerLib.Enums;
using CRUDControllerLib.PatientController;
using ControllerBaseLib.Enums;
using ControllerBaseLib.EventArgs;
using System.IO;
using System.Windows;
using CRUDControllerLib.SearchArgs;
using Newtonsoft.Json.Linq;
using Models.PatientModel.Enums;
using Models.Comparators;
using CRUDControllerLib.HistoryNotesController;
using Models.Interfaces;
using PatientRep.Views;
using ReportBuilderLib.Enums;
using Models.HistoryNoteModels.VisualModel;
using Models.HistoryNoteModels.StorageModel;
using Models.PatientModel.PatientVisualModel;
using Models.PatientModel.PatientStorageModel;
using Models.Configuration;
using static Models.Configuration.IntegratedData.Reasons;
using Models.ExportNoteModel;
using NotesExporterLib;
using System.Windows.Forms;
using Models.PatientModel.Comparators;
using AdditionalControllersLib;
using SignalizationSystemLib;
using SmartParser.Parsers;
using SmartParser;
using SmartParser.Dependencies;
using System.Text.RegularExpressions;
using IronOcr;
using System.Diagnostics;
using PatientRep.Enums;
using JsonDataProviderLibDNC;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;


#endregion

namespace PatientRep.ViewModels
{
    public class MainWindowViewModel : ViewModelBaseClass
    {
        #region Global Vars

        bool ENABLE_OCR_DEBUG;

        #endregion

        #region Constants

        private const int ONE_MINUTE_TOMILLISECOND_MULTIPL = 60000;

        #endregion

        #region Tasks

        Task m_CheckViber;

        CancellationTokenSource m_cts_for_all_Tasks;

        #endregion

        #region Regular Expretions

        readonly Regex m_codeReg;

        readonly Regex m_Name_or_Surename_or_Lastname;

        readonly Regex m_Surename_Name;

        readonly Regex m_Surename_Name_Lastname;

        readonly Regex m_ElnRefWithText;

        #endregion

        #region Events

        public event Func<Task> OnMainWindowInitialized;
        
        #endregion

        #region Windows

        ReportViewer? m_ReportViewerWindow;

        SettingsWindow? m_SettingsWindow;

        #endregion

        #region Fields

        #region Current Window

        Window m_currrentWindow;

        #endregion

        #region Viber Parser
       
        ViberParser m_ViberParser;

        bool m_UI_is_UsedbyViber_Parser;

        #endregion

        #region Additional Controllers 

        ReasonsManager m_ReasonManager;

        UIElementManager m_UIElementManager;

        #region SignalSystemController


        #endregion

        #endregion

        string m_selfpath;

        string m_PathToConfig;

        ConfigStorage? m_Configuration;

        #region Informator System

        string m_msg;//Information system Message

        OperStatus m_OperStatus;//Operation Status

        #endregion

        #region History Registration System

        int m_HistoryNotesCount;

        string m_PatientName;

        string m_PatientSurename;

        string m_PatientLastname;

        string m_center;

        string m_Department;

        DateTime m_HistoryRegistrationDate;

        bool m_IsDateCorrect;

        string m_Physician;

        string m_Reason;

        ObservableCollection<AdditionalInfoViewModel> m_HistoryRegistrationAddInfoCollection;

        private List<HistoryNoteStorage>? m_HistoryNotesStorageCollection;

        ObservableCollection<HistoryNote> m_HistoryNoteVisualModelCollection;

        int m_HistoryRegistrationSelectedIndex;

        bool m_ClearDate;

        bool m_IsDirExists;

        DateTime m_HospitalDateTime;

        bool m_ViewRegister;

        string m_InvestType;

        #region History Notes Controller

        HistoryNotesController m_HistoryNotesController;

        #endregion

        #region Visibility

        Visibility m_CaseRegisterHistory;

        Visibility m_CaseSearchHistory;

        Visibility m_PhysicianVisibility;

        #endregion

        #endregion

        #region Report System

        NotesExporterToTxt m_NoteExporterToTxt;

        bool m_SearchReport; // search - true Report - false

        DateTime m_NoteSearchStart;

        DateTime m_NoteSearchEnd;

        ReportType m_RepType;

        #region Visibility

        Visibility m_CaseSearchNotesVisibility;

        Visibility m_CaseReportSystemVisibility;

        #endregion

        #endregion

        int m_NoteCount;

        #region SearchFields

        string m_SearchSurename;

        string m_SearchCode;

        DateTime m_SearchDateStart;

        DateTime m_SearchDateEnd;

        bool m_SearchCodeCorrect;

        #endregion

        #region Visibility fields

        Visibility m_SurSearchGridVisibility;

        Visibility m_CodeSearchGridVisibility;

        Visibility m_DateSearchGridVisibility;

        Visibility m_StatusSearchGridVisibility;

        Visibility m_DateStatusSearchGridVisibility;

        #endregion

        string m_tittle;

        #region DataProvider

        JsonDataProvider<PatientRepDataProviderOperations> m_jdataprovider;

        #endregion

        #region Controllers

        PatientController m_pController;

        #endregion

        #region NewPatient

        string m_Surename;

        string m_name;

        string m_lastname;

        string m_code;

        string m_diagnosis;

        string m_pathToPatientsDB;

        string m_pathToHistoryDB;

        bool m_IsInputCodeCorrect;

        bool m_FocusName;

        ObservableCollection<AdditionalInfoViewModel> m_NewAddInfoCol;

        int m_AddInfoSelectedIndex;

        int mod = 4;

        DateTime m_NewRegisterDate;

        #endregion

        SearchCondition m_searchCondition;

        StringCoincidence m_StringCoincidence;

        PatientStatus m_SearchStatus;

        #region PatientsNotesCollection

        List<PatientStorage>? m_patients;

        ObservableCollection<Patient> m_SearchResult;

        #endregion

        #endregion

        #region Properties

        #region Informator System

        public string Text
        { get => m_msg; set => Set(ref m_msg, value, nameof(Text)); }

        public OperStatus OperStatus
        {
            get => m_OperStatus;
            set => Set(ref m_OperStatus, value, nameof(OperStatus));
        }

        #endregion

        #region History Registration System

        public int HistoryNotesCount
        {
            get => m_HistoryNotesCount;
            set => Set(ref m_HistoryNotesCount, value, nameof(HistoryNotesCount));
        }

        public string PatientSurename
        { get => m_PatientSurename; set => Set(ref m_PatientSurename, value, nameof(PatientSurename)); }

        public string PatientName
        { get => m_PatientName; set => Set(ref m_PatientName, value, nameof(PatientName)); }

        public string PatientLastName
        { get => m_PatientLastname; set => Set(ref m_PatientLastname, value, nameof(PatientLastName)); }

        public string Center
        { get => m_center; set => Set(ref m_center, value, nameof(Center)); }

        public string Department
        { get => m_Department; set => Set(ref m_Department, value, nameof(Department)); }

        public DateTime HistoryRegistrationDate
        {
            get => m_HistoryRegistrationDate;

            set => Set(ref m_HistoryRegistrationDate, value, nameof(HistoryRegistrationDate));
        }

        public bool IsDateCorrect
        {
            get => m_IsDateCorrect;
            set
            {
                Set(ref m_IsDateCorrect, value, nameof(IsDateCorrect));

                m_ValidationArray[11] = IsDateCorrect;
            }
        }

        public string Physician
        {
            get => m_Physician;
            set => Set(ref m_Physician, value, nameof(Physician));
        }

        public string Reason
        {
            get => m_Reason;
            set
            {
                Set(ref m_Reason, value, nameof(Reason));

                m_UIElementManager.SetVisibilityOfUIElementAccordingToReason(Reason, ConfigCodeUsageDictionary, "DocDep", GetCode);

                if (!String.IsNullOrWhiteSpace(Reason))
                {
                    m_ValidationArray[12] = true;
                }
                else
                {
                    m_ValidationArray[12] = false;
                }
            }
        }

        public ObservableCollection<AdditionalInfoViewModel> HistoryRegistrationAddInfoCollection
        {
            get => m_HistoryRegistrationAddInfoCollection;
            set => m_HistoryRegistrationAddInfoCollection = value;
        }

        public ObservableCollection<HistoryNote> HistoryNotesVisualModelCollection
        {
            get => m_HistoryNoteVisualModelCollection;

            set => m_HistoryNoteVisualModelCollection = value;
        }

        public int HistoryRegistrationSelectedIndex
        {
            get => m_HistoryRegistrationSelectedIndex;
            set => Set(ref m_HistoryRegistrationSelectedIndex, value, nameof(HistoryRegistrationSelectedIndex));
        }

        public bool ClearDate
        {
            get => m_ClearDate;

            set => Set(ref m_ClearDate, value, nameof(ClearDate));
        }

        public DateTime HospitalDateTime
        {
            get => m_HospitalDateTime;
            set => Set(ref m_HospitalDateTime, value, nameof(HospitalDateTime));
        }

        public bool IsDirExists
        {
            get => m_IsDirExists;

            set
            {
                Set(ref m_IsDirExists, value, nameof(IsDirExists));

                m_ReasonManager.GetReasonAccordingToExistanceOfDirection(IsDirExists, ConfigCodeUsageDictionary,
                    "DateDep", GetReasonAccordingToCode);
            }

        }

        public string InvestType
        {
            get => m_InvestType;
            set => Set(ref m_InvestType, value, nameof(InvestType));
        }


        #region Visibility

        public Visibility CaseRegisterHistory
        { get => m_CaseRegisterHistory; set => Set(ref m_CaseRegisterHistory, value, nameof(CaseRegisterHistory)); }

        public Visibility CaseSearchHistory
        { get => m_CaseSearchHistory; set => Set(ref m_CaseSearchHistory, value, nameof(CaseSearchHistory)); }

        public Visibility PhysicianVisibility
        {
            get => m_PhysicianVisibility;

            set => Set(ref m_PhysicianVisibility, value, nameof(PhysicianVisibility));
        }

        #endregion

        #endregion

        #region Report System

        #region Visibility
        public Visibility CaseSearchNotesVisibility
        { get => m_CaseSearchNotesVisibility; set => Set(ref m_CaseSearchNotesVisibility, value, nameof(CaseSearchNotesVisibility)); }

        public Visibility CaseReportSystemVisibility
        { get => m_CaseReportSystemVisibility; set => Set(ref m_CaseReportSystemVisibility, value, nameof(CaseReportSystemVisibility)); }

        #endregion

        public DateTime NoteSearchStart
        { get => m_NoteSearchStart; set => Set(ref m_NoteSearchStart, value, nameof(NoteSearchStart)); }

        public DateTime NoteSearchEnd
        { get => m_NoteSearchEnd; set => Set(ref m_NoteSearchEnd, value, nameof(NoteSearchEnd)); }

        public ReportType RepType
        { get => m_RepType; set => Set(ref m_RepType, value, nameof(RepType)); }

        #endregion

        public int NoteCount
        {
            get => m_NoteCount;
            set => Set<int>(ref m_NoteCount, value, nameof(NoteCount));
        }

        #region SerachProperties

        public string SearchSurename
        {
            get => m_SearchSurename;
            set => Set<string>(ref m_SearchSurename, value, nameof(SearchSurename));
        }

        public string SearchCode
        {
            get => m_SearchCode;

            set => Set<string>(ref m_SearchCode, value, nameof(SearchCode));
        }

        public DateTime DateSearchStart
        {
            get => m_SearchDateStart;
            set => Set<DateTime>(ref m_SearchDateStart, value, nameof(DateSearchStart));
        }

        public DateTime DateSearchEnd
        {
            get => m_SearchDateEnd;
            set => Set<DateTime>(ref m_SearchDateEnd, value, nameof(DateSearchEnd));
        }

        public bool IsSerchCodeCorrect
        {
            get => m_SearchCodeCorrect;

            set
            {
                Set<bool>(ref m_SearchCodeCorrect, value, nameof(IsSerchCodeCorrect));

                m_ValidationArray[5] = IsSerchCodeCorrect;

                OnSearchButtonPressed.CanExecute(null);
            }
        }

        #endregion

        #region New Patient

        public bool FocusName
        { get => m_FocusName; set => Set(ref m_FocusName, value, nameof(FocusName)); }

        public int SelectedAddInfoIndex
        {
            get => m_AddInfoSelectedIndex;
            set => Set<int>(ref m_AddInfoSelectedIndex, value, nameof(SelectedAddInfoIndex));
        }

        public string Surename { get => m_Surename; set => Set<string>(ref m_Surename, value, nameof(Surename)); }

        public string Name { get => m_name; set => Set<string>(ref m_name, value, nameof(Name)); }

        public string Lastname { get => m_lastname; set => Set<string>(ref m_lastname, value, nameof(Lastname)); }

        public string Code
        {
            get => m_code;
            set
            {
                Set<string>(ref m_code, value, nameof(Code));
            }
        }

        public bool IsInputCodeCorrect
        {
            get => m_IsInputCodeCorrect;

            set
            {
                Set<bool>(ref m_IsInputCodeCorrect, value, nameof(IsInputCodeCorrect));

                m_ValidationArray[3] = IsInputCodeCorrect;

                OnAddNewPatientButtonPressed.CanExecute(null);
            }

        }

        public ObservableCollection<AdditionalInfoViewModel> AddInfoCol
        {
            get => m_NewAddInfoCol; set => m_NewAddInfoCol = value;
        }


        public string Diagnosis { get => m_diagnosis; set => Set<string>(ref m_diagnosis, value, nameof(Diagnosis)); }

        public DateTime NewRegisterDate
        {
            get => m_NewRegisterDate;
            set
            {
                Set<DateTime>(ref m_NewRegisterDate, value, nameof(NewRegisterDate));
            }

        }

        #endregion

        #region Visibility Properties

        public Visibility StatusSearchGridVisibility
        {
            get => m_StatusSearchGridVisibility;
            set => Set<Visibility>(ref m_StatusSearchGridVisibility, value, nameof(StatusSearchGridVisibility));
        }

        public Visibility SurSearcherGridVisibility
        {
            get => m_SurSearchGridVisibility;
            set => Set<Visibility>(ref m_SurSearchGridVisibility, value, nameof(SurSearcherGridVisibility));
        }

        public Visibility CodeSearchGridVisibility
        {
            get => m_CodeSearchGridVisibility;
            set => Set<Visibility>(ref m_CodeSearchGridVisibility, value, nameof(CodeSearchGridVisibility));
        }

        public Visibility DateSearchGridVisibility
        {
            get => m_DateSearchGridVisibility;
            set => Set<Visibility>(ref m_DateSearchGridVisibility, value, nameof(DateSearchGridVisibility));
        }

        public Visibility DateStatusSearchGridVisibility
        {
            get => m_DateStatusSearchGridVisibility;
            set => Set<Visibility>(ref m_DateStatusSearchGridVisibility, value, nameof(DateStatusSearchGridVisibility));
        }

        #endregion

        public PatientStatus SearchStatus
        {
            get => m_SearchStatus;
            set => Set<PatientStatus>(ref m_SearchStatus, value, nameof(SearchStatus));
        }

        public SearchCondition SearchCon
        {
            get => m_searchCondition;
            set
            {
                Set<SearchCondition>(ref m_searchCondition, value, nameof(SearchCon));

                SetGridVisibility();
            }
        }

        public StringCoincidence StrCoincidence
        {
            get => m_StringCoincidence;
            set => Set<StringCoincidence>(ref m_StringCoincidence, value, nameof(StrCoincidence));
        }

        public ObservableCollection<Patient> SearchResult
        {
            get => m_SearchResult;
            set => m_SearchResult = value;
        }

        #region IDataErrorInfo

        public override string this[string columnName]
        // 3 (Input Code) - controlls Input Smart Code Input
        // 5(Serach Code) - controlls Smart Code Input
        //11 (Is Hosp Date is Coorect)
        //12 (is Reason is Correct)
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(Surename):

                        m_ValidationArray[0] = Validation.ValidateText(Surename, Validation.Restricted, out error);

                        return error;

                    case nameof(Name):

                        m_ValidationArray[1] = Validation.ValidateText(Name, Validation.Restricted, out error);

                        return error;

                    case nameof(Lastname):

                        m_ValidationArray[2] = Validation.ValidateText(Lastname, Validation.Restricted, out error);

                        return error;

                    //3

                    case nameof(SearchSurename):

                        m_ValidationArray[4] = Validation.ValidateText(SearchSurename, Validation.Restricted, out error);

                        return error;

                    //5

                    case nameof(PatientSurename):

                        m_ValidationArray[6] = Validation.ValidateText(PatientSurename, Validation.Restricted, out error);

                        return error;

                    case nameof(PatientName):

                        m_ValidationArray[7] = Validation.ValidateText(PatientName, Validation.Restricted, out error, true);

                        return error;

                    case nameof(PatientLastName):

                        m_ValidationArray[8] = Validation.ValidateText(PatientLastName, Validation.Restricted, out error, true);

                        return error;

                    case nameof(Center):

                        m_ValidationArray[9] = Validation.ValidateNumber(Center, out error);

                        return error;

                    case nameof(Department):

                        m_ValidationArray[10] = Validation.ValidateNumber(Department, out error, true);

                        return error;

                        //11

                        //12
                }

                return error;
            }
        }

        #endregion

        #region Commands

        #region Patient Rep

        public ICommand OnAddNewPatientButtonPressed { get; }

        public ICommand OnClearFialdsButtonPressed { get; }

        public ICommand OnClearSearchFieldsPressed { get; }

        public ICommand OnSearchButtonPressed { get; }

        public ICommand OnGetAllNotesPressed { get; }

        public ICommand OnSortByDateButtonPressed { get; }

        public ICommand OnSortByStatusButtonPressed { get; }

        public ICommand OnSortByCenterButtonPressed { get; }

        public ICommand OnAddNewAddInfoNotePressed { get; }

        public ICommand OnRemoveAddInfoButtonPressed { get; }

        public ICommand OnSettingsButtonPressed { get; }

        #endregion

        #region History Registration System

        public ICommand OnAddNewAddInfoHistoryNote { get; }

        public ICommand OnRemoveNewAddInfoHistoryNote { get; }

        public ICommand OnClearAllNewAddInfoHistoryNotes { get; }

        public ICommand OnAddNewHistoryNote { get; }

        public ICommand RegisterSystemClearAllFields { get; }

        public ICommand ViewHistorryNotes { get; }

        public ICommand RegisterHistoryNotes { get; }

        public ICommand GetAllHistoryNotes { get; }

        #endregion

        #region ReportSystem

        public ICommand OnSearchConditionsButtonPressed { get; }

        public ICommand OnReportConditionsButtonPressed { get; }

        public ICommand OnSearchHistoryNotesButtonPressed { get; }

        public ICommand OnCreateReportButtonPressed { get; }

        #endregion

        #region Export System

        public ICommand OnExportNotesButtonPressed { get; set; }

        #endregion

        #endregion

        #endregion

        #region Ctor

        public MainWindowViewModel(Window thisWindow)
        {      
            //ConfigCodeUsageDictionary Manager will be used in future for connection to DB file

            #region Init Tasks

            m_cts_for_all_Tasks = new CancellationTokenSource();

            m_CheckViber = new Task(() => 
            {
                for ( ; ; )
                {
                    DirectoryInfo d_info = new DirectoryInfo(m_Configuration.PathToViberPhoto);

                    var images = d_info.GetFiles();

                    if (m_ViberParser.TempData.CurrentImagesCount < images.Length)
                    {
                        m_ViberParser.ParseImages(images);
                    }

                    if (m_cts_for_all_Tasks.IsCancellationRequested)
                    {
                        break;
                    }

                    //Thread.Sleep(ONE_MINUTE_TOMILLISECOND_MULTIPL * 1);
                }

                
            },m_cts_for_all_Tasks.Token);

            #endregion

            #region OCR Configs

            var configuration1 = new TesseractConfiguration()
            {
                ReadBarCodes = true,
                BlackListCharacters = "`ë|^",
                RenderSearchablePdfsAndHocr = true,
                PageSegmentationMode = TesseractPageSegmentationMode.AutoOsd,

            };

            #endregion

            #region Init Fields

            m_UI_is_UsedbyViber_Parser = false;

            m_currrentWindow = thisWindow;

            m_codeReg = new Regex(@"^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}");

            m_Name_or_Surename_or_Lastname = new Regex(@"^[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{0,}");

            m_Surename_Name = new Regex(@"^[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{1,}[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{0,}");

            m_Surename_Name_Lastname =
                new Regex(@"^[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{1,}[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{1,}[А-ЯІЇЄҐ]{1}[а-яіїєґ']{0,}\s{0,}");

            m_ElnRefWithText =
                new Regex(@"^[а - яіїєґ'/|.А-ЯІЇЄҐ\s\S]{0,}[0-9]{4}[-.][0-9]{4}[-.][0-9]{4}[-.][0-9]{4}\s{0,}[а-яіїєґ'/|.А - ЯІЇЄҐ\s\S]{0,}");

            m_ViberParser = new ViberParser(
                new OCRResultParser(m_codeReg, m_Surename_Name_Lastname,
                m_Surename_Name, m_Name_or_Surename_or_Lastname, m_ElnRefWithText,
                new string[] { "Перевірено", "Профіль",
                "Зв'язки", "Страхування", "Призначення", "Активне", "Нове", "Пріоритет", "Планове", "Категорія",  "Програма", 
                "Детальніше"}), new OCR(configuration1),
                new JsonDataProvider<ViberParserDataProviderOperations>());

            m_ViberParser.OnOperationFinished += M_ViberParser_OnOperationFinished;

            m_msg = String.Empty;

            m_OperStatus = OperStatus.NoOperation;

            m_ReasonManager = new ReasonsManager();

            m_UIElementManager = new UIElementManager();

            m_NoteExporterToTxt = new NotesExporterToTxt();

            m_ReasonManager.OnOperationFinished += M_ReasonManager_OnOperationFinished;

            m_UIElementManager.OnOperationFinished += M_UIElementManager_OnOperationFinished;

            m_NoteExporterToTxt.OnOperationFinished += M_NoteExporterToTxt_OnOperationFinished;

            m_NewAddInfoCol = new ObservableCollection<AdditionalInfoViewModel>();

            m_selfpath = Environment.CurrentDirectory;

            m_PathToConfig = m_selfpath + Path.DirectorySeparatorChar + "Configuration" +
               Path.DirectorySeparatorChar + "Config.json";

            m_RepType = ReportType.По_Денний;

            m_InvestType = String.Empty;

            m_NoteSearchStart = DateTime.Now;

            m_NoteSearchEnd = DateTime.Now;

            m_CaseSearchNotesVisibility = Visibility.Visible;

            m_CaseReportSystemVisibility = Visibility.Hidden;

            m_SearchReport = true;

            m_HistoryRegistrationSelectedIndex = -1;

            m_AddInfoSelectedIndex = -1;

            m_HistoryRegistrationAddInfoCollection = new ObservableCollection<AdditionalInfoViewModel>();

            m_HistoryNoteVisualModelCollection = new ObservableCollection<HistoryNote>();

            m_NewRegisterDate = DateTime.Now;

            m_HistoryRegistrationDate = DateTime.Now;

            m_SearchDateStart = DateTime.Now;

            m_SearchDateEnd = DateTime.Now;

            m_SearchStatus = PatientStatus.Не_Погашено;

            m_SearchSurename = String.Empty;

            m_SearchCode = String.Empty;

            m_SurSearchGridVisibility = Visibility.Visible;

            m_DateSearchGridVisibility = Visibility.Hidden;

            m_CodeSearchGridVisibility = Visibility.Hidden;

            m_StatusSearchGridVisibility = Visibility.Hidden;

            m_DateStatusSearchGridVisibility = Visibility.Hidden;

            m_searchCondition = SearchCondition.Пошук_по_Прізвищу;

            m_StringCoincidence = StringCoincidence.Часткове;

            m_tittle = "Patient Repository Storage";

            m_jdataprovider = new JsonDataProvider<PatientRepDataProviderOperations>();

            m_pController = new PatientController();

            m_HistoryNotesController = new HistoryNotesController();

            m_HistoryNotesController.OnOperationFinished += M_HistoryNotesController_OnOperationFinished;

            m_pController.OnOperationFinished += M_pController_OnOperationFinished;

            m_jdataprovider.OnOperationFinished += M_jdataprovider_OnOperationFinished;

            m_pathToPatientsDB = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DB" + Path.DirectorySeparatorChar + "Rep.json";

            m_pathToHistoryDB = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DB" + Path.DirectorySeparatorChar + "HRep.json";

            m_SearchResult = new ObservableCollection<Patient>();

            m_ValidationArray = new bool[13];

            m_Surename = String.Empty;

            m_name = String.Empty;

            m_lastname = String.Empty;

            m_code = String.Empty;

            m_diagnosis = String.Empty;

            m_PatientSurename = String.Empty;

            m_PatientName = String.Empty;

            m_PatientLastname = String.Empty;

            m_center = String.Empty;

            m_Department = String.Empty;

            m_Reason = String.Empty;

            m_Physician = String.Empty;

            m_PhysicianVisibility = Visibility.Hidden;

            m_ViewRegister = false;

            m_CaseRegisterHistory = Visibility.Visible;

            m_CaseSearchHistory = Visibility.Hidden;

            #endregion

            #region Init commands

            OnAddNewPatientButtonPressed = new LambdaCommand(
                OnAddNewPatientButtonPressedExecute,
                CanOnAddNewPatientButtonPressedExecute
                );

            OnClearFialdsButtonPressed = new LambdaCommand(
                OnClearFieldsButtonPressedExecute,
                CanOnClerFieldsButtonPressedExecute
                );

            OnClearSearchFieldsPressed = new LambdaCommand(
                OnClearFieldsSearchButtonPressedExecute,
                CanOnClearFieldsSearchButtonPressedExecute
                );

            OnSearchButtonPressed = new LambdaCommand(
                OnSearchButtonPressedExecute,
                CanOnSearchButtonPressedExecute
                );

            OnGetAllNotesPressed = new LambdaCommand(
                OnGetAllNotesButtonPressedExecute,
                CanOnGetAllNotesButtonPressedExecute
                );

            OnSortByDateButtonPressed = new LambdaCommand(
                OnSortByDateButtonPressedExecute,
                CanOnSortByDateButtonPressedExecute
                );

            OnSortByStatusButtonPressed = new LambdaCommand(
                OnSortByStatusButtonPressedExecute,
                CanOnSortByStatusButtonPressedExecute

                );

            OnSortByCenterButtonPressed = new LambdaCommand(
                OnSortByCenterButtonPressedExecute,
                CanOnSortByCenterButtonPressedExecute
                );

            OnAddNewAddInfoNotePressed = new LambdaCommand(
                OnAddNewAddInfoButtonPressedExecute,
                CanOnAddNewAddInfoButtonPressedExecute
                );

            OnRemoveAddInfoButtonPressed = new LambdaCommand(
                OnRemoveAddInfoButtonPressedExecute,
                CanOnRemoveAddInfoButtonPressedExecute
                );

            OnSettingsButtonPressed = new LambdaCommand
                (
                    OnSettingsButtonPressedExecute,
                    CanOnSettingsButtonPressedExecute
                );

            OnExportNotesButtonPressed = new LambdaCommand
                (
                    OnExportNotesButtonPressedExecute,
                    CanOnExportNotesButtonPressedExecute
                );

            #region History Registration System

            OnAddNewAddInfoHistoryNote = new LambdaCommand(
                OnAddNewNewAddInfoHistoryNoteExecute,
                CanOnAddNewAddInfoHistoryNoteExecute
                );

            OnRemoveNewAddInfoHistoryNote = new LambdaCommand(
                OnRemoveNewNewAddInfoHistoryNoteExecute,
                CanOnRemoveNewNewAddInfoHistoryNoteExecute
                );

            OnClearAllNewAddInfoHistoryNotes = new LambdaCommand(
                OnRemoveAllNewNewAddInfoHistoryNotesExecute,
                CanOnRemoveAllNewNewAddInfoHistoryNotesExecute
                );

            OnAddNewHistoryNote = new LambdaCommand(
                OnAddNewHistoryNoteExecute,
                CanOnAddNewHistoryNoteExecute
                );

            RegisterSystemClearAllFields = new LambdaCommand(
                OnHistoryRegisterClearFieldsExecute,
                CanOnHistoryRegisterClearFieldsExecute
                );

            RegisterHistoryNotes = new LambdaCommand(
                OnRegisterHistoryNotesExecute,
                CanOnRegisterHistoryNotesExecute
                );

            ViewHistorryNotes = new LambdaCommand(
                OnViewHistoryButtonPressedExecute,
                CanOnViewHistoryButtonPressedExecute
                );

            GetAllHistoryNotes = new LambdaCommand(
                OnGetAllHistoryNotesPressed,
                CanOnGetAllNotesButtonPressedExecute
                );

            #endregion

            #region Report System

            OnSearchConditionsButtonPressed = new LambdaCommand
                (
                    OnSearchConditionsButtonPressedExecute,
                    CanOnSearchConditionsButtonPressedExecute
                );

            OnReportConditionsButtonPressed = new LambdaCommand
                (
                    OnReportConditionsButtonPressedExcute,
                    CanOnReportConditionsButtonPressedExcute
                );

            OnSearchHistoryNotesButtonPressed = new LambdaCommand
                (
                    OnSearchHistoryNotesButtonPressedExecute,
                    CanOnSearchHistoryNotesButtonPressedExecute
                );

            OnCreateReportButtonPressed = new LambdaCommand
                (
                    OnCreateReportButtonPressedExecute,
                    CanOnCreateReportButtonPressedExecute
                );

            #endregion

            #endregion

            OnMainWindowInitialized += MainWindowViewModel_OnMainWindowInitialized;

            OnMainWindowInitialized.Invoke();
        }

        private void M_ViberParser_OnOperationFinished(object s, OperationFinishedEventArgs<ViberParserOperations> e)
        {            
            var r = (e.Result as ViberParserResult);

            if (r == null)
            {
                return;
            }

        #if DEBUG
            Debug.WriteLine(r.ToString());
        #endif

            var temp = r.SuccessfullyRead.ToArray();
            
            //Even One Data Must Be found
            if (!(String.IsNullOrEmpty(temp[0]) || String.IsNullOrEmpty(temp[1]) || String.IsNullOrEmpty(temp[2]) 
                || String.IsNullOrEmpty(temp[3])) )
            {
                m_UI_is_UsedbyViber_Parser = true;

                var patient = new PatientStorage(Guid.NewGuid(), temp[0] == null ? String.Empty : temp[0],
                temp[1] == null ? String.Empty : temp[1],
                temp[2] == null ? String.Empty : temp[2],
                temp[3] == null ? String.Empty : temp[3],
                String.Empty,
                PatientStatus.Не_Погашено, DateTime.Now, new DateTime(), null, String.Empty);

                m_pController.AddAsync(patient, m_patients);

                m_currrentWindow.Dispatcher.Invoke(() =>
                {
                    var temp = patient.StorageToVisualModel();

                    temp.Number = SearchResult.Count + 1;

                    SearchResult.Add(temp);
                });

                m_UI_is_UsedbyViber_Parser = false;
            }
                      
            //Move image to FailToReadPaths_Storage
            if (r.Fail)
            {
                var fs = File.Open(r.FailedToReadPaths, FileMode.Open);

                var last = r.FailedToReadPaths.Split('\\');

                var ss = File.Create(m_Configuration.PathToFailToReadPhotos +
                    Path.DirectorySeparatorChar + last[last.Length - 1]);

                fs.CopyTo(ss);

                fs.Close();

                fs.Dispose();

                ss.Close();

                ss.Dispose();

                File.Delete(r.FailedToReadPaths);
            }
        }

        private async Task M_Configuration_OnConfigChanged()
        {
            await m_jdataprovider.SaveFileAsync(m_PathToConfig, m_Configuration, PatientRepDataProviderOperations.SaveSettings);

            var r = UIMessaging.CreateMessageBox("Налаштування додатку були успішно збережені! Але потрібно перезапустити додаток щоб оновити потрібні Комбо-Бокси!" +
                "Якщо ви зробили всі потрібні вам налаштування тисніть - ОК, якщо ні, то доробіть, та перезапускайтесь! :)",
               m_tittle, MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (r == MessageBoxResult.OK)
            {
                System.Windows.Application.Current.Shutdown(0);
            }
        }

        private async Task MainWindowViewModel_OnMainWindowInitialized()
        {
            await m_jdataprovider.LoadFileAsync(m_pathToPatientsDB, PatientRepDataProviderOperations.LoadPatientsDB);

            await m_jdataprovider.LoadFileAsync(m_pathToHistoryDB, PatientRepDataProviderOperations.LoadHistoryNotesDb);

            await m_jdataprovider.LoadFileAsync<ConfigStorage>(m_PathToConfig, m_Configuration, PatientRepDataProviderOperations.LoadSettings);
            
            if (!Directory.Exists(m_Configuration.PathToViberPhoto))
            {
                UIMessaging.CreateMessageBox("Сталася помилка при знаходженні папки з вайбер мультимедія! Перевірте налаштування шляхів системи зчитування з вайбера.",
                    m_tittle,
                    MessageBoxButton.OK, MessageBoxImage.Error);                              
            }
            else
            {                
                m_CheckViber.Start();
            }
        }

        #region Other Additional Methods

        private void UseSignalSystem<TControllerOperation>(OperationFinishedEventArgs<TControllerOperation> e)
            where TControllerOperation : struct, Enum
        {
            Text = $"Operation: {e.OperationType}";

            switch (e.ExecutionStatus)
            {
                case Status.Succed:

                    OperStatus = OperStatus.Ok;

                    break;
                case Status.Failed:

                    OperStatus = OperStatus.Failed;

                    break;
            }

            OperStatus = OperStatus.NoOperation;
        }

        #endregion

        #region HistoryNotesEvents
        private async void Hn_OnRemoveNote(HistoryNote obj)
        {
            await m_HistoryNotesController.RemoveAsync(obj, m_HistoryNotesStorageCollection);
        }

        private async void Hn_OnSaveNotes(HistoryNote obj)
        {
            await m_HistoryNotesController.EditAsync(obj, m_HistoryNotesStorageCollection);
        }

        #endregion

        #region ControllerEventsHandlers

        private void M_UIElementManager_OnOperationFinished(object s, OperationFinishedEventArgs<UIElementManagerOperations> e)
        {
            UIMessaging.CreateMessageBoxAccordingToResult(e, m_tittle, () =>
            {
                switch (e.OperationType)
                {
                    case UIElementManagerOperations.SetVisibilityOfDoctorsPropertyAccordingToReason:

                        PhysicianVisibility = e.Result;

                        break;
                }
            });
        }

        private void M_ReasonManager_OnOperationFinished(object s, OperationFinishedEventArgs<ReasonsManagerOperations> e)
        {
            UIMessaging.CreateMessageBoxAccordingToResult(e, m_tittle, () =>
            {
                switch (e.OperationType)
                {
                    case ReasonsManagerOperations.GetReasonIfDirDoesntExists:

                        Reason = e.Result;

                        break;
                }
            });
        }

        private void M_NoteExporterToTxt_OnOperationFinished(object s, OperationFinishedEventArgs<NotesExporterToTxtOperations> e)
        {
            UIMessaging.CreateMessageBoxAccordingToResult(e, m_tittle, () =>
            {
                switch (e.OperationType)
                {
                    case NotesExporterToTxtOperations.ExportNotes:

                        var r = UIMessaging.CreateMessageBox($"Експорт записів завершено. Бажаєте відкрити файл з результатами?"
                             , m_tittle, MessageBoxButton.YesNo, MessageBoxImage.Information);

                        if (r == MessageBoxResult.Yes)
                        {
                            OpenFileDialog fd = new OpenFileDialog();

                            fd.FileName = e.Result as string;

                            fd.OpenFile();
                        }

                        break;
                    case NotesExporterToTxtOperations.ExportReports:
                        break;
                }
            });
        }

        private void M_HistoryNotesController_OnOperationFinished(object s, OperationFinishedEventArgs<HistoryNotesControllerOperations> e)
        {
            UseSignalSystem(e);

            UIMessaging.CreateMessageBoxAccordingToResult<HistoryNotesControllerOperations>(e, m_tittle, async () =>
            {
                switch (e.OperationType)
                {
                    case HistoryNotesControllerOperations.AddNote:

                        await m_jdataprovider.SaveFileAsync(m_pathToHistoryDB,
                            m_HistoryNotesStorageCollection, PatientRepDataProviderOperations.SaveHistoryNotesDb);

                        HistoryNotesCount = m_HistoryNotesStorageCollection.Count;

                        break;
                    case HistoryNotesControllerOperations.EditNote:

                        await m_jdataprovider.SaveFileAsync(m_pathToHistoryDB,
                            m_HistoryNotesStorageCollection, PatientRepDataProviderOperations.SaveHistoryNotesDb);

                        break;
                    case HistoryNotesControllerOperations.RemoveNote:

                        HistoryNotesCount = m_HistoryNotesStorageCollection.Count;

                        await m_jdataprovider.SaveFileAsync(m_pathToHistoryDB,
                            m_HistoryNotesStorageCollection, PatientRepDataProviderOperations.SaveHistoryNotesDb);

                        break;
                    case HistoryNotesControllerOperations.GetNotes:

                        HistoryNotesVisualModelCollection.Clear();

                        if (e.Result != null)
                        {
                            FillVisualModelCollection<HistoryNote, HistoryNoteStorage>
                            (HistoryNotesVisualModelCollection,
                            m_HistoryNotesStorageCollection,
                            (hn, i) =>
                            {
                                hn.ShowNumber = i + 1;

                                hn.OnSaveNotes += Hn_OnSaveNotes;

                                hn.OnRemoveNote += Hn_OnRemoveNote;
                            });
                        }

                        break;

                    case HistoryNotesControllerOperations.SearchNotes:

                        HistoryNotesVisualModelCollection.Clear();

                        List<HistoryNoteStorage> temp = e.Result;

                        if (temp.Count != 0)
                        {
                            FillVisualModelCollection<HistoryNote, HistoryNoteStorage>
                            (HistoryNotesVisualModelCollection, temp,
                            (hn, i) =>
                            {
                                hn.ShowNumber = i + 1;

                                hn.OnRemoveNote += Hn_OnRemoveNote;

                                hn.OnSaveNotes += Hn_OnSaveNotes;
                            }
                            );
                        }

                        break;

                    case HistoryNotesControllerOperations.SortNotes:



                        break;
                }
            });
        }

        private void M_jdataprovider_OnOperationFinished(object s, OperationFinishedEventArgs<PatientRepDataProviderOperations> e)
        {
            UIMessaging.CreateMessageBoxAccordingToResult(e, m_tittle, () =>
            {
                switch (e.OperationType)
                {
                    case PatientRepDataProviderOperations.SavePatientsDB:
                        break;
                    case PatientRepDataProviderOperations.LoadPatientsDB:

                        m_patients = new List<PatientStorage>();

                        JArray array = e.Result;

                        JArray Info = null;

                        List<string> adInfoList = null;

                        if (array != null)
                        {
                            Guid id;

                            PatientStatus stat = PatientStatus.Не_Погашено;

                            for (int i = 0; i < array.Count; i++)
                            {
                                try
                                {
                                    Info = (JArray)array[i]["AdditionalInfo"];
                                }
                                catch (Exception exep)
                                {

                                }

                                if (Info != null)
                                {
                                    adInfoList = new List<string>();

                                    foreach (string item in Info)
                                    {
                                        adInfoList.Add(item);
                                    }
                                }

                                PatientStorage p = new PatientStorage(
                                        Guid.Parse(array[i]["Id"].ToString()),
                                        array[i]["Surename"].ToString(),
                                        array[i]["Name"].ToString(),
                                        array[i]["Lastname"].ToString(),
                                        array[i]["Code"].ToString(),
                                        array[i]["Diagnosis"].ToString(),
                                        (PatientStatus)Enum.Parse(stat.GetType(), array[i]["Status"].ToString()),
                                        DateTime.Parse(array[i]["RegisterDate"].ToString()),
                                        DateTime.Parse(array[i]["InvestigationDate"].ToString()),
                                        adInfoList, array[i]["Center"]?.ToString());

                                m_patients.Add(
                                    p
                                    );
                            }
                        }

                        break;

                    case PatientRepDataProviderOperations.LoadHistoryNotesDb:

                        m_HistoryNotesStorageCollection = new List<HistoryNoteStorage>();

                        JArray Histarray = e.Result;

                        JArray addInfo = null;

                        List<string> AddInfoStr = null;

                        Guid Histid;

                        if (Histarray != null)
                        {
                            foreach (var HistNote in Histarray)
                            {
                                addInfo = (JArray)HistNote["AddInfo"];

                                if (addInfo != null)
                                {
                                    AddInfoStr = new List<string>();

                                    foreach (string addInfoItem in addInfo)
                                    {
                                        AddInfoStr.Add(addInfoItem);
                                    }
                                }

                                Histid = Guid.Parse(HistNote["Id"].ToString());

                                m_HistoryNotesStorageCollection.Add(
                                    new HistoryNoteStorage(
                                        Histid,
                                        DateTime.Parse(HistNote["InvestigationDate"].ToString()),
                                        DateTime.Parse(HistNote["HospitalizationDateTime"].ToString()),
                                        HistNote["Surename"].ToString(),
                                        HistNote["Name"].ToString(),
                                        HistNote["Lastname"].ToString(),
                                        HistNote["Center"].ToString(),
                                        HistNote["Department"].ToString(),
                                        HistNote["Reason"].ToString(),
                                        HistNote["Doctor"].ToString(),
                                        HistNote["Investigation"].ToString(),
                                        AddInfoStr
                                        )
                                    );

                            }
                        }

                        HistoryNotesCount = m_HistoryNotesStorageCollection.Count;

                        break;

                    case PatientRepDataProviderOperations.LoadSettings:

                        if (e.Result == null)
                        {
                            m_Configuration = new ConfigStorage();
                        }
                        else
                        {
                            m_Configuration = e.Result;
                        }

                        m_ViberParser.PathToDebuggingFolder = m_Configuration.PathToFailToReadPhotos +
                Path.DirectorySeparatorChar + "Debug";

                        m_Configuration.UpdateIntegratedData();

                        m_Configuration.OnConfigChanged += M_Configuration_OnConfigChanged;

                        break;

                    case PatientRepDataProviderOperations.SaveSettings:



                        break;
                }

                NoteCount = m_patients.Count;
            });
        }

        private void M_pController_OnOperationFinished(object s, OperationFinishedEventArgs<PatientControllerOperations> e)
        {
            UseSignalSystem(e);

            UIMessaging.CreateMessageBoxAccordingToResult(e, m_tittle, async () =>
            {
                switch (e.OperationType)
                {
                    case PatientControllerOperations.Add:

                        await m_jdataprovider.SaveFileAsync(m_pathToPatientsDB, m_patients, PatientRepDataProviderOperations.SavePatientsDB);

                        OnClearFieldsButtonPressedExecute(null);

                        break;

                    case PatientControllerOperations.Remove:

                        Guid id = Guid.Parse(e.Result.ToString());

                        if (id != default)
                        {
                            await Task.Run(() =>
                            {
                                int count = SearchResult.Count;

                                for (int i = 0; i < count; i++)
                                {
                                    if (SearchResult[i].Id == id)
                                    {
                                        SearchResult[i].IsRemoved = true;
                                    }
                                }
                            });
                        }

                        await m_jdataprovider.SaveFileAsync(m_pathToPatientsDB, m_patients, PatientRepDataProviderOperations.SavePatientsDB);

                        break;
                    case PatientControllerOperations.Edit:

                        await m_jdataprovider.SaveFileAsync(m_pathToPatientsDB, m_patients, PatientRepDataProviderOperations.SavePatientsDB);

                        break;

                    case PatientControllerOperations.Search:

                        SearchResult.Clear();

                        List<PatientStorage> res = e.Result;//Patient Storage

                        FillVisualModelCollection<Patient, PatientStorage>(SearchResult, res, (p, i) =>
                        {
                            p.Number = i + 1;

                            p.OnSaveChangesButtonPressed += Pat_OnSaveChangesButtonPressed;

                            p.OnRemoveButtonPressed += Pat_OnRemoveButtonPressed;
                        });

                        break;

                    case PatientControllerOperations.GetRep:

                        SearchResult.Clear();

                        if (e.Result != null)
                        {
                            List<PatientStorage> result = e.Result;

                            FillVisualModelCollection<Patient, PatientStorage>(SearchResult, result, (p, i) =>
                            {
                                p.Number = i + 1;

                                p.OnSaveChangesButtonPressed += Pat_OnSaveChangesButtonPressed;

                                p.OnRemoveButtonPressed += Pat_OnRemoveButtonPressed;
                            });
                        }

                        break;

                    case PatientControllerOperations.Sorting:

                        SearchResult.Clear();

                        if (e.Result != null)
                        {
                            List<Patient> result = e.Result;

                            int num = 1;

                            foreach (var item in result)
                            {
                                Patient p = new Patient(item.Id, item.Surename, item.Name, item.Lastname,
                                    item.Code, item.Diagnosis, item.Status, item.RegisterDate, item.InvestigationDate, item.Center, null);

                                p.Number = num;

                                p.OnRemoveButtonPressed += Pat_OnRemoveButtonPressed;

                                p.OnSaveChangesButtonPressed += Pat_OnSaveChangesButtonPressed;

                                if (item.AddInfoVMCollection != null)
                                {
                                    int count = 1;

                                    foreach (var adInf in item.AddInfoVMCollection)
                                    {
                                        p.AddInfoVMCollection.Add(new AdditionalInfoViewModel(count, adInf.Value));
                                    }
                                }

                                SearchResult.Add(
                                    p
                                    );

                                num++;
                            }
                        }

                        break;
                }

                NoteCount = m_patients.Count;

            }, !m_UI_is_UsedbyViber_Parser, !m_UI_is_UsedbyViber_Parser);
        }

        #endregion

        private async void Pat_OnRemoveButtonPressed(Patient selected)
        {
            await m_pController.RemoveAsync(selected, m_patients);
        }

        private async void Pat_OnSaveChangesButtonPressed(Patient selected)
        {
            await m_pController.EditAsync(selected, m_patients);
        }

        #endregion

        #region Methods

        #region On Window Closing

        public void StopAllTasks()
        {
            m_cts_for_all_Tasks.Cancel();

            m_cts_for_all_Tasks.Dispose();

            //m_jdataprovider.SaveFile(m_PathToConfig, m_Configuration, PatientRepDataProviderOperations.SaveSettings);

            while (!m_CheckViber.IsCanceled)
            {
#if DEBUG
                Debug.WriteLine("Try to cancel Task!");
#endif
                break;
            }
        }

        #endregion

        #region On Settings Button Pressed

        private bool CanOnSettingsButtonPressedExecute(object p) => true;

        private void OnSettingsButtonPressedExecute(object p)
        {
            m_SettingsWindow = new SettingsWindow(m_Configuration);

            m_SettingsWindow.Topmost = true;

            m_SettingsWindow.Show();
        }

        #endregion

        #region Fill Visual Model

        //public void FillVisualModelCollection(ObservableCollection<Patient> VMCol, List<PatientStorage> storageCol)
        //{
        //    int count = 1;

        //    foreach (PatientStorage ps in storageCol)
        //    {
        //        Patient p = new Patient(
        //            ps.Id, ps.Surename, ps.Name, ps.Lastname, ps.Code, ps.Diagnosis, ps.Status, ps.RegisterDate,
        //            ps.InvestigationDate, ps.Center, ps.AdditionalInfo);

        //        p.Number = count;

        //        p.OnSaveChangesButtonPressed += Pat_OnSaveChangesButtonPressed;

        //        p.OnRemoveButtonPressed += Pat_OnRemoveButtonPressed;

        //        int AdInfoCount = 1;

        //        if (ps.AdditionalInfo != null)
        //        {
        //            foreach (string item in ps.AdditionalInfo)
        //            {
        //                p.AddInfoVMCollection.Add(new AdditionalInfoViewModel(AdInfoCount, item));

        //                AdInfoCount++;
        //            }
        //        }

        //        VMCol.Add(p);

        //        count++;
        //    }
        //}

        public void FillVisualModelCollection<TVisualModel, TStorageModel>(ObservableCollection<TVisualModel> noteVisualCol,
            List<TStorageModel>
            noteStorageCol, Action<TVisualModel, int> modifyVisModel)
            where TStorageModel : IConvertStorageToVisualModel<TStorageModel, TVisualModel>
        {
            noteVisualCol.Clear();

            int count = noteStorageCol.Count;

            for (int i = 0; i < count; i++)
            {
                TVisualModel t = noteStorageCol[i].StorageToVisualModel();

                modifyVisModel?.Invoke(t, i);

                noteVisualCol.Add(t);
            }
        }

        #endregion

        #region VisibilityController

        private void SetGridVisibility()
        {
            switch (SearchCon)
            {
                case SearchCondition.Пошук_по_Прізвищу:

                    SurSearcherGridVisibility = Visibility.Visible;

                    CodeSearchGridVisibility = Visibility.Hidden;

                    DateSearchGridVisibility = Visibility.Hidden;

                    StatusSearchGridVisibility = Visibility.Hidden;

                    DateStatusSearchGridVisibility = Visibility.Hidden;

                    break;
                case SearchCondition.Пошук_за_Направленням:

                    SurSearcherGridVisibility = Visibility.Hidden;

                    CodeSearchGridVisibility = Visibility.Visible;

                    DateSearchGridVisibility = Visibility.Hidden;

                    StatusSearchGridVisibility = Visibility.Hidden;

                    DateStatusSearchGridVisibility = Visibility.Hidden;

                    break;
                case SearchCondition.Пошук_за_датою:

                    SurSearcherGridVisibility = Visibility.Hidden;

                    CodeSearchGridVisibility = Visibility.Hidden;

                    DateSearchGridVisibility = Visibility.Visible;

                    StatusSearchGridVisibility = Visibility.Hidden;

                    DateStatusSearchGridVisibility = Visibility.Hidden;

                    break;

                case SearchCondition.Пошук_за_статусом:

                    SurSearcherGridVisibility = Visibility.Hidden;

                    CodeSearchGridVisibility = Visibility.Hidden;

                    DateSearchGridVisibility = Visibility.Hidden;

                    StatusSearchGridVisibility = Visibility.Visible;

                    DateStatusSearchGridVisibility = Visibility.Hidden;

                    break;

                case SearchCondition.Пошук_за_датою_та_Статусом:

                    SurSearcherGridVisibility = Visibility.Hidden;

                    CodeSearchGridVisibility = Visibility.Hidden;

                    DateSearchGridVisibility = Visibility.Hidden;

                    StatusSearchGridVisibility = Visibility.Hidden;

                    DateStatusSearchGridVisibility = Visibility.Visible;

                    break;
            }
        }

        #endregion

        #region On Add new patient button pressed

        private bool CanOnAddNewPatientButtonPressedExecute(object p)
        {
            return CheckValidArray(0, 4);
        }

        private async void OnAddNewPatientButtonPressedExecute(object p)
        {
            List<string> adInfo = new List<string>();

            foreach (var item in AddInfoCol)
            {
                adInfo.Add(item.Value);
            }

            DateTime current = DateTime.Now;

            DateTime CurrentDateAndTime = DateTime.Now;

            CurrentDateAndTime = NewRegisterDate.AddHours(current.Hour);

            CurrentDateAndTime = NewRegisterDate.AddMinutes(current.Minute);

            CurrentDateAndTime = NewRegisterDate.AddSeconds(current.Second);

            PatientStorage pTemp = new PatientStorage(
                Guid.NewGuid(), Surename, Name, Lastname, Code, Diagnosis, Models.PatientModel.Enums.PatientStatus.Не_Погашено
                , CurrentDateAndTime, default, adInfo, null);

            await m_pController.AddAsync(pTemp, m_patients);
        }

        #endregion

        #region OnClearFieldsButtonPressed

        private bool CanOnClerFieldsButtonPressedExecute(object p) => true;

        private void OnClearFieldsButtonPressedExecute(object p)
        {
            Surename = String.Empty;

            Name = String.Empty;

            Lastname = String.Empty;

            Code = String.Empty;

            Diagnosis = String.Empty;

            FocusName = true;

            FocusName = false;
        }


        #endregion

        #region Search Grid

        #region On Clear Fields Button Pressed

        private bool CanOnClearFieldsSearchButtonPressedExecute(object p) => true;

        private void OnClearFieldsSearchButtonPressedExecute(object p)
        {
            SearchSurename = String.Empty;

            SearchCode = String.Empty;

            AddInfoCol.Clear();
        }

        #endregion

        #region On Search Button Pressed

        private bool CanOnSearchButtonPressedExecute(object p)
        {
            bool res = false;

            switch (SearchCon)
            {
                case SearchCondition.Пошук_по_Прізвищу:

                    res = CheckValidArray(4, 5);
                    break;

                case SearchCondition.Пошук_за_Направленням:

                    res = CheckValidArray(5, 6);
                    break;

                case SearchCondition.Пошук_за_датою:

                    res = true;

                    break;

                case SearchCondition.Пошук_за_статусом:

                    res = true;

                    break;

                case SearchCondition.Пошук_за_датою_та_Статусом:

                    res = true;

                    break;
            }
            return res;
        }

        private async void OnSearchButtonPressedExecute(object p)
        {
            PatientSearchArguments args = new PatientSearchArguments(
                SearchSurename,
                DateSearchStart,
                DateSearchEnd,
                SearchCode,
                SearchCon,
                StrCoincidence,
                SearchStatus
                );

            await m_pController.SearchAsync(m_patients, args);
        }

        #endregion

        #region On Get All Notes Button Pressed

        private bool CanOnGetAllNotesButtonPressedExecute(object p) => true;

        private async void OnGetAllNotesButtonPressedExecute(object p)
        {
            await m_pController.GetAllNotesAsync(m_patients);
        }

        #endregion

        #region OnSortBy Date Button Pressed

        private bool CanOnSortByDateButtonPressedExecute(object p)
        {
            return SearchResult.Count > 0;
        }

        private async void OnSortByDateButtonPressedExecute(object p)
        {
            await m_pController.SortAsync(SearchResult.ToList(), new CompareByDate());
        }

        #endregion

        #region OnSortBy Status Button Pressed

        private bool CanOnSortByStatusButtonPressedExecute(object p) => SearchResult.Count > 0;

        private async void OnSortByStatusButtonPressedExecute(object p)
        {
            await m_pController.SortAsync(SearchResult.ToList(), new CompareByStatus());
        }


        #endregion

        #region On Sort by Center Button Presed

        private bool CanOnSortByCenterButtonPressedExecute(object p)
        {
            if (SearchResult.Count > 0)
            {
                return true;
            }

            return false;
        }

        private async void OnSortByCenterButtonPressedExecute(object p)
        {
            await m_pController.SortAsync(SearchResult.ToList(), new CompareByCenter());
        }

        #endregion

        #region On Add New Additional info button pressed

        private bool CanOnAddNewAddInfoButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnAddNewAddInfoButtonPressedExecute(object p)
        {
            AddInfoCol.Add(new AdditionalInfoViewModel(AddInfoCol.Count + 1, "      "));
        }

        #endregion On Remove AddInfo button pressed

        #region On Remove Add Info Button Pressed

        private bool CanOnRemoveAddInfoButtonPressedExecute(object p) => SelectedAddInfoIndex >= 0;

        private void OnRemoveAddInfoButtonPressedExecute(object p)
        {
            AddInfoCol.RemoveAt(SelectedAddInfoIndex);
        }

        #endregion

        #endregion

        #region History Registration System

        #region On Add New Additinal info History Note

        private bool CanOnAddNewAddInfoHistoryNoteExecute(object p) => true;

        private void OnAddNewNewAddInfoHistoryNoteExecute(object p)
        {
            HistoryRegistrationAddInfoCollection.Add(
                new AdditionalInfoViewModel(HistoryRegistrationAddInfoCollection.Count + 1, "   ")
                );
        }

        #endregion

        #region On Remove New Additinal info History Note

        private bool CanOnRemoveNewNewAddInfoHistoryNoteExecute(object p) => HistoryRegistrationSelectedIndex >= 0;

        private void OnRemoveNewNewAddInfoHistoryNoteExecute(object p)
        {
            HistoryRegistrationAddInfoCollection.RemoveAt(HistoryRegistrationSelectedIndex);
        }
        #endregion

        #region On Remove All Additinal info History Notes

        private bool CanOnRemoveAllNewNewAddInfoHistoryNotesExecute(object p) => HistoryRegistrationAddInfoCollection.Count > 0;

        private void OnRemoveAllNewNewAddInfoHistoryNotesExecute(object p)
        {
            HistoryRegistrationAddInfoCollection.Clear();
        }

        #endregion

        #region On Add new History note

        private bool CanOnAddNewHistoryNoteExecute(object p) => CheckValidArray(6, 13);

        private async void OnAddNewHistoryNoteExecute(object p)
        {
            List<string> adInfostr = new List<string>();

            foreach (var item in HistoryRegistrationAddInfoCollection)
            {
                adInfostr.Add(item.Value);
            }

            var hist =
                new HistoryNoteStorage(
                    Guid.NewGuid(),
                    HistoryRegistrationDate,
                    HospitalDateTime,
                    PatientSurename,
                    PatientName,
                    PatientLastName,
                    Center,
                    Department,
                    Reason,
                    Physician,
                    InvestType,
                    adInfostr
                );

            await m_HistoryNotesController.AddAsync(hist, m_HistoryNotesStorageCollection);
        }

        #endregion

        #region History Register Clear Fields

        private bool CanOnHistoryRegisterClearFieldsExecute(object p)
        {
            return true;
        }

        private void OnHistoryRegisterClearFieldsExecute(object p)
        {
            PatientName = String.Empty;

            PatientSurename = String.Empty;

            PatientLastName = String.Empty;

            ClearDate = true;

            ClearDate = false;

            HospitalDateTime = new DateTime();

            Center = String.Empty;

            Department = String.Empty;

            Reason = String.Empty;

            if (!String.IsNullOrEmpty(Physician))
            {
                Physician = String.Empty;
            }
        }

        #endregion

        #region Register History Notes

        private bool CanOnRegisterHistoryNotesExecute(object p)
        {
            return m_ViewRegister;
        }

        private void OnRegisterHistoryNotesExecute(object p)
        {
            CaseRegisterHistory = Visibility.Visible;

            CaseSearchHistory = Visibility.Hidden;

            m_ViewRegister = false;
        }

        #endregion

        #region View History Mode

        private bool CanOnViewHistoryButtonPressedExecute(object p)
        {
            return !m_ViewRegister;
        }

        private void OnViewHistoryButtonPressedExecute(object p)
        {
            CaseRegisterHistory = Visibility.Hidden;

            CaseSearchHistory = Visibility.Visible;

            m_ViewRegister = true;
        }

        #endregion

        #region Get all History Notes

        private bool CanOnGetAllHistoryNotesPressed(object p)
        {
            return true;
        }

        private async void OnGetAllHistoryNotesPressed(object p)
        {
            await m_HistoryNotesController.GetAllNotesAsync(m_HistoryNotesStorageCollection);
        }

        #endregion

        #endregion

        #region Report System

        #region On Search Conditions Button Pressed

        private bool CanOnSearchConditionsButtonPressedExecute(object p)
        {
            return !m_SearchReport;
        }

        private void OnSearchConditionsButtonPressedExecute(object p)
        {
            CaseSearchNotesVisibility = Visibility.Visible;

            CaseReportSystemVisibility = Visibility.Hidden;

            m_SearchReport = true;
        }
        #endregion

        #region On Report Conditions Button Pressed

        private bool CanOnReportConditionsButtonPressedExcute(object p)
        {
            return m_SearchReport;
        }

        private void OnReportConditionsButtonPressedExcute(object p)
        {
            CaseSearchNotesVisibility = Visibility.Hidden;

            CaseReportSystemVisibility = Visibility.Visible;

            m_SearchReport = false;
        }

        #endregion

        #region On Search History Notes Button Pressed

        private bool CanOnSearchHistoryNotesButtonPressedExecute(object p) => true;

        private async void OnSearchHistoryNotesButtonPressedExecute(object p)
        {
            HistoryNoteSearchArgs args = new HistoryNoteSearchArgs(NoteSearchStart, NoteSearchEnd);

            await m_HistoryNotesController.SearchAsync(m_HistoryNotesStorageCollection, args);
        }


        #endregion

        #region On Create report Button Pressed

        private bool CanOnCreateReportButtonPressedExecute(object p) => HistoryNotesVisualModelCollection.Count > 0;

        private void OnCreateReportButtonPressedExecute(object p)
        {
            List<HistoryNote> notes = new List<HistoryNote>();

            foreach (var item in HistoryNotesVisualModelCollection) //O(n)
            {
                if (!item.IsRemoved)
                {
                    notes.Add(item);
                }
            }

            m_ReportViewerWindow = new ReportViewer(notes, RepType, m_Configuration);

            m_ReportViewerWindow.Topmost = true;

            m_ReportViewerWindow.Show();
        }

        #endregion

        #region On Export Notes Button Pressed 

        private bool CanOnExportNotesButtonPressedExecute(object p)
        {
            if (m_Configuration != null)
            {
                return !String.IsNullOrWhiteSpace(m_Configuration.NotesReportOutput) && SearchResult.Count > 0;
            }
            return false;

        }

        private void OnExportNotesButtonPressedExecute(object p)
        {
            string header = $"Боржники на ФЛГ від {DateSearchStart.ToShortDateString()} до {DateSearchEnd.ToShortDateString()}";

            List<NoteExport> notes = new List<NoteExport>();

            foreach (var item in SearchResult)
            {
                notes.Add(item.ConvertToExportable());
            }

            m_NoteExporterToTxt.Export(NotesExporterToTxtOperations.ExportNotes,
                path: m_Configuration.NotesReportOutput,
                fileName: header + ".txt",
                Header: header,
                notesForExport: notes);

        }

        #endregion

        #endregion

        #endregion
    }
}
