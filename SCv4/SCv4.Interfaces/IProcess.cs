/* 
ServerChecker v4 operates and manages various kinds of software on server systems.
Copyright (C) 2010 Stijn Devriendt

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;

namespace SC.Interfaces
{
    public interface IProcess
    {
        //
        // Summary:
        //     Gets the base priority of the associated process.
        //
        // Returns:
        //     The base priority, which is computed from the System.Diagnostics.Process.PriorityClass
        //     of the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     the System.Diagnostics.ProcessStartInfo.UseShellExecute property to false
        //     to access this property on Windows 98 and Windows Me.
        //
        //   System.InvalidOperationException:
        //     The process has exited.-or- The process has not started, so there is no process
        //     ID.
        int BasePriority { get; }
        //
        // Summary:
        //     Gets or sets whether the System.Diagnostics.Process.Exited event should be
        //     raised when the process terminates.
        //
        // Returns:
        //     true if the System.Diagnostics.Process.Exited event should be raised when
        //     the associated process is terminated (through either an exit or a call to
        //     System.Diagnostics.Process.Kill()); otherwise, false. The default is false.
        //bool EnableRaisingEvents { get; set; }
        //
        // Summary:
        //     Gets the value that the associated process specified when it terminated.
        //
        // Returns:
        //     The code that the associated process specified when it terminated.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The process has not exited.-or- The process System.Diagnostics.Process.Handle
        //     is not valid.
        int ExitCode { get; }
        //
        // Summary:
        //     Gets the time that the associated process exited.
        //
        // Returns:
        //     A System.DateTime that indicates when the associated process was terminated.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        DateTime ExitTime { get; }
        //
        // Summary:
        //     Returns the associated process's native handle.
        //
        // Returns:
        //     The handle that the operating system assigned to the associated process when
        //     the process was started. The system uses this handle to keep track of process
        //     attributes.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The process has not been started. The System.Diagnostics.Process.Handle property
        //     cannot be read because there is no process associated with this System.Diagnostics.Process
        //     instance.-or- The System.Diagnostics.Process instance has been attached to
        //     a running process but you do not have the necessary permissions to get a
        //     handle with full access rights.
        //
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.Handle property for
        //     a process running on a remote computer.
        IntPtr Handle { get; }
        //
        // Summary:
        //     Gets the number of handles opened by the process.
        //
        // Returns:
        //     The number of operating system handles the process has opened.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     the System.Diagnostics.ProcessStartInfo.UseShellExecute property to false
        //     to access this property on Windows 98 and Windows Me.
        int HandleCount { get; }
        //
        // Summary:
        //     Gets a value indicating whether the associated process has been terminated.
        //
        // Returns:
        //     true if the operating system process referenced by the System.Diagnostics.Process
        //     component has terminated; otherwise, false.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     There is no process associated with the object.
        //
        //   System.ComponentModel.Win32Exception:
        //     The exit code for the process could not be retrieved.
        bool HasExited { get; }
        //
        // Summary:
        //     Gets the unique identifier for the associated process.
        //
        // Returns:
        //     The system-generated unique identifier of the process that is referenced
        //     by this System.Diagnostics.Process instance.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     the System.Diagnostics.ProcessStartInfo.UseShellExecute property to false
        //     to access this property on Windows 98 and Windows Me.
        //
        //   System.InvalidOperationException:
        //     The process's System.Diagnostics.Process.Id property has not been set.-or-
        //     There is no process associated with this System.Diagnostics.Process object.
        int Id { get; }
        //
        // Summary:
        //     Gets the name of the computer the associated process is running on.
        //
        // Returns:
        //     The name of the computer that the associated process is running on.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     There is no process associated with this System.Diagnostics.Process object.
        string MachineName { get; }
        //
        // Summary:
        //     Gets the main module for the associated process.
        //
        // Returns:
        //     The System.Diagnostics.ProcessModule that was used to start the process.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     You are attempting to access this property for a process on a remote computer.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        ProcessModule MainModule { get; }
        //
        // Summary:
        //     Gets the window handle of the main window of the associated process.
        //
        // Returns:
        //     The system-generated window handle of the main window of the associated process.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.MainWindowHandle is not defined because the
        //     process has exited.
        //
        //   System.NotSupportedException:
        //     You are attempting to retrieve the System.Diagnostics.Process.MainWindowHandle
        //     for a process that is running on a remote computer.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        IntPtr MainWindowHandle { get; }
        //
        // Summary:
        //     Gets the caption of the main window of the process.
        //
        // Returns:
        //     The process's main window title.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        string MainWindowTitle { get; }
        //
        // Summary:
        //     Gets or sets the maximum allowable working set size for the associated process.
        //
        // Returns:
        //     The maximum working set size that is allowed in memory for the process, in
        //     bytes.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     Working set information cannot be retrieved from the associated process resource.-or-
        //     The process identifier or process handle is zero because the process has
        //     not been started.
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.MaxWorkingSet
        //     property for a process that is running on a remote computer. The property
        //     is available only for processes running on the local computer.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.-or- The process
        //     has exited.
        IntPtr MaxWorkingSet { get; set; }
        //
        // Summary:
        //     Gets or sets the minimum allowable working set size for the associated process.
        //
        // Returns:
        //     The minimum working set size that is required in memory for the process,
        //     in bytes.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     Working set information cannot be retrieved from the associated process resource.-or-
        //     The process identifier or process handle is zero because the process has
        //     not been started.
        //
        //   System.SystemException:
        //     You are attempting to access the System.Diagnostics.Process.MaxWorkingSet
        //     property for a process that is running on a remote computer. The property
        //     is available only for processes running on the local computer.-or- The process
        //     System.Diagnostics.Process.Id is not available.-or- The process has exited.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        IntPtr MinWorkingSet { get; set; }
        //
        // Summary:
        //     Gets the modules that have been loaded by the associated process.
        //
        // Returns:
        //     An array of type System.Diagnostics.ProcessModule that represents the modules
        //     that have been loaded by the associated process.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.Modules property
        //     for a process that is running on a remote computer. The property is available
        //     only for processes running on the local computer.-or- The process System.Diagnostics.Process.Id
        //     is not available.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        ProcessModuleCollection Modules { get; }
        //
        // Summary:
        //     Gets the nonpaged system memory size allocated to this process.
        //
        // Returns:
        //     The amount of memory the system has allocated for the associated process
        //     that cannot be written to the virtual memory paging file.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long NonpagedSystemMemorySize64 { get; }
        //
        // Summary:
        //     Gets the amount of paged memory allocated for the associated process.
        //
        // Returns:
        //     The amount of memory, in bytes, allocated in the virtual memory paging file
        //     for the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PagedMemorySize64 { get; }
        //
        // Summary:
        //     Gets the amount of pageable system memory allocated for the associated process.
        //
        // Returns:
        //     The amount of system memory, in bytes, allocated for the associated process
        //     that can be written to the virtual memory paging file.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PagedSystemMemorySize64 { get; }
        //
        // Summary:
        //     Gets the maximum amount of memory in the virtual memory paging file used
        //     by the associated process.
        //
        // Returns:
        //     The maximum amount of memory, in bytes, allocated in the virtual memory paging
        //     file for the associated process since it was started.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PeakPagedMemorySize64 { get; }
        //
        // Summary:
        //     Gets the maximum amount of virtual memory used by the associated process.
        //
        // Returns:
        //     The maximum amount of virtual memory, in bytes, allocated for the associated
        //     process since it was started.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PeakVirtualMemorySize64 { get; }
        //
        // Summary:
        //     Gets the maximum amount of physical memory used by the associated process.
        //
        // Returns:
        //     The maximum amount of physical memory, in bytes, allocated for the associated
        //     process since it was started.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PeakWorkingSet64 { get; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the associated process priority should
        //     temporarily be boosted by the operating system when the main window has the
        //     focus.
        //
        // Returns:
        //     true if dynamic boosting of the process priority should take place for a
        //     process when it is taken out of the wait state; otherwise, false. The default
        //     is false.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.-or- The process identifier or process handle
        //     is zero. (The process has not been started.)
        //
        //   System.ComponentModel.Win32Exception:
        //     Priority boost information could not be retrieved from the associated process
        //     resource.
        //
        //   System.SystemException:
        //     You are attempting to access the System.Diagnostics.Process.PriorityBoostEnabled
        //     property for a process that is running on a remote computer. The property
        //     is available only for processes running on the local computer.-or- The process
        //     System.Diagnostics.Process.Id is not available.
        bool PriorityBoostEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the overall priority category for the associated process.
        //
        // Returns:
        //     The priority category for the associated process, from which the System.Diagnostics.Process.BasePriority
        //     of the process is calculated.
        //
        // Exceptions:
        //   System.SystemException:
        //     You are attempting to access the System.Diagnostics.Process.PriorityClass
        //     property for a process that is running on a remote computer. The property
        //     is available only for processes running on the local computer.-or- The process
        //     System.Diagnostics.Process.Id was not available.
        //
        //   System.ComponentModel.Win32Exception:
        //     Process priority information could not be set or retrieved from the associated
        //     process resource.-or- The process identifier or process handle is zero. (The
        //     process has not been started.)
        //
        //   System.PlatformNotSupportedException:
        //     You have set the System.Diagnostics.Process.PriorityClass to AboveNormal
        //     or BelowNormal when using Windows 98 or Windows Millennium Edition (Windows
        //     Me). These platforms do not support those values for the priority class.
        ProcessPriorityClass PriorityClass { get; set; }
        //
        // Summary:
        //     Gets the amount of private memory allocated for the associated process.
        //
        // Returns:
        //     The amount of memory, in bytes, allocated for the associated process that
        //     cannot be shared with other processes.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long PrivateMemorySize64 { get; }
        //
        // Summary:
        //     Gets the privileged processor time for this process.
        //
        // Returns:
        //     A System.TimeSpan that indicates the amount of time that the process has
        //     spent running code inside the operating system core.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        TimeSpan PrivilegedProcessorTime { get; }
        //
        // Summary:
        //     Gets the name of the process.
        //
        // Returns:
        //     The name that the system uses to identify the process to the user.
        //
        // Exceptions:
        //   System.SystemException:
        //     The process does not have an identifier, or no process is associated with
        //     the System.Diagnostics.Process.-or- The associated process has exited.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        string ProcessName { get; }
        //
        // Summary:
        //     Gets or sets the processors on which the threads in this process can be scheduled
        //     to run.
        //
        // Returns:
        //     A bitmask representing the processors that the threads in the associated
        //     process can run on. The default depends on the number of processors on the
        //     computer. The default value is 2 n -1, where n is the number of processors.
        //
        // Exceptions:
        //   System.SystemException:
        //     The process System.Diagnostics.Process.Id was not available.-or- The process
        //     has exited.
        //
        //   System.ComponentModel.Win32Exception:
        //     System.Diagnostics.Process.ProcessorAffinity information could not be set
        //     or retrieved from the associated process resource.-or- The process identifier
        //     or process handle is zero. (The process has not been started.)
        IntPtr ProcessorAffinity { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the user interface of the process is responding.
        //
        // Returns:
        //     true if the user interface of the associated process is responding to the
        //     system; otherwise, false.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        //
        //   System.InvalidOperationException:
        //     There is no process associated with this System.Diagnostics.Process object.
        bool Responding { get; }
        //
        // Summary:
        //     Gets the Terminal Services session identifier for the associated process.
        //
        // Returns:
        //     The Terminal Services session identifier for the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The System.Diagnostics.Process.SessionId property is not supported on Windows
        //     98.
        //
        //   System.InvalidOperationException:
        //     There is no process associated with this session identifier.-or-The associated
        //     process is not on this machine.
        int SessionId { get; }
        //
        // Summary:
        //     Gets a stream used to read the error output of the application.
        //
        // Returns:
        //     A System.IO.StreamReader that can be used to read the standard error stream
        //     of the application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardError stream has not been defined
        //     for redirection; ensure System.Diagnostics.ProcessStartInfo.RedirectStandardError
        //     is set to true and System.Diagnostics.ProcessStartInfo.UseShellExecute is
        //     set to false.- or - The System.Diagnostics.Process.StandardError stream has
        //     been opened for asynchronous read operations with System.Diagnostics.Process.BeginErrorReadLine().
        //StreamReader StandardError { get; }
        //
        // Summary:
        //     Gets a stream used to write the input of the application.
        //
        // Returns:
        //     A System.IO.StreamWriter that can be used to write the standard input stream
        //     of the application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardInput stream has not been defined
        //     because System.Diagnostics.ProcessStartInfo.RedirectStandardInput is set
        //     to false.
        //StreamWriter StandardInput { get; }
        //
        // Summary:
        //     Gets a stream used to read the output of the application.
        //
        // Returns:
        //     A System.IO.StreamReader that can be used to read the standard output stream
        //     of the application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardOutput stream has not been defined
        //     for redirection; ensure System.Diagnostics.ProcessStartInfo.RedirectStandardOutput
        //     is set to true and System.Diagnostics.ProcessStartInfo.UseShellExecute is
        //     set to false.- or - The System.Diagnostics.Process.StandardOutput stream
        //     has been opened for asynchronous read operations with System.Diagnostics.Process.BeginOutputReadLine().
        //StreamReader StandardOutput { get; }
        //
        // Summary:
        //     Gets or sets the properties to pass to the System.Diagnostics.Process.Start()
        //     method of the System.Diagnostics.Process.
        //
        // Returns:
        //     The System.Diagnostics.ProcessStartInfo that represents the data with which
        //     to start the process. These arguments include the name of the executable
        //     file or document used to start the process.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The value that specifies the System.Diagnostics.Process.StartInfo is null.
        ProcessStartInfo StartInfo { get; set; }
        //
        // Summary:
        //     Gets the time that the associated process was started.
        //
        // Returns:
        //     A System.DateTime that indicates when the process started. This only has
        //     meaning for started processes.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        DateTime StartTime { get; }
        //
        // Summary:
        //     Gets or sets the object used to marshal the event handler calls that are
        //     issued as a result of a process exit event.
        //
        // Returns:
        //     The System.ComponentModel.ISynchronizeInvoke used to marshal event handler
        //     calls that are issued as a result of an System.Diagnostics.Process.Exited
        //     event on the process.
        //ISynchronizeInvoke SynchronizingObject { get; set; }
        //
        // Summary:
        //     Gets the set of threads that are running in the associated process.
        //
        // Returns:
        //     An array of type System.Diagnostics.ProcessThread representing the operating
        //     system threads currently running in the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        //
        //   System.SystemException:
        //     The process does not have an System.Diagnostics.Process.Id, or no process
        //     is associated with the System.Diagnostics.Process instance.-or- The associated
        //     process has exited.
        ProcessThreadCollection Threads { get; }
        //
        // Summary:
        //     Gets the total processor time for this process.
        //
        // Returns:
        //     A System.TimeSpan that indicates the amount of time that the associated process
        //     has spent utilizing the CPU. This value is the sum of the System.Diagnostics.Process.UserProcessorTime
        //     and the System.Diagnostics.Process.PrivilegedProcessorTime.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        TimeSpan TotalProcessorTime { get; }
        //
        // Summary:
        //     Gets the user processor time for this process.
        //
        // Returns:
        //     A System.TimeSpan that indicates the amount of time that the associated process
        //     has spent running code inside the application portion of the process (not
        //     inside the operating system core).
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        TimeSpan UserProcessorTime { get; }
        //
        // Summary:
        //     Gets the amount of the virtual memory allocated for the associated process.
        //
        // Returns:
        //     The amount of virtual memory, in bytes, allocated for the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long VirtualMemorySize64 { get; }
        //
        // Summary:
        //     Gets the amount of physical memory allocated for the associated process.
        //
        // Returns:
        //     The amount of physical memory, in bytes, allocated for the associated process.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        long WorkingSet64 { get; }
        //
        // Summary:
        //     Occurs when an application writes to its redirected System.Diagnostics.Process.StandardError
        //     stream.
        //event DataReceivedEventHandler ErrorDataReceived;
        //
        // Summary:
        //     Occurs when a process exits.
        //event EventHandler Exited;
        //
        // Summary:
        //     Occurs when an application writes to its redirected System.Diagnostics.Process.StandardOutput
        //     stream.
        //event DataReceivedEventHandler OutputDataReceived;
        //
        // Summary:
        //     Begins asynchronous read operations on the redirected System.Diagnostics.Process.StandardError
        //     stream of the application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.ProcessStartInfo.RedirectStandardError property is
        //     false.- or - An asynchronous read operation is already in progress on the
        //     System.Diagnostics.Process.StandardError stream.- or - The System.Diagnostics.Process.StandardError
        //     stream has been used by a synchronous read operation.
        //void BeginErrorReadLine();
        //
        // Summary:
        //     Begins asynchronous read operations on the redirected System.Diagnostics.Process.StandardOutput
        //     stream of the application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.ProcessStartInfo.RedirectStandardOutput property is
        //     false.- or - An asynchronous read operation is already in progress on the
        //     System.Diagnostics.Process.StandardOutput stream.- or - The System.Diagnostics.Process.StandardOutput
        //     stream has been used by a synchronous read operation.
        //void BeginOutputReadLine();
        //
        // Summary:
        //     Cancels the asynchronous read operation on the redirected System.Diagnostics.Process.StandardError
        //     stream of an application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardError stream is not enabled for asynchronous
        //     read operations.
        //void CancelErrorRead();
        //
        // Summary:
        //     Cancels the asynchronous read operation on the redirected System.Diagnostics.Process.StandardOutput
        //     stream of an application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardOutput stream is not enabled for asynchronous
        //     read operations.
        //void CancelOutputRead();
        //
        // Summary:
        //     Frees all the resources that are associated with this component.
        //void Close();
        //
        // Summary:
        //     Closes a process that has a user interface by sending a close message to
        //     its main window.
        //
        // Returns:
        //     true if the close message was successfully sent; false if the associated
        //     process does not have a main window or if the main window is disabled (for
        //     example if a modal dialog is being shown).
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     the System.Diagnostics.ProcessStartInfo.UseShellExecute property to false
        //     to access this property on Windows 98 and Windows Me.
        //bool CloseMainWindow();
        //
        // Summary:
        //     Immediately stops the associated process.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     The associated process could not be terminated. -or-The process is terminating.-or-
        //     The associated process is a Win16 executable.
        //
        //   System.SystemException:
        //     No process System.Diagnostics.Process.Id has been set, and a System.Diagnostics.Process.Handle
        //     from which the System.Diagnostics.Process.Id property can be determined does
        //     not exist.-or- There is no process associated with this System.Diagnostics.Process
        //     object.-or- You are attempting to call System.Diagnostics.Process.Kill()
        //     for a process that is running on a remote computer. The method is available
        //     only for processes running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process has already exited.
        void Kill();
        //
        // Summary:
        //     Discards any information about the associated process that has been cached
        //     inside the process component.
        void Refresh();
        //
        // Summary:
        //     Starts (or reuses) the process resource that is specified by the System.Diagnostics.Process.StartInfo
        //     property of this System.Diagnostics.Process component and associates it with
        //     the component.
        //
        // Returns:
        //     true if a process resource is started; false if no new process resource is
        //     started (for example, if an existing process is reused).
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.InvalidOperationException:
        //     No file name was specified in the System.Diagnostics.Process component's
        //     System.Diagnostics.Process.StartInfo.-or- The System.Diagnostics.ProcessStartInfo.UseShellExecute
        //     member of the System.Diagnostics.Process.StartInfo property is true while
        //     System.Diagnostics.ProcessStartInfo.RedirectStandardInput, System.Diagnostics.ProcessStartInfo.RedirectStandardOutput,
        //     or System.Diagnostics.ProcessStartInfo.RedirectStandardError is true.
        //bool Start();
        //
        // Summary:
        //     Instructs the System.Diagnostics.Process component to wait indefinitely for
        //     the associated process to exit.
        //
        // Exceptions:
        //   System.SystemException:
        //     No process System.Diagnostics.Process.Id has been set, and a System.Diagnostics.Process.Handle
        //     from which the System.Diagnostics.Process.Id property can be determined does
        //     not exist.-or- There is no process associated with this System.Diagnostics.Process
        //     object.-or- You are attempting to call System.Diagnostics.Process.WaitForExit(System.Int32)
        //     for a process running on a remote computer. The method is available only
        //     for processes that are running on the local computer.
        //
        //   System.ComponentModel.Win32Exception:
        //     The wait setting could not be accessed.
        //void WaitForExit();
        //
        // Summary:
        //     Instructs the System.Diagnostics.Process component to wait the specified
        //     number of milliseconds for the associated process to exit.
        //
        // Parameters:
        //   milliseconds:
        //     The amount of time, in milliseconds, to wait for the associated process to
        //     exit. The maximum is the largest possible value of a 32-bit integer, which
        //     represents infinity to the operating system.
        //
        // Returns:
        //     true if the associated process has exited; otherwise, false.
        //
        // Exceptions:
        //   System.SystemException:
        //     No process System.Diagnostics.Process.Id has been set, and a System.Diagnostics.Process.Handle
        //     from which the System.Diagnostics.Process.Id property can be determined does
        //     not exist.-or- There is no process associated with this System.Diagnostics.Process
        //     object.-or- You are attempting to call System.Diagnostics.Process.WaitForExit(System.Int32)
        //     for a process running on a remote computer. The method is only available
        //     for processes that are running on the local computer.
        //
        //   System.ComponentModel.Win32Exception:
        //     The wait setting could not be accessed.
        //bool WaitForExit(int milliseconds);
        //
        // Summary:
        //     Causes the System.Diagnostics.Process component to wait indefinitely for
        //     the associated process to enter an idle state. This overload applies only
        //     to processes with a user interface and, therefore, a message loop.
        //
        // Returns:
        //     true if the associated process has reached an idle state; otherwise, false.
        //bool WaitForInputIdle();
        //
        // Summary:
        //     Causes the System.Diagnostics.Process component to wait the specified number
        //     of milliseconds for the associated process to enter an idle state. This overload
        //     applies only to processes with a user interface and, therefore, a message
        //     loop.
        //
        // Parameters:
        //   milliseconds:
        //     The amount of time, in milliseconds, to wait for the associated process to
        //     become idle. The maximum is the largest possible value of a 32-bit integer,
        //     which represents infinity to the operating system.
        //
        // Returns:
        //     true if the associated process has reached an idle state; otherwise, false.
        //bool WaitForInputIdle(int milliseconds);
    }
}
