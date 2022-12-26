using CRUDControllerLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerBaseLib;
using CRUDControllerLib.Enums;
using CRUDControllerLib.SearchArgs;
using System.Security.Cryptography;
using CRUDControllerLib.PatientController.Exceptions;
using Models.PatientModel.PatientVisualModel;
using Models.PatientModel.PatientStorageModel;

namespace CRUDControllerLib.PatientController
{
    public class PatientController : ControllerBaseClass<PatientControllerOperations>, ICRUDController<Patient, PatientStorage, PatientSearchArguments>, ISortable<Patient>
    {        
        public async Task AddAsync(PatientStorage entity, IList<PatientStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                PatientControllerOperations.Add, 
                (state, cts)=>
                {
                    bool flag = false;   

                    for (int i = 0; i < col.Count; i++)
                    {
                        if (col[i].Equals(entity))
                        {
                            throw new EntityAlreadyExistsException("Такий хворий вже існує. Хтось його вже додав.");
                        }
                    }
                    
                    col.Add(entity);

                    return null;
                }
                );
        }

        public async Task RemoveAsync(Patient entity, IList<PatientStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                PatientControllerOperations.Remove, (state, cts)=>
                {
                    Guid id = new Guid();

                    for (int i = 0; i < col.Count; i++)
                    {
                        if (col[i].Id == entity.Id 
                        && col[i].Surename.Equals(entity.Surename, StringComparison.OrdinalIgnoreCase)
                        && col[i].Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
                        && col[i].Lastname.Equals(entity.Lastname, StringComparison.OrdinalIgnoreCase))
                       
                        {
                            id = col[i].Id;

                            col.RemoveAt(i);
                            
                            break;
                        }
                    }

                    return id;
                }
                );
        }

        public async Task SearchAsync(IList<PatientStorage> col, PatientSearchArguments args)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                PatientControllerOperations.Search, (state, cts)=>
                {
                    IEnumerable<PatientStorage> res = null;

                    switch (args.SearchCondition)
                    {
                        case SearchCondition.Пошук_по_Прізвищу:

                            switch (args.StrCoincidence)
                            {
                                case StringCoincidence.Часткове:

                                    res = (from p in col where Contains(p.Surename, args.Surename) select p)
                                        .OrderBy(p => p.Surename).ToList();

                                    break;
                                case StringCoincidence.Повне:

                                    res = (from p in col where p.Surename.Equals(args.Surename, StringComparison.OrdinalIgnoreCase) select p)
                                        .OrderBy(p => p.Surename).ToList();

                                    break;

                            }

                            break;
                        case SearchCondition.Пошук_за_Направленням:

                            res = (from p in col where p.Code.Equals(args.Code, StringComparison.OrdinalIgnoreCase) select p)
                                .OrderBy(p => p.Surename).ToList();

                            break;
                        case SearchCondition.Пошук_за_датою:

                            res = (from p in col where p.RegisterDate.Date >= args.DateStart.Date && p.RegisterDate.Date <= args.DateEnd.Date select p)
                                .OrderBy(p => p.RegisterDate.Date).ToList();

                            break;

                        case SearchCondition.Пошук_за_статусом:

                            res = (from p in col
                                   where p.Status == args.Status
                                   select p).OrderBy(p => p.Surename).ToList();

                            break;

                        case SearchCondition.Пошук_за_датою_та_Статусом:

                            res = (from p in col
                                   where p.RegisterDate.Date >= args.DateStart.Date
                                  && p.RegisterDate.Date <= args.DateEnd && p.Status == args.Status
                                   select p).ToList();

                            break;
                    }

                    return res;

                }
                );            
        }

        private static bool CompareCodes(string code1, string code2)
        {
            for (int i = 0; i < code1.Length; i++)
            {
                if (!char.Equals(code1[i], code2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Contains(string strSource, string strSearch)
        {
            if (strSource == null && strSearch == null)
            {
                return false;
            }

            bool flag = false;

            bool iterStop = false;

            int strSourceLength = strSource.Length;

            int strSearchLength = strSearch.Length;

            int Jtemp = 0;

            for (int i = 0; i < strSourceLength; i++) // Iterate Search
            {
                for (int j = Jtemp; j < strSearchLength; j++) // iterate Source
                {
                    if (j == strSearchLength)
                    {
                        iterStop = true;

                        break;
                    }

                    if (strSource[i].ToString().Equals(strSearch[j].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }

                    Jtemp = j + 1;

                    break;
                }

                if (iterStop)
                {
                    break;
                }
            }

            return flag;
        }

        public async Task EditAsync(Patient entity, IList<PatientStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync(
                PatientControllerOperations.Edit, 
                (state, cts)=>
                {
                    PatientStorage PatForedit = (from p in col
                                      where entity.Id == p.Id
                                      select p).First();

                    PatForedit.Surename = entity.Surename;

                    PatForedit.Name = entity.Name;

                    PatForedit.Lastname = entity.Lastname;

                    PatForedit.Diagnosis = entity.Diagnosis;

                    PatForedit.Status = entity.Status;

                    PatForedit.InvestigationDate = entity.InvestigationDate;

                    PatForedit.RegisterDate = entity.RegisterDate;

                    PatForedit.Code = entity.Code;

                    PatForedit.Center = entity.Center;

                    PatForedit.AdditionalInfo.Clear();

                    for (int i = 0; i < entity.AddInfoVMCollection.Count; i++)
                    {
                        PatForedit.AdditionalInfo.Add(entity.AddInfoVMCollection[i].Value);
                    }

                    return null;
                }
                );
        }

        public async Task GetAllNotesAsync(IList<PatientStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync
                (PatientControllerOperations.GetRep, (state, cts)=>
                {
                    return (from p in col select p).OrderBy(p => p.Surename).ToList();
                }
                );
        }

        public async Task SortAsync(List<Patient> col, IComparer<Patient> comparer)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync
                (PatientControllerOperations.Sorting,
                (stste, cts) =>
                {
                    col.Sort(comparer);

                    return col;
                });
                       
        }
    }
}
