using System;
using System.Runtime.Loader;
using System.Reflection;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        try
        {
            return LoadUnmanagedDll(absolutePath);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        try
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        throw new NotImplementedException();
    }
}
