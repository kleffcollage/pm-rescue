using System.Collections;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface ICleanRepository
    {
        Cleaning CreateAndReturn(Cleaning cleaning);
        Cleaning GetById(int id);
        CleaningQuote CreateCleaningQuote(CleaningQuote cleaningQuote);
        IEnumerable<Cleaning> ListCleaning();
        Cleaning Update(Cleaning cleaning);
        CleaningQuote GetQuoteById(int Id);
        CleaningQuote UpdateQuote(CleaningQuote cleaningQuote);
    }
}