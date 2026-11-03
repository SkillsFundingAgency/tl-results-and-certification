using Respawn;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers
{
    /// <summary>
    /// Wraps Respawn's <see cref="Respawner"/> (Respawn 6+), which replaced the obsolete Checkpoint API.
    /// </summary>
    public class DatabaseCheckpoint
    {
        private Respawner _respawner;

        public bool WithReseed { get; init; }

        public async Task Reset(string connectionString)
        {
            _respawner ??= await Respawner.CreateAsync(connectionString, new RespawnerOptions { WithReseed = WithReseed });
            await _respawner.ResetAsync(connectionString);
        }
    }
}
