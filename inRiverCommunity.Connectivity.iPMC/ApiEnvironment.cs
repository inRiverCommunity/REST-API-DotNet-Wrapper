namespace inRiverCommunity.Connectivity.iPMC
{
    /// <summary>
    ///     Known API environments.
    /// </summary>
    public enum ApiEnvironment
    {
        /// <summary>
        ///     Customer environments, like: test, acceptance, production, etc.
        /// </summary>
        Customer,

        /// <summary>
        ///     Partner environments, like: sandbox1, sandbox2, etc.
        /// </summary>
        Partner,

        /// <summary>
        ///     Demo environment: hosted and managed by inRiver.
        /// </summary>
        Demo
    }
}