using AnyDiff;
using AnyDiff.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Extensions
{
    public static class DiffExtension
    {
        public const string LOG_PREFIX = "DiffComparer";

        public static List<Difference> GetDifferences<ObjectType>(this ObjectType originalObject, ObjectType newObject)
        {
            List<Difference> differences = new();
            ICollection<AnyDiff.Difference> foundDifferences = originalObject.Diff(newObject);

            foreach (AnyDiff.Difference foundDifference in foundDifferences)
            {
                differences.Add(new Difference(foundDifference));
            }

            return differences;
        }
    }

    public class Difference
    {
        public string Property { get; set; }
        public Type PropertyType { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
        public string Path { get; set; }
        public object Delta { get; set; }

        public DifferenceType Type { get; set; }

        public Difference(AnyDiff.Difference originalDiff)
        {
            Property = originalDiff.Property;
            PropertyType = originalDiff.PropertyType;
            OriginalValue = originalDiff.LeftValue;
            NewValue = originalDiff.RightValue;
            Path = originalDiff.Path;
            Delta = originalDiff.Delta;

            if (OriginalValue == null)
            {
                Type = DifferenceType.Added;
            }
            else if (NewValue == null)
            {
                Type = DifferenceType.Removed;
            }
            else
            {
                Type = DifferenceType.Changed;
            }
        }
    }

    public enum DifferenceType
    {
        Changed,
        Added,
        Removed
    }
}
