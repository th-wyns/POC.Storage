
namespace POC.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public enum ContentDeletionStateEnum
    {
        Undefined,
        ErrorDocumentDeletion,
        ErrorFileDeletion,
        ErrorIndexDeletion,
        ErrorBinaryDeletion,
        SucceededDocumentDeletion,
        SucceededFileDeletion,
        SucceededIndexDeletion,
        SucceededBinaryDeletion,
    }
}
