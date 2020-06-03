using inRiver.Remoting.Extension.Interface;

namespace inRiverCommunity.Extensions.TestExtensions.iPMC
{
    public class TestInboundDataExtension : TestExtensionBase, IInboundDataExtension
    {
        public string Add(string value)
        {
            return value;
        }

        public string Update(string value)
        {
            return value;
        }

        public string Delete(string value)
        {
            return value;
        }
    }
}
