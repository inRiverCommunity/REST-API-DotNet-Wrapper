using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Contract;

namespace inRiverCommunity.Connectivity.iPMC.Services.Impl
{
    internal class ApiExtensionWrapper : IApiExtensionService
    {
        private readonly ApiClient _adapter;

        internal ApiExtensionWrapper(ApiClient adapter)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }


        public async Task<List<ExtensionModel>> GetAllExtensions() 
            => await _adapter.GetAsync<List<ExtensionModel>>(@"extensions");

        public async Task<ExtensionModel> GetExtension(string extensionId) 
            => await _adapter.GetAsync<ExtensionModel>($@"extensions/{extensionId}");

        public async Task<ExtensionModel> AddExtension(ExtensionCreationModel extensionCreation) 
            => await _adapter.PostAsync<ExtensionCreationModel, ExtensionModel>(@"extensions", extensionCreation);

        public async Task<bool> ReplaceExtensionApiKey(string extensionId, string apiKey) 
            => await _adapter.PutAsync($@"extensions/{extensionId}/apikey", new ApiKeyModel {ApiKey = apiKey});

        public async Task<bool> DeleteExtension(string extensionId) 
            => await _adapter.DeleteAsync($@"extensions/{extensionId}");

        public async Task<List<ExtensionSettingModel>> GetExtensionSettings(string extensionId) 
            => await _adapter.GetAsync<List<ExtensionSettingModel>>($@"extensions/{extensionId}/settings");

        public async Task<ExtensionSettingModel> AddOrUpdateExtensionSetting(string extensionId, ExtensionSettingModel extensionSetting) 
            => await _adapter.PutAsync<ExtensionSettingModel, ExtensionSettingModel>($@"extensions/{extensionId}/settings", extensionSetting);

        public async Task<bool> DeleteExtensionSetting(string extensionId, string key) 
            => await _adapter.DeleteAsync($@"extensions/{extensionId}/settings/{key}");

        public async Task<List<ExtensionSettingModel>> ApplyDefaultExtensionSettings(string extensionId) 
            => await _adapter.PostAsync<List<ExtensionSettingModel>>($@"extensions/{extensionId}/settings:applydefaults");

        public async Task<bool> ReloadExtensionSettings(string extensionId) 
            => await _adapter.PostAsync($@"extensions/{extensionId}/settings:reload");

        public async Task<StatusModel> GetExtensionStatus(string extensionId) 
            => await _adapter.GetAsync<StatusModel>($@"extensions/{extensionId}/status");

        public async Task<StatusModel> EnableExtension(string extensionId) 
            => await _adapter.PostAsync<StatusModel>($@"extensions/{extensionId}:enable");

        public async Task<StatusModel> DisableExtension(string extensionId) 
            => await _adapter.PostAsync<StatusModel>($@"extensions/{extensionId}:disable");

        public async Task<StatusModel> ResumeExtension(string extensionId) 
            => await _adapter.PostAsync<StatusModel>($@"extensions/{extensionId}:resume");

        public async Task<StatusModel> PauseExtension(string extensionId) 
            => await _adapter.PostAsync<StatusModel>($@"extensions/{extensionId}:pause");

        public async Task<ResponseModel> RunExtension(string extensionId) 
            => await _adapter.PostAsync<ResponseModel>($@"extensions/{extensionId}:run");

        public async Task<ResponseModel> TestExtension(string extensionId) 
            => await _adapter.PostAsync<ResponseModel>($@"extensions/{extensionId}:test");

        public async Task<ResponseModel> RestartService() 
            => await _adapter.PostAsync<ResponseModel>(@"extensionmanager:restartservice");
    }
}