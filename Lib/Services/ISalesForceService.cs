using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Services
{
    public interface ISalesForceService
    {
        Task<string> GetQueryAsync(SalesForceParameters parameters, CancellationToken cancellationToken = default);

        Task<SalesForceResults<T>> RunQueryAsync<T>(SalesForceParameters parameters, CancellationToken cancellationToken = default);

        Task<ICollection<string>> GetFieldsAsync(SalesForceParameters parameters, CancellationToken cancellationToken = default);

        Task<ICollection<SalesForceObject>> GetObjectsAsync(SalesForceParameters parameters, CancellationToken cancellationToken = default);
    }
}
