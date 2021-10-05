using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Configuration;
using Lib.Extensions;
using Lib.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Salesforce.Common;
using Salesforce.Force;

namespace Lib.Services
{
    public class SalesForceService : ISalesForceService
    {
        private readonly IFileHelper _fileHelper;
        private readonly IOptions<Settings> _options;

        public SalesForceService(IFileHelper fileHelper, IOptions<Settings> options)
        {
            _options = options;
            _fileHelper = fileHelper;
        }

        public async Task<ICollection<T>> RunQueryAsync<T>(string query, CancellationToken cancellationToken = default)
        {
            using var authenticationClient = await GetAuthenticationClientAsync();
            using var forceClient = GetForceClient(authenticationClient);
            var result = await forceClient.QueryAsync<T>(query);
            var objects = result.Records;
            return objects;
        }

        public async Task<string> GetQueryAsync(string objectName, int maxItems, CancellationToken cancellationToken = default)
        {
            using var authenticationClient = await GetAuthenticationClientAsync();
            using var forceClient = GetForceClient(authenticationClient);
            var fields = await forceClient.GetFieldsCommaSeparatedListAsync(objectName);
            var query = $"SELECT {string.Join(", ", fields)} FROM {objectName} LIMIT {maxItems}";
            return query;
        }

        public async Task<SalesForceResults<T>> RunQueryAsync<T>(SalesForceParameters parameters, CancellationToken cancellationToken = default)
        {
            var fileName = parameters.FileName;
            var outputFileName = parameters.OutputFileName;
            var query = await File.ReadAllTextAsync(fileName, cancellationToken);
            using var authenticationClient = await GetAuthenticationClientAsync();
            using var forceClient = GetForceClient(authenticationClient);
            var result = await forceClient.QueryAsync<T>(query);
            var objects = result.Records;
            _fileHelper.WriteToFile(outputFileName, objects);
            return new SalesForceResults<T>
            {
                Records = objects,
                InputFile = fileName,
                OutputFile = outputFileName
            };
        }

        public async Task<ICollection<string>> GetFieldsAsync(SalesForceParameters parameters, CancellationToken cancellationToken = default)
        {
            var maxItems = parameters.MaxItems;
            var pattern = parameters.Pattern;
            var objectName = parameters.ObjectName;
            using var authenticationClient = await GetAuthenticationClientAsync();
            using var forceClient = GetForceClient(authenticationClient);
            var result = await forceClient.DescribeAsync<JObject>(objectName);
            var tokens = (JArray)result["fields"] ?? new JArray();
            var fields = tokens
                .Select(x => x["name"].ToString())
                .Where(x => pattern.IsMatchingPattern(x))
                .OrderBy(x => x)
                .Take(maxItems)
                .ToList();
            return fields;
        }

        public async Task<ICollection<SalesForceObject>> GetObjectsAsync(SalesForceParameters parameters, CancellationToken cancellationToken = default)
        {
            var maxItems = parameters.MaxItems;
            var pattern = parameters.Pattern;
            using var authenticationClient = await GetAuthenticationClientAsync();
            using var forceClient = GetForceClient(authenticationClient);
            var result = await forceClient.GetObjectsAsync<SalesForceObject>();
            var objects = result.SObjects
                .Where(x => pattern.IsMatchingPattern(x.Name))
                .OrderBy(x => x.Name)
                .Take(maxItems)
                .ToList();
            return objects;
        }

        private static IForceClient GetForceClient(IAuthenticationClient authenticationClient)
        {
            var instanceUrl = authenticationClient.InstanceUrl;
            var accessToken = authenticationClient.AccessToken;
            var apiVersion = authenticationClient.ApiVersion;
            return new ForceClient(instanceUrl, accessToken, apiVersion);
        }

        private async Task<IAuthenticationClient> GetAuthenticationClientAsync()
        {
            var settings = _options.Value;
            var username = settings.UserName;
            var password = settings.Password;
            var endpoint = settings.Endpoint;
            var clientId = settings.ClientId;
            var clientSecret = settings.ClientSecret;
            var authenticationClient = new AuthenticationClient();
            await authenticationClient.UsernamePasswordAsync(clientId, clientSecret, username, password, endpoint);
            return authenticationClient;
        }
    }
}