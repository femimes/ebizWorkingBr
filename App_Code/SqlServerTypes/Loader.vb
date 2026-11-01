Imports System
Imports System.IO
Imports System.Runtime.InteropServices



Namespace SqlServerTypes
    Public Class Utilities



        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function LoadLibrary(ByVal libname As String) As IntPtr

        End Function

        Public Shared Sub LoadNativeAssemblies(ByVal rootApplicationPath As String)
            Dim nativeBinaryPath = If(IntPtr.Size > 4, Path.Combine(rootApplicationPath, "SqlServerTypes\x64\"), Path.Combine(rootApplicationPath, "SqlServerTypes\x86\"))
            LoadNativeAssembly(nativeBinaryPath, "msvcr120.dll")
            LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial140.dll")
        End Sub

        Private Shared Sub LoadNativeAssembly(ByVal nativeBinaryPath As String, ByVal assemblyName As String)
            Dim path = System.IO.Path.Combine(nativeBinaryPath, assemblyName)
            Dim ptr = LoadLibrary(path)

            If ptr = IntPtr.Zero Then
                Throw New Exception(String.Format("Error loading {0} (ErrorCode: {1})", assemblyName, Marshal.GetLastWin32Error()))
            End If
        End Sub
    End Class
End Namespace
