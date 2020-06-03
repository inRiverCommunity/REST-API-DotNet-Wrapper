using System.Collections.Generic;
using inRiver.Remoting.Extension;

namespace inRiverCommunity.Extensions.TestExtensions.iPMC
{
    public abstract class TestExtensionBase
    {

        public virtual Dictionary<string, string> DefaultSettings { get; } = new Dictionary<string, string>() { { "Test key", "Test value" } };

        public virtual inRiverContext Context { get; set; }

        public virtual string Test() => $@"Extension '{Context?.ExtensionId}' with assembly namespace {GetType().FullName} works!";

    }
}
