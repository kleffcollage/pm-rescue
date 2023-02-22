using System.Collections.Generic;

namespace PropertyMataaz.Utilities.Abstrctions
{
    public interface IPDFHandler
    {
         string ComposeFromTemplate(string name, List<KeyValuePair<string, string>> customValues);
    }
}