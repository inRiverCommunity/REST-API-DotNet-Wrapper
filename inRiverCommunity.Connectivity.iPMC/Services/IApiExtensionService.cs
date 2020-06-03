using System.Collections.Generic;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Contract;

namespace inRiverCommunity.Connectivity.iPMC.Services
{
    public interface IApiExtensionService
    {
        Task<ExtensionModel> AddExtension(ExtensionCreationModel extensionCreation);

        Task<ExtensionSettingModel> AddOrUpdateExtensionSetting(string extensionId, ExtensionSettingModel extensionSetting);

        Task<List<ExtensionSettingModel>> ApplyDefaultExtensionSettings(string extensionId);

        Task<bool> DeleteExtension(string extensionId);

        Task<bool> DeleteExtensionSetting(string extensionId, string key);

        Task<StatusModel> DisableExtension(string extensionId);

        Task<StatusModel> EnableExtension(string extensionId);

        Task<List<ExtensionModel>> GetAllExtensions();

        Task<ExtensionModel> GetExtension(string extensionId);

        Task<List<ExtensionSettingModel>> GetExtensionSettings(string extensionId);

        Task<StatusModel> GetExtensionStatus(string extensionId);

        Task<StatusModel> PauseExtension(string extensionId);

        Task<bool> ReloadExtensionSettings(string extensionId);

        Task<bool> ReplaceExtensionApiKey(string extensionId, string apiKey);

        Task<ResponseModel> RestartService();

        Task<StatusModel> ResumeExtension(string extensionId);

        Task<ResponseModel> RunExtension(string extensionId);

        Task<ResponseModel> TestExtension(string extensionId);
    }
}