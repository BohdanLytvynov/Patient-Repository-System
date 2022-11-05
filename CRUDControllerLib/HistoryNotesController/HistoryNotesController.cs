using ControllerBaseLib;
using CRUDControllerLib.Enums;
using CRUDControllerLib.Interfaces;
using CRUDControllerLib.SearchArgs;
using Models.HistoryNoteModels.StorageModel;
using Models.HistoryNoteModels.VisualModel;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRUDControllerLib.HistoryNotesController
{
    public class HistoryNotesController : ControllerBaseClass, ICRUDController<HistoryNote, HistoryNoteStorage, HistoryNoteSearchArgs>
    {
        public HistoryNotesController()
        {

        }

        public async Task AddAsync(HistoryNoteStorage entity, IList<HistoryNoteStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<HistoryNotesControllerOperations>(HistoryNotesControllerOperations.AddNote,
                (state, cts)=>
                {
                    while (IsGuidExists<HistoryNoteStorage>(col, entity.Id))
                    {
                        entity.Id = Guid.NewGuid();
                    }

                    col.Add(entity);

                    return null;
                });
        }

        public async Task EditAsync(HistoryNote entity, IList<HistoryNoteStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<HistoryNotesControllerOperations>
                (
                    HistoryNotesControllerOperations.EditNote,

                    (state, cts)=>
                    {                        
                        var r = (from n in col where n.Id == entity.Id select n).First();

                        r.Surename = entity.Surename;

                        r.Name = entity.Name;

                        r.Lastname = entity.Lastname;

                        r.InvestigationDate = entity.InvestDate;

                        r.HospitalizationDateTime = entity.HospdateTime;

                        r.Center = entity.Center;

                        r.Department = entity.Department;

                        r.Reason = entity.Reason;

                        r.Doctor = entity.Doctor;

                        r.Investigation = entity.Investigation;

                        r.AddInfo.Clear();

                        foreach (var item in entity.AddInfoCollection)
                        {
                            r.AddInfo.Add(item.Value);
                        }

                        return null;
                    }
                );
        }

        public async Task GetAllNotesAsync(IList<HistoryNoteStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<HistoryNotesControllerOperations>
                (HistoryNotesControllerOperations.GetNotes, (state, cts) =>
                {
                    return (from n in col select n).OrderBy(n => n.InvestigationDate).ToList();
                }
                );
        }

        public async Task RemoveAsync(HistoryNote entity, IList<HistoryNoteStorage> col)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<HistoryNotesControllerOperations>
                 (HistoryNotesControllerOperations.RemoveNote,
                 (state, cts)=>
                 {                    
                     int count = col.Count;

                     for (int i = 0; i < count; i++)
                     {
                         if (col[i].Id == entity.Id)                                                   
                         {
                             col.RemoveAt(i);

                             break;
                         }
                     }

                     return null;
                 }
                 );
        }

        public async Task SearchAsync(IList<HistoryNoteStorage> col, HistoryNoteSearchArgs args)
        {
            await ExecuteFunctionAndGetResultThroughEventAsync<HistoryNotesControllerOperations>
                (HistoryNotesControllerOperations.SearchNotes,
                (state, cts)=>
                {
                    return (from n in col where n.InvestigationDate >= args.Start && n.InvestigationDate <= args.End select n)
                    .OrderBy(n => n.InvestigationDate).ToList();
                }
                
                );
        }

        private bool IsGuidExists<TItem>(IList<TItem> col, Guid id)
           where TItem : IGetId
        {
            foreach (var item in col)
            {
                if (item.GetId() == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
