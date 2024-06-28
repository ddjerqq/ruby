using System.Reflection;

namespace Ruby.Common;

public static class Ruby
{
    public static Assembly[] Assemblies =>
    [
        Assembly.Load("Domain"),
        Assembly.Load("Application"),
        Assembly.Load("Infrastructure"),
    ];
}