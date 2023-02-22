using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Utilities.Abstrctions
{
    public interface ICodeProvider
    {
        public Code New(int userId, string key, int expiryInMinutes = 2880, int length = 6, string prefix = "", string suffix = "");

        public Code GetByCodeString(string code);

        public bool SetExpired(Code code);

        Code Update(Code code);
    }
}
