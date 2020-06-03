using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Contract;

namespace inRiverCommunity.Connectivity.iPMC.Services.Impl
{
    internal class ApiPackageService : IApiPackageService
    {
        private readonly ApiClient _adapter;

        internal ApiPackageService(ApiClient adapter)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        public async Task<List<PackageModel>> GetAllPackages() 
            => await _adapter.GetAsync<List<PackageModel>>(@"packages");

        public async Task<PackageModel> GetPackage(int packageId) 
            => await _adapter.GetAsync<PackageModel>($@"packages/{packageId}");

        public async Task<PackageModel> UploadPackageFile(string fileName, Stream data) 
            => await UploadOrReplacePackageFile(@"packages:uploadbase64", fileName, data);

        public async Task<PackageModel> ReplacePackageFile(int packageId, string fileName, Stream data) 
            => await UploadOrReplacePackageFile($@"packages/{packageId}:uploadandreplacebase64", fileName, data);

        public async Task<bool> DeletePackage(int packageId) 
            => await _adapter.DeleteAsync($@"packages/{packageId}");

        private async Task<PackageModel> UploadOrReplacePackageFile(string path, string fileName, Stream data)
        {
            string base64;
            await using (MemoryStream memoryStream = new MemoryStream())
            {
                await data.CopyToAsync(memoryStream);
                base64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            Base64FileModel fileModel = new Base64FileModel {FileName = fileName, Data = base64};

            return await _adapter.PostAsync<Base64FileModel, PackageModel>(path, fileModel);
        }
    }
}