namespace WY.Data.Utility
{
    /// <summary>
    /// Reader/Write locker type
    /// </summary>
    public enum ReaderWriteLockType
    {
        Read,
        Write,
        UpgradeableRead
    }
}
