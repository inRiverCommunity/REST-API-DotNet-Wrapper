namespace inRiverCommunity.Connectivity.iPMC.Contract
{
    public class ExtensionModel
    {
        public string AssemblyName { get; set; }
        public string AssemblyType { get; set; }
        public string ExtensionId { get; set; }
        public string ExtensionType { get; set; }
        public PackageModel Package { get; set; }
        public StatusModel Status { get; set; }
    }
}