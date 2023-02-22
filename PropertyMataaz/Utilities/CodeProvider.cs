using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities.Abstrctions;

namespace PropertyMataaz.Utilities
{
    public class CodeProvider : BaseRepository<Code>, ICodeProvider
    {
        private readonly IUtilityMethods _utilityMethods;

        public CodeProvider(PMContext context, IUtilityMethods utilityMethods) : base(context)
        {
            _utilityMethods = utilityMethods;
        }

        public Code New(int userId, string key, int expiryInMinutes = 2880, int length = 6, string prefix = "", string suffix = "")
        {
            try
            {
                var NewCode = new Code
                {
                    UserId = userId <= 0 ? null : userId,
                    Key = key ?? "",
                    CodeString = prefix.ToLower() + _utilityMethods.RandomCode(length).ToLower() + suffix.ToLower(),
                    ExpiryDate = DateTime.Now.AddMinutes(expiryInMinutes),
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };
                while ((GetByCodeString(NewCode.CodeString) != null) || NewCode.CodeString == null)
                {
                    NewCode.CodeString = prefix + _utilityMethods.RandomCode(length) + suffix;
                }

                NewCode = CreateAndReturn(NewCode);

                return NewCode;


            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public Code GetByCodeString(string code)
        {
            return GetAll().FirstOrDefault(c => c.CodeString == code);
        }

        public bool SetExpired(Code code)
        {
            try
            {
                code.ExpiryDate = DateTime.Now;
                code.IsExpired = true;
                Update(code);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return true;
            }
        }
    }
}
