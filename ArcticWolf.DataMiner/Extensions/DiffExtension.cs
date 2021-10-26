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
            List<Difference> differences = new List<Difference>();
            PropertyInfo[] fi = originalObject.GetType().GetProperties();

            Log.Debug("Fields: " + fi.Length, LOG_PREFIX);

            foreach (PropertyInfo f in fi)
            {
                Difference v = new Difference();
                v.Property = f.Name;
                v.OriginalValue = f.GetValue(originalObject);
                v.NewValue = f.GetValue(newObject);

                if (!Equals(v.OriginalValue, v.NewValue))
                {
                    Log.Error($"(PropertyChanged) Property '{f.Name}' changed from {v.OriginalValue} to {v.NewValue}", LOG_PREFIX);

                    if (v.NewValue.GetType().IsClass)
                    {
                        Log.Debug("Oh boy, seems that we can do deeper in here", LOG_PREFIX);
                    }

                    differences.Add(v);
                }
                else
                {
                    Log.Debug($"(PropertyChanged) Property '{f.Name}' DIDN'T change from {v.OriginalValue} to {v.NewValue}", LOG_PREFIX);
                }
            }

            return differences;
        }
    }

    public class Difference
    {
        public string Property { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
    }
}
