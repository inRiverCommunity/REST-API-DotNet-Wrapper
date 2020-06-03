using System;
using inRiverCommunity.Connectivity.iPMC.Services;

namespace inRiverCommunity.Connectivity.iPMC
{
    public interface IApiClient : IDisposable
    {
        public IApiExtensionService ExtensionService { get; }
        public IApiPackageService PackageService { get; }
        public string Version { get; }
    }
}