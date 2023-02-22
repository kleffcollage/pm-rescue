using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Utilities.Abstrctions
{
    public interface IUtilityMethods
    {
        string RandomCode(int size);

        string FormattedDate(DateTime thisDate);

        string GetUniqueFileName(string fileName);
    }
}
