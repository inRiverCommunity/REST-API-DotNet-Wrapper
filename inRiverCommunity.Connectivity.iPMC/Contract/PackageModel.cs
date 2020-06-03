using System;

namespace inRiverCommunity.Connectivity.iPMC.Contract
{
    public class PackageModel
    {
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }
        public int Id { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Version { get; set; }
    }
}