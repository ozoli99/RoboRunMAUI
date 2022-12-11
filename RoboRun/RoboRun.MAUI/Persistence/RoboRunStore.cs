namespace RoboRun.Persistence
{
    public class RoboRunStore : IStore
    {
        public async Task<IEnumerable<string>> GetFilesAsync()
        {
            return await Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
                                                 .Select(Path.GetFileName)
                                                 .Where(name => name?.EndsWith(".stl") ?? false)
                                                 .OfType<string>());
        }

        public async Task<DateTime> GetModifiedTimeAsync(string name)
        {
            var info = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}
