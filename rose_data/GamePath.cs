using System;
using System.Collections.Generic;
using System.IO;

namespace rose_data
{
    /// <summary>
    /// Turns the paths the game data uses into paths that exist on disk.
    ///
    /// The data files name each other the Windows way: backslashes, any mix of
    /// upper and lower case, relative to the game's root directory. On a file
    /// system that tells upper and lower case apart those names only open when
    /// they are matched against what is really there, one path segment at a time.
    /// </summary>
    public static class GamePath
    {
        private static readonly char[] Separators = { '\\', '/' };

        // What each directory looked at so far contains. Null for a directory
        // that does not exist.
        private static readonly Dictionary<string, Listing> Listings =
            new Dictionary<string, Listing>(StringComparer.Ordinal);

        /// <summary>
        /// The path on disk of <paramref name="gamePath"/> below the configured
        /// root directory.
        /// </summary>
        public static string Resolve(string gamePath)
        {
            return Resolve(Config.RootDirectory, gamePath);
        }

        /// <summary>
        /// The path on disk of <paramref name="gamePath"/> below
        /// <paramref name="root"/>. A segment that matches nothing is kept as
        /// given, so opening a missing file fails with its name in the error.
        /// </summary>
        public static string Resolve(string root, string gamePath)
        {
            var current = string.IsNullOrEmpty(root) ? "." : root;
            if (string.IsNullOrEmpty(gamePath))
                return current;

            foreach (var segment in gamePath.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
            {
                if (segment == ".")
                    continue;

                var name = segment == ".." ? segment : NameIn(current, segment);
                current = Path.Combine(current, name);
            }

            return current;
        }

        /// <summary>
        /// The name <paramref name="segment"/> has in <paramref name="directory"/>:
        /// itself if it is there spelled that way, else a name that differs
        /// from it only in case, else itself.
        /// </summary>
        private static string NameIn(string directory, string segment)
        {
            var listing = List(directory);
            if (listing == null || listing.Exact.Contains(segment))
                return segment;
            return listing.AnyCase.TryGetValue(segment, out var actual) ? actual : segment;
        }

        private static Listing List(string directory)
        {
            if (Listings.TryGetValue(directory, out var listing))
                return listing;

            if (Directory.Exists(directory))
            {
                var names = new List<string>();
                foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
                    names.Add(Path.GetFileName(entry));
                // Names that differ only in case can sit side by side. Sorted,
                // the same one of them is picked on every run.
                names.Sort(StringComparer.Ordinal);
                listing = new Listing();
                foreach (var name in names)
                {
                    listing.Exact.Add(name);
                    listing.AnyCase.TryAdd(name, name);
                }
            }

            Listings[directory] = listing;
            return listing;
        }

        private sealed class Listing
        {
            public readonly HashSet<string> Exact = new HashSet<string>(StringComparer.Ordinal);

            public readonly Dictionary<string, string> AnyCase =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
