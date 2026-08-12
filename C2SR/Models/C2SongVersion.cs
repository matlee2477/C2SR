using System.Diagnostics.CodeAnalysis;

namespace C2SR.Models
{
    public readonly struct C2SongVersion : IEquatable<C2SongVersion>, IComparable, IComparable<C2SongVersion>
    {
        // Properties
        public required int Major { get; init; }
        public required int Minor { get; init; }
        public required int Revision { get; init; }

        #region Methods
        public override string ToString()
        {
            if (Revision > 0)
            {
                return $"{Major}.{Minor}.{Revision}";
            }
            else
            {
                return $"{Major}.{Minor}";
            }
        }

        public static bool TryParse(string versionString, out C2SongVersion version)
        {
            version = Empty;
            var parts = versionString.Split('.');
            if (parts.Length == 3)
            {
                if (!int.TryParse(parts[0], out int major) ||
                    !int.TryParse(parts[1], out int minor) ||
                    !int.TryParse(parts[2], out int revision))
                {
                    return false;
                }

                version = new C2SongVersion
                {
                    Major = major,
                    Minor = minor,
                    Revision = revision
                };
                return true;
            }
            else if (parts.Length == 2)
            {
                if (!int.TryParse(parts[0],out int major) ||
                    !int.TryParse(parts[1], out int minor))
                {
                    return false;
                }

                version = new C2SongVersion
                {
                    Major = major,
                    Minor = minor,
                    Revision = 0
                };
                return true;
            }
            else
            {
                return false;
            }
        }

        public static C2SongVersion Parse(string versionString)
        {
            if (!TryParse(versionString, out C2SongVersion version))
            {
                throw new FormatException($"Invalid version string: {versionString}");
            }

            return version;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor, Revision);
        }

        #endregion

        #region Interface Implementations
        public int CompareTo(object? obj)
        {
            return obj is C2SongVersion other ? CompareTo(other) : throw new ArgumentException("Object is not a C2SongVersion");
        }

        public int CompareTo(C2SongVersion other)
        {
            if (Major != other.Major)
            {
                return Major.CompareTo(other.Major);
            }

            if (Minor != other.Minor)
            {
                return Minor.CompareTo(other.Minor);
            }

            return Revision.CompareTo(other.Revision);
        }

        public bool Equals(C2SongVersion other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Operators
        public static bool operator ==(C2SongVersion left, C2SongVersion right) => left.Equals(right);
        public static bool operator !=(C2SongVersion left, C2SongVersion right) => !left.Equals(right);
        public static bool operator <(C2SongVersion left, C2SongVersion right) => left.CompareTo(right) < 0;
        public static bool operator >(C2SongVersion left, C2SongVersion right) => left.CompareTo(right) > 0;
        public static bool operator <=(C2SongVersion left, C2SongVersion right) => left.CompareTo(right) <= 0;
        public static bool operator >=(C2SongVersion left, C2SongVersion right) => left.CompareTo(right) >= 0;

        #endregion

        public static readonly C2SongVersion Empty = new() { Major = 0, Minor = 0, Revision = 0 };
    }
}
