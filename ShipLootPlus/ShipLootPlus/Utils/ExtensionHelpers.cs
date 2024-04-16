using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtensionHelpers
{
    /// <summary>
    /// Gets the value or default of a nullable bool
    /// </summary>
    /// <param name="nullableBool"></param>
    /// <returns></returns>
    public static bool GetValueOrDefault(this bool? InputNullableBool)
    {
        return InputNullableBool ?? false;
    }
}
