using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Words;
using Aspose.Words.Replacing;
using GemBox.Document;
using Microsoft.AspNetCore.Hosting;
using PropertyMataaz.Utilities.Abstrctions;

namespace PropertyMataaz.Utilities
{
    public class PDFHandler : IPDFHandler
    {
        private readonly IWebHostEnvironment _env;

        public PDFHandler(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string ComposeFromTemplate(string name, List<KeyValuePair<string, string>> customValues)
        {
            var docTemplate = string.Empty;
            Logger.Info(Directory.GetCurrentDirectory());
            Logger.Info(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
            var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (var dir in dirs)
            {
                Logger.Info(dir);
            }

            Logger.Info(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);

            var TemplateFolder = Path.Combine(_env.WebRootPath, "DocumentTemplates");

            if (!Directory.Exists(TemplateFolder))
            {
                Directory.CreateDirectory(TemplateFolder);

            }

            var path = Path.Combine(TemplateFolder, name ?? "");

            if (path == TemplateFolder)
            {
                Logger.Info("Template not found");
                return string.Empty;
            }

            Document thisDocument = new(path);

            if (customValues != null)
            {
                foreach (KeyValuePair<string, string> pair in customValues)
                {
                    thisDocument.Range.Replace(pair.Key, pair.Value, new FindReplaceOptions(FindReplaceDirection.Forward));
                }
            }

            using MemoryStream stream = new();
            thisDocument.Save(stream, SaveFormat.Pdf);

            return Convert.ToBase64String(stream.ToArray());
        }
    }
}