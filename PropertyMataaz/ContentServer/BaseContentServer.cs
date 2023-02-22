using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Abstrctions;

namespace PropertyMataaz.ContentServer
{
    public class BaseContentServer
    {
        public static IBaseContentServer Build(ContentServerTypeEnum type, Globals globals, IUtilityMethods _utilityMethods)
        {
            switch (type)
            {
                case ContentServerTypeEnum.GOOGLEDRIVE:
                    return new GoogleDriveUpload(globals,_utilityMethods);

                default:
                    return new GoogleDriveUpload(globals, _utilityMethods);
                     
            }
        }

        public enum ContentServerTypeEnum
        {
            FIREBASE = 1,
            GOOGLEDRIVE,
            DROPBOX
        }
    }
}
