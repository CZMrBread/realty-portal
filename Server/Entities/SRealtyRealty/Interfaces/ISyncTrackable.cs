namespace Server.Entities.SRealtyRealty.Interfaces;

/// <summary>
/// Tracks synchronization state with external APIs
/// </summary>
public interface ISyncTrackable
{
    bool IsSynced { get; }
    DateTime? LastSyncedAt { get; }
    string? LastSyncError { get; }
    int SyncAttemptCount { get; }
    
    void MarkSyncSuccess();
    void MarkSyncFailure(string error);
    void IncrementSyncAttempts();
    void ResetSyncAttempts();
}