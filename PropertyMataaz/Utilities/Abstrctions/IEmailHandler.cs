using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace PropertyMataaz.Utilities.Abstrctions
{
    public interface IEmailHandler
    {
        Task SendEmail(string email, string subject, string message);

        string ComposeFromTemplate(string name, List<KeyValuePair<string, string>> customValues);
    }
}
