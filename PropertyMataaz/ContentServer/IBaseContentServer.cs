using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.ContentServer
{
    public interface IBaseContentServer
    {
        Task<FileDocument> UploadDocumentAsync(FileDocument document);
    }
}
