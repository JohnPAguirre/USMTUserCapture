namespace USMTUserCapture.Interfaces
{
    /// <summary>
    /// All users located in the current computer
    /// </summary>
    public interface IUsersInfo
    {
        /// <summary>
        /// returns all the users on the computer
        /// </summary>
        string[] AllUsers { get; }
    }
}
