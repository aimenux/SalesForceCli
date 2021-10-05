using System;
using System.Collections.Generic;
using System.Dynamic;
using Lib.Services;

namespace Lib.Helpers
{
    public interface IConsoleHelper
    {
        void RenderTitle(string text);

        void RenderQuery(string objectName, string query);

        void RenderSettingsFile(string filepath);

        void RenderSalesForceFields(ICollection<string> fields, string caption = null);

        void RenderSalesForceResults<T>(SalesForceResults<T> results, string caption = null);

        void RenderSalesForceObjects(ICollection<SalesForceObject> objects, string caption = null);

        void RenderException(Exception exception);
    }
}
