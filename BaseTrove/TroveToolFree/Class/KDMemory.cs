using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KDMemory
{
    class KDMemoryLibrary
    {
        public class WINAPI
        {
            /// <summary>
            /// Provides access to Standard Access Rights.
            /// </summary>
            /// <remarks>See http://msdn.microsoft.com/library/aa379607.aspx for more information.</remarks>
            public enum StandardAccessRights
            {
                DELETE = 0x00010000,
                READ_CONTROL = 0x00020000,
                WRITE_DAC = 0x00040000,
                WRITE_OWNER = 0x00080000,
                SYNCHRONIZE = 0x00100000,
                STANDARD_RIGHTS_READ = READ_CONTROL,
                STANDARD_RIGHTS_WRITE = READ_CONTROL,
                STANDARD_RIGHTS_EXECUTE = READ_CONTROL,
                STANDARD_RIGHTS_REQUIRED = (DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER),
                STANDARD_RIGHTS_ALL = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE)
            }

            /// <summary>
            /// Provides access to SACL Access Right.
            /// </summary>
            /// <remarks>See http://msdn.microsoft.com/library/aa379321.aspx for more information.</remarks>
            public enum SACLAccessRight
            {
                ACCESS_SYSTEM_SECURITY = 0x01000000
            }

            /// <summary>
            /// Provides access to Process Access Rights.
            /// </summary>
            /// <remarks>See http://msdn.microsoft.com/library/ms684880.aspx for more information.</remarks>
            public enum ProcessAccessRights
            {
                PROCESS_TERMINATE = 0x0001,
                PROCESS_CREATE_THREAD = 0x0002,
                PROCESS_VM_OPERATION = 0x0008,
                PROCESS_VM_READ = 0x0010,
                PROCESS_VM_WRITE = 0x0020,
                PROCESS_DUP_HANDLE = 0x0040,
                PROCESS_CREATE_PROCESS = 0x0080,
                PROCESS_SET_QUOTA = 0x0100,
                PROCESS_SET_INFORMATION = 0x0200,
                PROCESS_QUERY_INFORMATION = 0x0400,
                PROCESS_SUSPEND_RESUME = 0x0800,
                PROCESS_QUERY_LIMITED_INFORMATION = 0x1000, // Windows Server 2003 and Windows XP: This access right is not supported.
                PROCESS_ALL_ACCESS_XP = (StandardAccessRights.STANDARD_RIGHTS_REQUIRED | StandardAccessRights.SYNCHRONIZE | 0x0FFF),
                PROCESS_ALL_ACCESS = (StandardAccessRights.STANDARD_RIGHTS_REQUIRED | StandardAccessRights.SYNCHRONIZE | 0xFFFF)
            };

            /// <summary>
            /// Opens an existing local process object.
            /// </summary>
            /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the <see cref="KDMemoryLibrary.WINAPI.ProcessAccessRights"/></param>
            /// <param name="inheritHandle">If this value is true, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
            /// <param name="processId">The identifier of the local process to be opened.</param>
            /// <returns>
            /// <para>If the function succeeds, the return value is an open handle to the specified process.</para><para>If the function fails, the return value is 0. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error()"/>.</para></returns>
            /// <remarks>See http://msdn.microsoft.com/library/ms684320.aspx for more information.</remarks>
            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern IntPtr OpenProcess(ProcessAccessRights desiredAccess, Boolean inheritHandle, Int32 processId);

            /// <summary>
            /// Closes an open object handle.
            /// </summary>
            /// <param name="objectHandle">A valid handle to an open object.</param>
            /// <returns><para>If the function succeeds, the return value is nonzero.</para><para>If the function fails, the return value is 0. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error()"/>.</para></returns>
            /// <remarks>See http://msdn.microsoft.com/library/ms724211.aspx for more information.</remarks>
            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern Int32 CloseHandle(IntPtr objectHandle);

            /// <summary>
            /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
            /// </summary>
            /// <param name="processHandle">A handle to the process with memory that is being read. The handle must have <see cref="KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_VM_READ"/> access to the process.</param>
            /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
            /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
            /// <param name="size">The number of bytes to be read from the specified process.</param>
            /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
            /// <returns><para>If the function succeeds, the return value is nonzero.</para><para>If the function fails, the return value is 0. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error()"/>.</para></returns>
            /// <remarks>See http://msdn.microsoft.com/library/ms680553.aspx for more information.</remarks>
            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern Int32 ReadProcessMemory(IntPtr processHandle, IntPtr address, Byte[] buffer, UIntPtr size, ref UIntPtr numberOfBytesRead);

            /// <summary>
            /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
            /// </summary>
            /// <param name="processHandle">A handle to the process memory to be modified. The handle must have <see cref="KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_VM_WRITE"/> and <see cref="KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_VM_OPERATION"/> access to the process.</param>
            /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
            /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
            /// <param name="size">The number of bytes to be written to the specified process.</param>
            /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process.</param>
            /// <returns><para>If the function succeeds, the return value is nonzero.</para><para>If the function fails, the return value is 0. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error()"/>.</para></returns>
            /// <remarks>See http://msdn.microsoft.com/library/ms681674.aspx for more information.</remarks>
            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern Int32 WriteProcessMemory(IntPtr processHandle, IntPtr address, Byte[] buffer, UIntPtr size, ref UIntPtr numberOfBytesWritten);
        }

        /// <summary>
        /// Represents system errors that occur during WINAPI execution. This class cannot be inherited.
        /// </summary>
        /// <remarks>See http://msdn.microsoft.com/library/ms681381.aspx for more information.</remarks>
        [SerializableAttribute]
        [ComVisibleAttribute(true)]
        public sealed class Win32ErrorException : System.Exception
        {
            /// <summary>
            /// Gets the WIN32 system error code.
            /// </summary>
            public Int32 SystemErrorCode { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="KDMemoryLibrary.Win32ErrorException"/> class with the system error code.
            /// </summary>
            /// <param name="systemErrorCode">The WIN32 system error code.</param>
            public Win32ErrorException(Int32 systemErrorCode)
                : this(systemErrorCode, null, null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="KDMemoryLibrary.Win32ErrorException"/> class with the system error code and a specified error message.
            /// </summary>
            /// <param name="systemErrorCode">The WIN32 system error code.</param>
            /// <param name="message">The message that describes the error.</param>
            public Win32ErrorException(Int32 systemErrorCode, String message)
                : this(systemErrorCode, message, null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="KDMemoryLibrary.Win32ErrorException"/> class with the system error code, a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="systemErrorCode">The WIN32 system error code.</param>
            /// <param name="message">The message that describes the error.</param>
            /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
            public Win32ErrorException(Int32 systemErrorCode, String message, Exception innerException)
                : base(message, innerException)
            {
                this.SystemErrorCode = systemErrorCode;
            }
        }

        public class KDMemory
        {
            /// <summary>
            /// Gets or sets the friendly name of the process.
            /// </summary>
            public String ProcessName { get; set; }

            /// <summary>
            /// Gets or sets the memory address to access data. This value cannot be accessed if <see cref="ModuleName"/>' is specified.
            /// </summary>
            protected IntPtr? address;
            public IntPtr? Address
            {
                get
                {
                    return this.address;
                }
                set
                {
                    if (this.moduleName == null)
                    {
                        this.address = value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the name of the module which base address will be used. This value cannot be accessed if <see cref="Address"/> is specified.
            /// </summary>
            protected String moduleName;
            public String ModuleName
            {
                get
                {
                    return this.moduleName;
                }
                set
                {
                    if (this.address == null)
                    {
                        this.moduleName = value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the static offset which is added to the module base address. This value cannot be accessed if <see cref="Address"/> is specified.
            /// </summary>
            protected Int32? staticOffset;
            public Int32? StaticOffset
            {
                get
                {
                    return this.staticOffset;
                }
                set
                {
                    if (this.address == null)
                    {
                        this.staticOffset = value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the offsets which are added to the address.
            /// </summary>
            public Int32[] Offsets { get; set; }

            /// <summary>
            /// Gets or sets the access to the process object.
            /// </summary>
            public WINAPI.ProcessAccessRights DesiredAccess { get; set; }

            /// <summary>
            /// Gets or sets the indicator if the occuring exception should be ignored.
            /// </summary>
            public Boolean IgnoreException { get; set; }

            /// <summary>
            /// Provides access to all processes with the specific name.
            /// </summary>
            /// <param name="processName">The friendly name of the process.</param>
            /// <param name="address">A memory address to access data.</param>
            /// <param name="offsets">The offsets which are added to the address.</param>
            /// <param name="desiredAccess">The access to the process object.</param>
            public KDMemory(String processName, IntPtr? address, Int32[] offsets, WINAPI.ProcessAccessRights desiredAccess)
            {
                this.ProcessName = System.IO.Path.GetFileNameWithoutExtension(processName);
                this.address = address;
                this.moduleName = null;
                this.staticOffset = null;
                this.Offsets = offsets;
                this.DesiredAccess = desiredAccess;
                this.IgnoreException = true;
            }

            /// <summary>
            /// Provides access to all processes with the specific name.
            /// </summary>
            /// <param name="processName">The friendly name of the process.</param>
            /// <param name="address">A memory address to access data.</param>
            /// <param name="desiredAccess">The access to the process object.</param>
            public KDMemory(String processName, IntPtr? address, WINAPI.ProcessAccessRights desiredAccess)
                : this(processName, address, null, desiredAccess) { }

            /// <summary>
            /// Provides access to all processes with the specific name.
            /// </summary>
            /// <param name="processName">The friendly name of the process.</param>
            /// <param name="moduleName">The name of the module which base address will be used.</param>
            /// <param name="staticOffset">The static offset which is added to the module base address.</param>
            /// <param name="offsets">The offsets which are added to the address.</param>
            /// <param name="desiredAccess">The access to the process object.</param>
            public KDMemory(String processName, String moduleName, Int32 staticOffset, Int32[] offsets, WINAPI.ProcessAccessRights desiredAccess)
                : this(processName, null, offsets, desiredAccess)
            {
                this.moduleName = moduleName;
                this.staticOffset = staticOffset;
            }

            /// <summary>
            /// Provides access to all processes with the specific name.
            /// </summary>
            /// <param name="processName">The friendly name of the process.</param>
            /// <param name="moduleName">The name of the module which base address will be used.</param>
            /// <param name="staticOffset">The static offset which is added to the module base address.</param>
            /// <param name="desiredAccess">The access to the process object.</param>
            public KDMemory(String processName, String moduleName, Int32 staticOffset, WINAPI.ProcessAccessRights desiredAccess)
                : this(processName, moduleName, staticOffset, null, desiredAccess) { }

            /// <summary>
            /// Gets the memory address where the module was loaded. Each element corresponds to its associated process.
            /// </summary>
            /// <param name="moduleName">The name of the module. If <see cref="moduleName"/> is an empty String, the return value is the address of the main module.</param>
            protected List<IntPtr> GetModuleBaseAddress(String moduleName)
            {
                var processes = Process.GetProcessesByName(this.ProcessName);
                var moduleAddresses = new List<IntPtr>();

                foreach (var process in processes)
                {
                    if (String.IsNullOrWhiteSpace(moduleName))
                    {
                        moduleAddresses.Add(process.MainModule.BaseAddress);
                    }
                    else
                    {
                        var moduleAddressAdded = false;
                        var processModuleCollection = process.Modules;
                        foreach (ProcessModule processModule in processModuleCollection)
                        {
                            if (processModule.ModuleName == moduleName)
                            {
                                moduleAddresses.Add(IntPtr.Add(processModule.BaseAddress, (Int32)this.staticOffset));
                                moduleAddressAdded = true;
                                break;
                            }
                        }
                        if (!moduleAddressAdded)
                        {
                            moduleAddresses.Add(IntPtr.Zero);
                        }
                    }
                }
                return moduleAddresses;
            }

            /// <summary>
            /// Gets the memory address where the main module was loaded. Each element corresponds to its associated process.
            /// </summary>
            protected List<IntPtr> GetModuleBaseAddress()
            {
                return this.GetModuleBaseAddress(String.Empty);
            }

            /// <summary>
            /// Opens all existing local processes with the specified name.
            /// </summary>
            /// <returns>A list with process handles for each process.</returns>
            protected List<IntPtr> GetHandles()
            {
                var processes = Process.GetProcessesByName(this.ProcessName);
                var processHandles = new List<IntPtr>();

                foreach (var process in processes)
                {
                    var processHandle = WINAPI.OpenProcess(this.DesiredAccess, false, process.Id);
                    var systemErrorCode = Marshal.GetLastWin32Error();
                    if ((this.IgnoreException) || (systemErrorCode == 0))
                    {
                        processHandles.Add(processHandle);
                    }
                    else
                    {
                        throw new Win32ErrorException(systemErrorCode);
                    }
                }
                return processHandles;
            }

            /// <summary>
            /// Closes all open process handles.
            /// </summary>
            /// <param name="processHandles">A list containing the process handles.</param>
            protected void CloseHandles(List<IntPtr> processHandles)
            {
                foreach (var processHandle in processHandles)
                {
                    WINAPI.CloseHandle(processHandle);
                    //var systemErrorCode = Marshal.GetLastWin32Error();
                    //if ((!this.IgnoreException) && (systemErrorCode != 0)) {
                    //    throw new Win32ErrorException(systemErrorCode);
                    //}
                }
            }

            protected List<IntPtr> GetPointers(out List<IntPtr> processHandles)
            {
                processHandles = GetHandles();
                List<IntPtr> pointers;

                if (this.address == null)
                {
                    pointers = this.GetModuleBaseAddress(this.moduleName);
                }
                else
                {
                    pointers = new List<IntPtr>();
                    for (var i = 0; i < processHandles.Count; i++)
                    {
                        pointers.Add((IntPtr)this.address);
                    }
                }

                for (var i = 0; i < processHandles.Count; i++)
                {
                    if (processHandles[i] == IntPtr.Zero)
                    {
                        pointers[i] = IntPtr.Zero;
                    }
                    else
                    {
                        if ((this.Offsets != null) && (this.Offsets.Length > 0))
                        {
                            var size = UIntPtr.Size;
                            var buffer = new Byte[size];
                            var bytesRead = UIntPtr.Zero;
                            foreach (var offset in this.Offsets)
                            {
                                WINAPI.ReadProcessMemory(processHandles[i], pointers[i], buffer, (UIntPtr)size, ref bytesRead);
                                if (!this.IgnoreException)
                                {
                                    Int32 systemErrorCode = Marshal.GetLastWin32Error();
                                    if (systemErrorCode != 0)
                                    {
                                        throw new Win32ErrorException(systemErrorCode);
                                    }
                                }

                                if (size == 8)
                                {
                                    pointers[i] = (IntPtr)(BitConverter.ToInt64(buffer, 0));
                                }
                                else
                                {
                                    pointers[i] = (IntPtr)(BitConverter.ToUInt32(buffer, 0));
                                }

                                if (pointers[i] == IntPtr.Zero)
                                {
                                    break;
                                }

                                pointers[i] = IntPtr.Add(pointers[i], offset);
                                //Debug
                                //Console.WriteLine(pointers[i]);
                            }
                        }
                    }
                }
                return pointers;
            }

            protected List<Byte[]> GetBytes(Int32 size, Int32 offset)
            {
                Process.EnterDebugMode();

                List<IntPtr> processHandles;
                var bytes = new List<Byte[]>();
                var pointers = GetPointers(out processHandles);

                for (var i = 0; i < pointers.Count; i++)
                {
                    bytes.Add(new Byte[size]);

                    if (pointers[i] != IntPtr.Zero)
                    {
                        var buffer = new Byte[size];
                        var bytesRead = UIntPtr.Zero;

                        WINAPI.ReadProcessMemory(processHandles[i], IntPtr.Add(pointers[i], offset), buffer, (UIntPtr)size, ref bytesRead);
                        if (!this.IgnoreException)
                        {
                            Int32 systemErrorCode = Marshal.GetLastWin32Error();
                            if (systemErrorCode != 0)
                            {
                                throw new Win32ErrorException(systemErrorCode);
                            }
                        }

                        for (var x = 0; x < buffer.Length; x++)
                        {
                            (bytes[i])[x] = buffer[x];
                        }
                    }
                }

                CloseHandles(processHandles);
                Process.LeaveDebugMode();
                return bytes;
            }

            protected List<Byte[]> GetBytes(Int32 size)
            {
                return this.GetBytes(size, 0);
            }

            /// <summary>
            /// Gets a list of boolean values. Each element corresponds to its associated process.
            /// </summary>
            public List<Boolean> GetBoolean()
            {
                var returnValue = new List<Boolean>();
                var bytes = this.GetBytes(sizeof(Boolean));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToBoolean(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of 16-bit signed integers. Each element corresponds to its associated process.
            /// </summary>
            public List<Int16> GetInt16()
            {
                var returnValue = new List<Int16>();
                var bytes = this.GetBytes(sizeof(Int16));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToInt16(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of 32-bit signed integers. Each element corresponds to its associated process.
            /// </summary>
            public List<Int32> GetInt32()
            {
                var returnValue = new List<Int32>();
                var bytes = this.GetBytes(sizeof(Int32));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToInt32(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of 64-bit signed integers. Each element corresponds to its associated process.
            /// </summary>
            public List<Int64> GetInt64()
            {
                var returnValue = new List<Int64>();
                var bytes = this.GetBytes(sizeof(Int64));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToInt64(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of single-precision floating point numbers. Each element corresponds to its associated process.
            /// </summary>
            public List<Single> GetSingle()
            {
                var returnValue = new List<Single>();
                var bytes = this.GetBytes(sizeof(Single));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToSingle(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of double-precision floating point numbers. Each element corresponds to its associated process.
            /// </summary>
            public List<Double> GetDouble()
            {
                var returnValue = new List<Double>();
                var bytes = this.GetBytes(sizeof(Double));
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(BitConverter.ToDouble(byteArray, 0));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of strings. Each element corresponds to its associated process.
            /// </summary>
            /// <param name="encoding">The encoding of the string.</param>
            /// <param name="lenght">The lenght of the string to be read.</param>
            public List<String> GetString(Encoding encoding, Int32 lenght)
            {
                var returnValue = new List<String>();
                var bytes = this.GetBytes(lenght);
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(encoding.GetString(byteArray, 0, lenght));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of ANSI strings. Each element corresponds to its associated process.
            /// </summary>
            /// <param name="lenght">The lenght of the string to be read.</param>
            public List<String> GetStringAnsi(Int32 lenght)
            {
                var returnValue = new List<String>();
                var bytes = this.GetBytes(lenght);
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(Encoding.Default.GetString(byteArray, 0, lenght));
                }
                return returnValue;
            }

            /// <summary>
            /// Gets a list of Unicode strings. Each element corresponds to its associated process.
            /// </summary>
            /// <param name="lenght">The lenght of the string to be read.</param>
            public List<String> GetStringUnicode(Int32 lenght)
            {
                var returnValue = new List<String>();
                var bytes = this.GetBytes(lenght);
                foreach (var byteArray in bytes)
                {
                    returnValue.Add(Encoding.Unicode.GetString(byteArray, 0, lenght));
                }
                return returnValue;
            }


            protected void SetBytes(Byte[] buffer)
            {
                Process.EnterDebugMode();

                List<IntPtr> processHandles;
                var pointers = GetPointers(out processHandles);

                for (var i = 0; i < pointers.Count; i++)
                {
                    if (pointers[i] != IntPtr.Zero)
                    {
                        var bytesWritten = UIntPtr.Zero;
                        WINAPI.WriteProcessMemory(processHandles[i], pointers[i], buffer, (UIntPtr)buffer.Length, ref bytesWritten);
                        if (!this.IgnoreException)
                        {
                            Int32 systemErrorCode = Marshal.GetLastWin32Error();
                            if (systemErrorCode != 0)
                            {
                                throw new Win32ErrorException(systemErrorCode);
                            }
                        }
                    }
                }

                CloseHandles(processHandles);
                Process.LeaveDebugMode();
            }

            /// <summary>
            /// Sets a boolean value for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetBoolean(Boolean value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a 16-bit signed integer for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetInt16(Int16 value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a 32-bit signed integer for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetInt32(Int32 value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a 64-bit signed integer for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetInt64(Int64 value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a single-precision floating point number for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetSingle(Single value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a double-precision floating point number for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetDouble(Double value)
            {
                Byte[] bytes = BitConverter.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a ANSI String for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetStringAnsi(String value)
            {
                Byte[] bytes = Encoding.Default.GetBytes(value);
                this.SetBytes(bytes);
            }

            /// <summary>
            /// Sets a Unicode String for all processes.
            /// </summary>
            /// <param name="value">The value to be set.</param>
            public void SetStringUnicode(String value)
            {
                Byte[] bytes = Encoding.Unicode.GetBytes(value);
                this.SetBytes(bytes);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void TesteGetBytes(Byte[] value)
            {
                this.SetBytes(value);
            }
            /// <summary>
            /// SetValues Byte[] Using For NOP :)
            /// </summary>
            /// <param name="value"></param>
            public void SetTheBytes(Byte[] value)
            {
                this.SetBytes(value);
            }
        }
    }
}
