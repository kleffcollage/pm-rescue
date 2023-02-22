using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IReliefRepository
    {
         RentRelief CreateAndReturn(RentRelief model);
        IEnumerable<RentRelief> ListReliefs();
        Installment UpdateInstallment(Installment model);
        void CreateBulkInstallments(List<Installment> installments);
        IEnumerable<Installment> ListInstallments();
    }
}