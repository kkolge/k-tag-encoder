using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

public class EmbeddedDll
{
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr LoadLibrary(string lpFileName);

    public static void Load(string embeddedResource, string fileName)
    {    
        byte[] ba = null;
        Assembly curAsm = Assembly.GetExecutingAssembly();

        string dirName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        if (!Directory.Exists(dirName))
            Directory.CreateDirectory(dirName);

        string dllPath = Path.Combine(dirName, fileName);

        //IntPtr h = LoadLibrary(dllPath);
        //if (h != IntPtr.Zero)
        //    return;

        test(dirName);

        using (Stream stm = curAsm.GetManifestResourceStream(embeddedResource))
        {
            // Either the file is not existed or it is not mark as embedded resource
            if (stm == null)
                throw new Exception(embeddedResource + " is not found in Embedded Resources.");

            // Get byte[] from the file from embedded resource
            ba = new byte[(int)stm.Length];
            stm.Read(ba, 0, (int)stm.Length);

            try
            {
                using (Stream outFile = File.Create(dllPath))
                {
                    outFile.Write(ba, 0, (int)stm.Length);
                }
            }
            catch (Exception e)
            {
                // This may happen if another process has already created and loaded the file.
                // Since the directory includes the version number of this assembly we can
                // assume that it's the same bits, so we just ignore the excecption here and
                // load the DLL.
                System.Console.WriteLine(e.ToString());
            }
        }

        IntPtr
        h = LoadLibrary(dllPath);
        if (h == IntPtr.Zero)
        {
            Exception e = new Win32Exception();
            throw new DllNotFoundException("Unable to load library: " + fileName + " from " + dllPath, e);
        }
    }

    private static bool GrantAccess(string fullPath)
    {
        DirectoryInfo dInfo = new DirectoryInfo(fullPath);
        DirectorySecurity dSecurity = dInfo.GetAccessControl();
        dSecurity.AddAccessRule(new FileSystemAccessRule(
            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            FileSystemRights.FullControl,
            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
            PropagationFlags.NoPropagateInherit,
            AccessControlType.Allow));
        dInfo.SetAccessControl(dSecurity);
        return true;
    }

    private static void test(string dirPath)
    {
        DirectoryInfo info = new DirectoryInfo(dirPath);
        WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
        DirectorySecurity ds = info.GetAccessControl();
        ds.AddAccessRule(new FileSystemAccessRule(self.Name,
            FileSystemRights.CreateFiles,
            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
            PropagationFlags.None,
            AccessControlType.Allow));
        info.SetAccessControl(ds);
    }
}