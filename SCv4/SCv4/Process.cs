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
using System.Security;
using System.Runtime.InteropServices;

namespace SC.Core
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Assert, Name="FullTrust")]
    class Process : SC.Interfaces.EternalMarshalByRefObject, SC.Interfaces.IProcess, IDisposable
    {
        private System.Diagnostics.Process _Process = null;
        internal Process() { _Process = new System.Diagnostics.Process(); }
        internal Process(System.Diagnostics.Process process) { _Process = process; }


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
        [MonitoringDescription("ProcessBasePriority")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BasePriority { get { return _Process.BasePriority; } }
        //
        // Summary:
        //     Gets or sets whether the System.Diagnostics.Process.Exited event should be
        //     raised when the process terminates.
        //
        // Returns:
        //     true if the System.Diagnostics.Process.Exited event should be raised when
        //     the associated process is terminated (through either an exit or a call to
        //     System.Diagnostics.Process.Kill()); otherwise, false. The default is false.
        [Browsable(false)]
        [DefaultValue(false)]
        [MonitoringDescription("ProcessEnableRaisingEvents")]
        public bool EnableRaisingEvents { get { return _Process.EnableRaisingEvents; } set { _Process.EnableRaisingEvents = value; } }
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
        //
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.ExitCode property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [MonitoringDescription("ProcessExitCode")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ExitCode { get { return _Process.ExitCode; } }
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
        //
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.ExitTime property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [Browsable(false)]
        [MonitoringDescription("ProcessExitTime")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime ExitTime { get { return _Process.ExitTime; } }
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
        //     a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [MonitoringDescription("ProcessHandle")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr Handle { get { return _Process.Handle; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessHandleCount")]
        public int HandleCount { get { return _Process.HandleCount; } }
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
        //
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.HasExited property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [MonitoringDescription("ProcessTerminated")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasExited { get { return _Process.HasExited; } }
        //
        // Summary:
        //     Gets the unique identifier for the associated process.
        //
        // Returns:
        //     The system-generated unique identifier of the process that is referenced
        //     by this System.Diagnostics.Process instance.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The process's System.Diagnostics.Process.Id property has not been set.-or-
        //     There is no process associated with this System.Diagnostics.Process object.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     the System.Diagnostics.ProcessStartInfo.UseShellExecute property to false
        //     to access this property on Windows 98 and Windows Me.
        [MonitoringDescription("ProcessId")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Id { get { return _Process.Id; } }
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
        [Browsable(false)]
        [MonitoringDescription("ProcessMachineName")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MachineName { get { return _Process.MachineName; } }
        //
        // Summary:
        //     Gets the main module for the associated process.
        //
        // Returns:
        //     The System.Diagnostics.ProcessModule that was used to start the process.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.MainModule property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [MonitoringDescription("ProcessMainModule")]
        public ProcessModule MainModule { get { return _Process.MainModule; } }
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
        //     You are trying to access the System.Diagnostics.Process.MainWindowHandle
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessMainWindowHandle")]
        public IntPtr MainWindowHandle { get { return _Process.MainWindowHandle; } }
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
        [MonitoringDescription("ProcessMainWindowTitle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MainWindowTitle { get { return _Process.MainWindowTitle; } }
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
        //     You are trying to access the System.Diagnostics.Process.MaxWorkingSet property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.-or- The process
        //     has exited.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessMaxWorkingSet")]
        public IntPtr MaxWorkingSet { get { return _Process.MaxWorkingSet; } set { _Process.MaxWorkingSet = value; } }
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
        //   System.NotSupportedException:
        //     You are trying to access the System.Diagnostics.Process.MinWorkingSet property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.-or- The process
        //     has exited.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessMinWorkingSet")]
        public IntPtr MinWorkingSet { get { return _Process.MinWorkingSet; } set { _Process.MinWorkingSet = value; } }
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
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        //
        //   System.ComponentModel.Win32Exception:
        //     You are attempting to access the System.Diagnostics.Process.Modules property
        //     for either the system process or the idle process. These processes do not
        //     have modules.
        [Browsable(false)]
        [MonitoringDescription("ProcessModules")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProcessModuleCollection Modules { get { return _Process.Modules; } }
        //
        // Summary:
        //     Gets the amount of nonpaged system memory allocated for the associated process.
        //
        // Returns:
        //     The amount of system memory, in bytes, allocated for the associated process
        //     that cannot be written to the virtual memory paging file.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.
        [ComVisible(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessNonpagedSystemMemorySize")]
        public long NonpagedSystemMemorySize64 { get { return _Process.NonpagedSystemMemorySize64; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessPagedMemorySize")]
        [ComVisible(false)]
        public long PagedMemorySize64 { get { return _Process.PagedMemorySize64; } }
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
        [MonitoringDescription("ProcessPagedSystemMemorySize")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ComVisible(false)]
        public long PagedSystemMemorySize64 { get { return _Process.PagedSystemMemorySize64; } }
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
        [MonitoringDescription("ProcessPeakPagedMemorySize")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ComVisible(false)]
        public long PeakPagedMemorySize64 { get { return _Process.PeakPagedMemorySize64; } }
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
        [MonitoringDescription("ProcessPeakVirtualMemorySize")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ComVisible(false)]
        public long PeakVirtualMemorySize64 { get { return _Process.PeakVirtualMemorySize64; } }
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
        [ComVisible(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessPeakWorkingSet")]
        public long PeakWorkingSet64 { get { return _Process.PeakWorkingSet64; } }
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
        //   System.ComponentModel.Win32Exception:
        //     Priority boost information could not be retrieved from the associated process
        //     resource.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me), which
        //     does not support this property.-or- The process identifier or process handle
        //     is zero. (The process has not been started.)
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.PriorityBoostEnabled
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessPriorityBoostEnabled")]
        public bool PriorityBoostEnabled { get { return _Process.PriorityBoostEnabled; } set { _Process.PriorityBoostEnabled = value; } }
        //
        // Summary:
        //     Gets or sets the overall priority category for the associated process.
        //
        // Returns:
        //     The priority category for the associated process, from which the System.Diagnostics.Process.BasePriority
        //     of the process is calculated.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     Process priority information could not be set or retrieved from the associated
        //     process resource.-or- The process identifier or process handle is zero. (The
        //     process has not been started.)
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.PriorityClass
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id is not available.
        //
        //   System.PlatformNotSupportedException:
        //     You have set the System.Diagnostics.Process.PriorityClass to AboveNormal
        //     or BelowNormal when using Windows 98 or Windows Millennium Edition (Windows
        //     Me). These platforms do not support those values for the priority class.
        //
        //   System.ComponentModel.InvalidEnumArgumentException:
        //     Priority class cannot be set because it does not use a valid value, as defined
        //     in the System.Diagnostics.ProcessPriorityClass enumeration.
        [MonitoringDescription("ProcessPriorityClass")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProcessPriorityClass PriorityClass { get { return _Process.PriorityClass; } set { _Process.PriorityClass = value; } }
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
        [MonitoringDescription("ProcessPrivateMemorySize")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ComVisible(false)]
        public long PrivateMemorySize64 { get { return _Process.PrivateMemorySize64; } }
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
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.PrivilegedProcessorTime
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessPrivilegedProcessorTime")]
        public TimeSpan PrivilegedProcessorTime { get { return _Process.PrivilegedProcessorTime; } }
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
        [MonitoringDescription("ProcessProcessName")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ProcessName { get { return _Process.ProcessName; } }
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
        //   System.ComponentModel.Win32Exception:
        //     System.Diagnostics.Process.ProcessorAffinity information could not be set
        //     or retrieved from the associated process resource.-or- The process identifier
        //     or process handle is zero. (The process has not been started.)
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.ProcessorAffinity
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process System.Diagnostics.Process.Id was not available.-or- The process
        //     has exited.
        [MonitoringDescription("ProcessProcessorAffinity")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr ProcessorAffinity { get { return _Process.ProcessorAffinity; } set { _Process.ProcessorAffinity = value; } }
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
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.Responding property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [MonitoringDescription("ProcessResponding")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Responding { get { return _Process.Responding; } }
        //
        // Summary:
        //     Gets the Terminal Services session identifier for the associated process.
        //
        // Returns:
        //     The Terminal Services session identifier for the associated process.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     There is no process associated with this session identifier.-or-The associated
        //     process is not on this machine.
        //
        //   System.PlatformNotSupportedException:
        //     The System.Diagnostics.Process.SessionId property is not supported on Windows 98.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessSessionId")]
        public int SessionId { get { return _Process.SessionId; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [MonitoringDescription("ProcessStandardError")]
        public StreamReader StandardError { get { return _Process.StandardError; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessStandardInput")]
        [Browsable(false)]
        public StreamWriter StandardInput { get { return _Process.StandardInput; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessStandardOutput")]
        [Browsable(false)]
        public StreamReader StandardOutput { get { return _Process.StandardOutput; } }
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
        [Browsable(false)]
        [MonitoringDescription("ProcessStartInfo")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ProcessStartInfo StartInfo { get { return _Process.StartInfo; } set { _Process.StartInfo = value; } }
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
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.StartTime property
        //     for a process that is running on a remote computer. This property is available
        //     only for processes that are running on the local computer.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessStartTime")]
        public DateTime StartTime { get { return _Process.StartTime; } }
        //
        // Summary:
        //     Gets or sets the object used to marshal the event handler calls that are
        //     issued as a result of a process exit event.
        //
        // Returns:
        //     The System.ComponentModel.ISynchronizeInvoke used to marshal event handler
        //     calls that are issued as a result of an System.Diagnostics.Process.Exited
        //     event on the process.
        [MonitoringDescription("ProcessSynchronizingObject")]
        [Browsable(false)]
        [DefaultValue("")]
        public ISynchronizeInvoke SynchronizingObject { get { return _Process.SynchronizingObject; } set { _Process.SynchronizingObject = value; } }
        //
        // Summary:
        //     Gets the set of threads that are running in the associated process.
        //
        // Returns:
        //     An array of type System.Diagnostics.ProcessThread representing the operating
        //     system threads currently running in the associated process.
        //
        // Exceptions:
        //   System.SystemException:
        //     The process does not have an System.Diagnostics.Process.Id, or no process
        //     is associated with the System.Diagnostics.Process instance.-or- The associated
        //     process has exited.
        //
        //   System.PlatformNotSupportedException:
        //     The platform is Windows 98 or Windows Millennium Edition (Windows Me); set
        //     System.Diagnostics.ProcessStartInfo.UseShellExecute to false to access this
        //     property on Windows 98 and Windows Me.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [MonitoringDescription("ProcessThreads")]
        public ProcessThreadCollection Threads { get { return _Process.Threads; } }
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
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.TotalProcessorTime
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        [MonitoringDescription("ProcessTotalProcessorTime")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan TotalProcessorTime { get { return _Process.TotalProcessorTime; } }
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
        //
        //   System.NotSupportedException:
        //     You are attempting to access the System.Diagnostics.Process.UserProcessorTime
        //     property for a process that is running on a remote computer. This property
        //     is available only for processes that are running on the local computer.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessUserProcessorTime")]
        public TimeSpan UserProcessorTime { get { return _Process.UserProcessorTime; } }
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
        [ComVisible(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessVirtualMemorySize")]
        public long VirtualMemorySize64 { get { return _Process.VirtualMemorySize64; } }
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [MonitoringDescription("ProcessWorkingSet")]
        [ComVisible(false)]
        public long WorkingSet64 { get { return _Process.WorkingSet64; } }

        // Summary:
        //     Occurs when an application writes to its redirected System.Diagnostics.Process.StandardError
        //     stream.
        [Browsable(true)]
        [MonitoringDescription("ProcessAssociated")]
        public event DataReceivedEventHandler ErrorDataReceived { add { _Process.ErrorDataReceived += value; } remove { _Process.ErrorDataReceived -= value; } }
        //
        // Summary:
        //     Occurs when a process exits.
        [Category("Behavior")]
        [MonitoringDescription("ProcessExited")]
        public event EventHandler Exited { add { _Process.Exited += value; } remove { _Process.Exited -= value; } }
        //
        // Summary:
        //     Occurs when an application writes to its redirected System.Diagnostics.Process.StandardOutput
        //     stream.
        [Browsable(true)]
        [MonitoringDescription("ProcessAssociated")]
        public event DataReceivedEventHandler OutputDataReceived { add { _Process.OutputDataReceived += value; } remove { _Process.OutputDataReceived -= value; } }

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
        [ComVisible(false)]
        public void BeginErrorReadLine() { _Process.BeginErrorReadLine(); }
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
        [ComVisible(false)]
        public void BeginOutputReadLine() { _Process.BeginOutputReadLine(); }
        //
        // Summary:
        //     Cancels the asynchronous read operation on the redirected System.Diagnostics.Process.StandardError
        //     stream of an application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardError stream is not enabled for asynchronous
        //     read operations.
        [ComVisible(false)]
        public void CancelErrorRead() { _Process.CancelErrorRead(); }
        //
        // Summary:
        //     Cancels the asynchronous read operation on the redirected System.Diagnostics.Process.StandardOutput
        //     stream of an application.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Diagnostics.Process.StandardOutput stream is not enabled for asynchronous
        //     read operations.
        [ComVisible(false)]
        public void CancelOutputRead() { _Process.CancelOutputRead(); }
        //
        // Summary:
        //     Frees all the resources that are associated with this component.
        public void Close() { _Process.Close(); }
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
        //
        //   System.InvalidOperationException:
        //     The process has already exited. -or-No process is associated with this System.Diagnostics.Process
        //     object.
        public bool CloseMainWindow() { return _Process.CloseMainWindow(); }
        //
        // Summary:
        //     Gets a new System.Diagnostics.Process component and associates it with the
        //     currently active process.
        //
        // Returns:
        //     A new System.Diagnostics.Process component associated with the process resource
        //     that is running the calling application.
        public static SC.Core.Process GetCurrentProcess() { return new Process(System.Diagnostics.Process.GetCurrentProcess()); }
        //
        // Summary:
        //     Returns a new System.Diagnostics.Process component, given the identifier
        //     of a process on the local computer.
        //
        // Parameters:
        //   processId:
        //     The system-unique identifier of a process resource.
        //
        // Returns:
        //     A System.Diagnostics.Process component that is associated with the local
        //     process resource identified by the processId parameter.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The process specified by the processId parameter is not running. The identifier
        //     might be expired.
        public static SC.Core.Process GetProcessById(int processId) { return new Process(System.Diagnostics.Process.GetProcessById(processId)); }
        //
        // Summary:
        //     Returns a new System.Diagnostics.Process component, given a process identifier
        //     and the name of a computer on the network.
        //
        // Parameters:
        //   processId:
        //     The system-unique identifier of a process resource.
        //
        //   machineName:
        //     The name of a computer on the network.
        //
        // Returns:
        //     A System.Diagnostics.Process component that is associated with a remote process
        //     resource identified by the processId parameter.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The process specified by the processId parameter is not running. The identifier
        //     might be expired.-or- The machineName parameter syntax is invalid. The name
        //     might have length zero (0).
        //
        //   System.ArgumentNullException:
        //     The machineName parameter is null.
        public static Process GetProcessById(int processId, string machineName) { return new SC.Core.Process(System.Diagnostics.Process.GetProcessById(processId, machineName)); }
        //
        // Summary:
        //     Creates a new System.Diagnostics.Process component for each process resource
        //     on the local computer.
        //
        // Returns:
        //     An array of type System.Diagnostics.Process that represents all the process
        //     resources running on the local computer.
        public static SC.Core.Process[] GetProcesses()
        {
            return Wrap(System.Diagnostics.Process.GetProcesses());
        }

        private static Process[] Wrap(System.Diagnostics.Process[] processes)
        {
            SC.Core.Process[] ret = new SC.Core.Process[processes.Length];
            for (int i = 0; i < processes.Length; ++i)
            {
                ret[i] = new SC.Core.Process(processes[i]);
            }
            return ret;
        }
        //
        // Summary:
        //     Creates a new System.Diagnostics.Process component for each process resource
        //     on the specified computer.
        //
        // Parameters:
        //   machineName:
        //     The computer from which to read the list of processes.
        //
        // Returns:
        //     An array of type System.Diagnostics.Process that represents all the process
        //     resources running on the specified computer.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The machineName parameter syntax is invalid. It might have length zero (0).
        //
        //   System.ArgumentNullException:
        //     The machineName parameter is null.
        //
        //   System.PlatformNotSupportedException:
        //     The operating system platform does not support this operation on remote computers.
        //
        //   System.InvalidOperationException:
        //     There are problems accessing the performance counter API's used to get process
        //     information. This exception is specific to Windows NT, Windows 2000, and
        //     Windows XP.
        //
        //   System.ComponentModel.Win32Exception:
        //     A problem occurred accessing an underlying system API.
        public static Process[] GetProcesses(string machineName) { return Wrap(System.Diagnostics.Process.GetProcesses(machineName)); }
        //
        // Summary:
        //     Creates an array of new System.Diagnostics.Process components and associates
        //     them with all the process resources on the local computer that share the
        //     specified process name.
        //
        // Parameters:
        //   processName:
        //     The friendly name of the process.
        //
        // Returns:
        //     An array of type System.Diagnostics.Process that represents the process resources
        //     running the specified application or file.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     There are problems accessing the performance counter API's used to get process
        //     information. This exception is specific to Windows NT, Windows 2000, and
        //     Windows XP.
        public static Process[] GetProcessesByName(string processName) { return Wrap(System.Diagnostics.Process.GetProcessesByName(processName)); }
        //
        // Summary:
        //     Creates an array of new System.Diagnostics.Process components and associates
        //     them with all the process resources on a remote computer that share the specified
        //     process name.
        //
        // Parameters:
        //   processName:
        //     The friendly name of the process.
        //
        //   machineName:
        //     The name of a computer on the network.
        //
        // Returns:
        //     An array of type System.Diagnostics.Process that represents the process resources
        //     running the specified application or file.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The machineName parameter syntax is invalid. It might have length zero (0).
        //
        //   System.ArgumentNullException:
        //     The machineName parameter is null.
        //
        //   System.PlatformNotSupportedException:
        //     The operating system platform does not support this operation on remote computers.
        //
        //   System.InvalidOperationException:
        //     There are problems accessing the performance counter API's used to get process
        //     information. This exception is specific to Windows NT, Windows 2000, and
        //     Windows XP.
        //
        //   System.ComponentModel.Win32Exception:
        //     A problem occurred accessing an underlying system API.
        public static Process[] GetProcessesByName(string processName, string machineName) { return Wrap(System.Diagnostics.Process.GetProcessesByName(processName, machineName)); }
        //
        // Summary:
        //     Immediately stops the associated process.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     The associated process could not be terminated. -or-The process is terminating.-or-
        //     The associated process is a Win16 executable.
        //
        //   System.NotSupportedException:
        //     You are attempting to call System.Diagnostics.Process.Kill() for a process
        //     that is running on a remote computer. The method is available only for processes
        //     running on the local computer.
        //
        //   System.InvalidOperationException:
        //     The process has already exited. -or-There is no process associated with this
        //     System.Diagnostics.Process object.
        public void Kill() { try { _Process.Kill(); } catch (InvalidOperationException) { /*Process already exited */ } }
        //
        // Summary:
        //     Discards any information about the associated process that has been cached
        //     inside the process component.
        public void Refresh() { _Process.Refresh(); }
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
        //   System.InvalidOperationException:
        //     No file name was specified in the System.Diagnostics.Process component's
        //     System.Diagnostics.Process.StartInfo.-or- The System.Diagnostics.ProcessStartInfo.UseShellExecute
        //     member of the System.Diagnostics.Process.StartInfo property is true while
        //     System.Diagnostics.ProcessStartInfo.RedirectStandardInput, System.Diagnostics.ProcessStartInfo.RedirectStandardOutput,
        //     or System.Diagnostics.ProcessStartInfo.RedirectStandardError is true.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        public bool Start() { return _Process.Start(); }
        //
        // Summary:
        //     Starts the process resource that is specified by the parameter containing
        //     process start information (for example, the file name of the process to start)
        //     and associates the resource with a new System.Diagnostics.Process component.
        //
        // Parameters:
        //   startInfo:
        //     The System.Diagnostics.ProcessStartInfo that contains the information that
        //     is used to start the process, including the file name and any command-line
        //     arguments.
        //
        // Returns:
        //     A new System.Diagnostics.Process component that is associated with the process
        //     resource, or null if no process resource is started (for example, if an existing
        //     process is reused).
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     No file name was specified in the startInfo parameter's System.Diagnostics.ProcessStartInfo.FileName
        //     property.-or- The System.Diagnostics.ProcessStartInfo.UseShellExecute property
        //     of the startInfo parameter is true and the System.Diagnostics.ProcessStartInfo.RedirectStandardInput,
        //     System.Diagnostics.ProcessStartInfo.RedirectStandardOutput, or System.Diagnostics.ProcessStartInfo.RedirectStandardError
        //     property is also true.-or-The System.Diagnostics.ProcessStartInfo.UseShellExecute
        //     property of the startInfo parameter is true and the System.Diagnostics.ProcessStartInfo.UserName
        //     property is not null or empty or the System.Diagnostics.ProcessStartInfo.Password
        //     property is not null.
        //
        //   System.ArgumentNullException:
        //     The startInfo parameter is null.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        public static Process Start(ProcessStartInfo startInfo) { return new SC.Core.Process(System.Diagnostics.Process.Start(startInfo)); }
        //
        // Summary:
        //     Starts a process resource by specifying the name of a document or application
        //     file and associates the resource with a new System.Diagnostics.Process component.
        //
        // Parameters:
        //   fileName:
        //     The name of a document or application file to run in the process.
        //
        // Returns:
        //     A new System.Diagnostics.Process component that is associated with the process
        //     resource, or null, if no process resource is started (for example, if an
        //     existing process is reused).
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        //
        //   System.IO.FileNotFoundException:
        //     The PATH environment variable has a string containing quotes.
        public static Process Start(string fileName) { return new SC.Core.Process(System.Diagnostics.Process.Start(fileName)); }
        //
        // Summary:
        //     Starts a process resource by specifying the name of an application and a
        //     set of command-line arguments, and associates the resource with a new System.Diagnostics.Process
        //     component.
        //
        // Parameters:
        //   fileName:
        //     The name of an application file to run in the process.
        //
        //   arguments:
        //     Command-line arguments to pass when starting the process.
        //
        // Returns:
        //     A new System.Diagnostics.Process component that is associated with the process,
        //     or null, if no process resource is started (for example, if an existing process
        //     is reused).
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The fileName or arguments parameter is null.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        //
        //   System.IO.FileNotFoundException:
        //     The PATH environment variable has a string containing quotes.
        public static Process Start(string fileName, string arguments) { return new SC.Core.Process(System.Diagnostics.Process.Start(fileName, arguments)); }
        //
        // Summary:
        //     Starts a process resource by specifying the name of an application, a user
        //     name, a password, and a domain and associates the resource with a new System.Diagnostics.Process
        //     component.
        //
        // Parameters:
        //   fileName:
        //     The name of an application file to run in the process.
        //
        //   userName:
        //     The user name to use when starting the process.
        //
        //   password:
        //     A System.Security.SecureString that contains the password to use when starting
        //     the process.
        //
        //   domain:
        //     The domain to use when starting the process.
        //
        // Returns:
        //     A new System.Diagnostics.Process component that is associated with the process
        //     resource, or null if no process resource is started (for example, if an existing
        //     process is reused).
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     No file name was specified.
        //
        //   System.ComponentModel.Win32Exception:
        //     fileName is not an executable (.exe) file.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        public static Process Start(string fileName, string userName, SecureString password, string domain) { return new SC.Core.Process(System.Diagnostics.Process.Start(fileName, userName, password, domain)); }
        //
        // Summary:
        //     Starts a process resource by specifying the name of an application, a set
        //     of command-line arguments, a user name, a password, and a domain and associates
        //     the resource with a new System.Diagnostics.Process component.
        //
        // Parameters:
        //   fileName:
        //     The name of an application file to run in the process.
        //
        //   arguments:
        //     Command-line arguments to pass when starting the process.
        //
        //   userName:
        //     The user name to use when starting the process.
        //
        //   password:
        //     A System.Security.SecureString that contains the password to use when starting
        //     the process.
        //
        //   domain:
        //     The domain to use when starting the process.
        //
        // Returns:
        //     A new System.Diagnostics.Process component that is associated with the process
        //     resource, or null if no process resource is started (for example, if an existing
        //     process is reused).
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     No file name was specified.
        //
        //   System.ComponentModel.Win32Exception:
        //     fileName is not an executable (.exe) file.
        //
        //   System.ComponentModel.Win32Exception:
        //     There was an error in opening the associated file.
        //
        //   System.ObjectDisposedException:
        //     The process object has already been disposed.
        public static Process Start(string fileName, string arguments, string userName, SecureString password, string domain) { return new SC.Core.Process(System.Diagnostics.Process.Start(fileName, arguments, userName, password, domain)); }
        //
        // Summary:
        //     Formats the process's name as a string, combined with the parent component
        //     type, if applicable.
        //
        // Returns:
        //     The System.Diagnostics.Process.ProcessName, combined with the base component's
        //     System.Object.ToString() return value.
        //
        // Exceptions:
        //   System.PlatformNotSupportedException:
        //     System.Diagnostics.Process.ToString() is not supported on Windows 98.
        public override string ToString() { return _Process.ToString(); }
        //
        // Summary:
        //     Instructs the System.Diagnostics.Process component to wait indefinitely for
        //     the associated process to exit.
        //
        // Exceptions:
        //   System.ComponentModel.Win32Exception:
        //     The wait setting could not be accessed.
        //
        //   System.SystemException:
        //     No process System.Diagnostics.Process.Id has been set, and a System.Diagnostics.Process.Handle
        //     from which the System.Diagnostics.Process.Id property can be determined does
        //     not exist.-or- There is no process associated with this System.Diagnostics.Process
        //     object.-or- You are attempting to call System.Diagnostics.Process.WaitForExit()
        //     for a process that is running on a remote computer. This method is available
        //     only for processes that are running on the local computer.
        public void WaitForExit() { _Process.WaitForExit(); }
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
        //   System.ComponentModel.Win32Exception:
        //     The wait setting could not be accessed.
        //
        //   System.SystemException:
        //     No process System.Diagnostics.Process.Id has been set, and a System.Diagnostics.Process.Handle
        //     from which the System.Diagnostics.Process.Id property can be determined does
        //     not exist.-or- There is no process associated with this System.Diagnostics.Process
        //     object.-or- You are attempting to call System.Diagnostics.Process.WaitForExit(System.Int32)
        //     for a process that is running on a remote computer. This method is available
        //     only for processes that are running on the local computer.
        public bool WaitForExit(int milliseconds) { return _Process.WaitForExit(milliseconds); }
        //
        // Summary:
        //     Causes the System.Diagnostics.Process component to wait indefinitely for
        //     the associated process to enter an idle state. This overload applies only
        //     to processes with a user interface and, therefore, a message loop.
        //
        // Returns:
        //     true if the associated process has reached an idle state; otherwise, false.
        public bool WaitForInputIdle() { return _Process.WaitForInputIdle(); }
        //
        // Summary:
        //     Causes the System.Diagnostics.Process component to wait the specified number
        //     of milliseconds for the associated process to enter an idle state. This overload
        //     applies only to processes with a user interface and, therefore, a message
        //     loop.
        //
        // Parameters:
        //   milliseconds:
        //     A value of 1 to System.Int32.MaxValue that specifies the amount of time,
        //     in milliseconds, to wait for the associated process to become idle. A value
        //     of 0 specifies an immediate return, and a value of -1 specifies an infinite
        //     wait.
        //
        // Returns:
        //     true if the associated process has reached an idle state; otherwise, false.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     An unknown error occurred. The process failed to enter an idle state.
        public bool WaitForInputIdle(int milliseconds) { return _Process.WaitForInputIdle(milliseconds); }

        public void Dispose() { _Process.Dispose(); }
    }
}
