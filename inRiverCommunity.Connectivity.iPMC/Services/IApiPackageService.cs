using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Contract;

namespace inRiverCommunity.Connectivity.iPMC.Services
{
    public interface IApiPackageService
    {
        Task<bool> DeletePackage(int packageId);

        Task<List<PackageModel>> GetAllPackages();

        Task<PackageModel> GetPackage(int packageId);

        Task<PackageModel> ReplacePackageFile(int packageId, string fileName, Stream data);

        Task<PackageModel> UploadPackageFile(string fileName, Stream data);
    }
}